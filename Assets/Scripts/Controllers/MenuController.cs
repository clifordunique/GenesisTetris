using UnityEngine;

namespace Genesis.Tetris
{
    [RequireComponent(typeof(MenuView))]
    public class MenuController : MonoBehaviour
    {
        private MenuView View;

        void Start()
        {
            View = GetComponent<MenuView>();
            bool fromSave = PlayerPrefs.HasKey(GameSettings.Instance.ApplicationName);

            View.InitButtons(GameSettings.Instance.Levels, fromSave, (int index) =>
            {
                EventManager.Instance.Events.Invoke(new ApplicationCommand 
                { 
                    Type = CommandType.GameStart,
                    Params = new GameStartParams 
                    { 
                        Level = index 
                    }
                });
            });
        }

    }
}
