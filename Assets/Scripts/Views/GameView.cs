using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Genesis.Tetris
{
    public class GameView : MonoBehaviour
    {

        public GameObject FallArea;
        public GameObject BackgroundArea;
        public GameObject LevelObject;
        public Button BackButton;

        private GameController m_Controller;
        private GameObject FallenBlock;

        private float BlockSize = GameSettings.Instance.BlockSize;
        private GameObject[] m_Pieces;
        private Dictionary<int, GameObject> m_Elements;

        public void Start()
        {
            m_Elements = new Dictionary<int, GameObject>();
            m_Controller = GetComponent<GameController>();

            transform.position = Vector3.zero;

            // add new 
            for (int i = 0; i < m_Controller.Width; ++i)
            {
                for (int k = 0; k < m_Controller.Height; ++k)
                {
                    GameObject cell = UnityEngine.GameObject.Instantiate(GameSettings.Instance.BlockPrefab, BackgroundArea.transform);
                    cell.transform.localPosition = new Vector3(i * BlockSize, k * BlockSize, 0);
                    m_Elements.Add(k * m_Controller.Width + i, cell);
                }
            }

            LevelObject.transform.position = new Vector3(
                -m_Controller.Width * BlockSize * 0.5f + BlockSize * 0.5f,
                -4.33f,
                0
            );
        }

        public void RedrawFallElement(ModelPositionElement element)
        {
            FallenBlock.transform.localPosition = new Vector3(
                element.Position.x * BlockSize,
                element.Position.y * BlockSize
            );

            int index = 0;
            element.EachElement((int i, int j, bool filled) =>
            {
                if (filled)
                {
                    var piece = m_Pieces[index++];
                    piece.transform.localPosition = new Vector3(i * BlockSize, j * BlockSize, 0);
                }
            });
        }

        public void RemoveFallBlock()
        {
            DestroyImmediate(FallenBlock);
            FallenBlock = null;
        }

        public void RedrawField(ModelGameField field)
        {
            field.EachElement((int i, int j, bool filled) =>
            {
                int index = j * m_Controller.Width + i;
                var element = m_Elements[index];
                SpriteRenderer sprite = element.GetComponent<SpriteRenderer>();
                sprite.color = filled ? Color.cyan : new Color(1f, 1f, 1f, 0.078f);

            });
        }

        public void ChangeElement(int index, Color color)
        {
            if (m_Elements.ContainsKey(index))
            {

            }
        }

        public void CreateFallBlock(ModelPositionElement element)
        {
            FallenBlock = new GameObject("FallenBlock");
            FallenBlock.transform.parent = FallArea.transform;

            var pieces = new List<GameObject>();
            element.EachElement((int i, int j, bool filled) =>
            {
                if (filled)
                {

                    var piece = Instantiate(GameSettings.Instance.BlockPrefab, FallenBlock.transform);
                    piece.transform.localPosition = new Vector3(i * BlockSize, j * BlockSize, 0);

                    var render = piece.GetComponent<SpriteRenderer>();
                    render.color = element.Color;

                    pieces.Add(piece);
                }
            });

            // save pieces for rotation
            m_Pieces = pieces.ToArray();

            RedrawFallElement(element);
        }
    }

}
