using System;
using System.IO;
using UnityEngine;

namespace Genesis.Tetris
{
    [RequireComponent(typeof(GameView), typeof(InputTouchController))]
    public class GameController : MonoBehaviour
    {
        enum GameState
        {
            Next,
            Falling,
            Collision,
            LoadProgress,
            Pause,
            Score,
            Complete
        }

        public int Width = 10;
        public int Height = 20;
        public float FallingSpeed = 0.5f;

        private readonly GameSettings Settings = GameSettings.Instance;

        private ModelGameField Field;
        private GameView View;
        private ModelFalling Falling;

        private GameState State = GameState.Next;
        private float verticalPosition = 0;

        public void Start()
        {
            View = GetComponent<GameView>();

            var inputs = GetComponents<InputController>();
            for(int i = 0; i < inputs.Length; ++i)
            {
                var input = inputs[i];
                input.HorizontalMove += OnHorizontalMove;
                input.TouchClick += OnTouchClick;
                input.InstantPlace += OnInstantPlace;
            }

            Field = new ModelGameField
            {
                Blocks = new bool[Width * Height],
                Height = Height,
                Width = Width
            };

            Falling = new ModelFalling { };
            Falling.Events += OnPositionChange;

            // continue game 
            if (Settings.CurrentLevel < 0)
            {
                State = GameState.LoadProgress;
            }

            View.BackButton.onClick.AddListener(() =>
            {
                EventManager.Instance.Events.Invoke(new ApplicationCommand 
                { 
                    Type = CommandType.MainMenu
                });
            });
        }

        public void Update()
        {
            switch (State)
            {

                case GameState.LoadProgress:
                    LoadProgress();
                    break;

                case GameState.Next:

                    var config = Settings.Levels[Settings.CurrentLevel];
                    int randomIndex = UnityEngine.Random.Range(0, config.Blocks.Count);
                    ModelElement source = config.Blocks[randomIndex];
                    Falling.CreateBlock(source);
                    Falling.SetPosition(new Vector2Int((Width - source.Width) / 2, Height));

                    View.CreateFallBlock(Falling.Element);
                    State = GameState.Falling;
                    break;

                case GameState.Falling:
                    verticalPosition += FallingSpeed * Time.deltaTime;
                    if(verticalPosition > Settings.BlockSize)
                    {
                        verticalPosition = 0;
                        Falling.VerticalMove();
                    }
                    break;
            }
        }

        private void OnHorizontalMove(int direction)
        {
            if(State == GameState.Falling)
            {
                Falling.HorizontalMove(direction);
            }           
        }

        private void OnInstantPlace()
        {
            if (State == GameState.Falling)
            {
                var ghost = Falling.Element.Clone;
                for (int i = ghost.Position.y; i >= 0; --i)
                {
                    ghost.Position.y = i;
                    if (Field.IntersectBlocks(ghost))
                    {
                        ghost.Position.y++;
                        break;
                    };
                }

                Field.SplitFallBlock(ghost);

                View.RemoveFallBlock();
                View.RedrawField(Field);

                Falling.Dispose();

                State = GameState.Next;
            }
        }

        public void OnTouchClick()
        {
            if (!Falling.IsEmpty)
            {
                Falling.Rotate(false);
            }
        }

        private void OnPositionChange(ModelPositionElement ghost)
        {

            // save level
            SaveProgress();

            // check for intersection 
            if (Field.IntersectBlocks(ghost))
            {
                // game over
                if(ghost.Position.y + ghost.Height >= Height)
                {
                    Debug.Log(ghost.Position.y);
                    State = GameState.Complete;
                    PlayerPrefs.DeleteKey(Settings.ApplicationName);
                    return;
                }                

                Field.SplitFallBlock(Falling.Element);
                View.RemoveFallBlock();
                View.RedrawField(Field);
                Falling.Dispose();

                State = GameState.Next;

                return;
            }

            if (!Field.IntersectWalls(ghost))
            {
                Falling.Element = ghost;
            }

            View.RedrawFallElement(Falling.Element);
        }

        private void LoadProgress()
        {
            // TODO: export serialize / desialize to class ??
            // errors catch ??
            string saveString = PlayerPrefs.GetString(Settings.ApplicationName);
            byte[] bArray = Convert.FromBase64String(saveString);
            using (var binary = new BinaryReader(new MemoryStream(bArray)))
            {
                Settings.CurrentLevel = binary.ReadByte();
                Field.Deserialize(binary);

                Falling.Element = new ModelPositionElement();
                Falling.Element.Deserialize(binary);

                View.RedrawField(Field);
                View.CreateFallBlock(Falling.Element);

                State = GameState.Falling;
            }
        }

        private void SaveProgress()
        {
            var bArray = new byte[2048];
            using (var binary = new BinaryWriter(new MemoryStream(bArray)))
            {
                binary.Write((byte)Settings.CurrentLevel);
                Field.Serialize(binary);
                Falling.Element.Serialize(binary);

                binary.BaseStream.Position = 0; // no need ?
                string saveString = Convert.ToBase64String(((MemoryStream)binary.BaseStream).ToArray());
                PlayerPrefs.SetString(Settings.ApplicationName, saveString);
            }
        }

        void OnApplicationQuit()
        {
            SaveProgress();
        }
    }
}
