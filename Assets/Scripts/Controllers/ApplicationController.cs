using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Genesis.Tetris
{
    public class ApplicationController : MonoBehaviour
    {
        enum BuildScene : int
        {
            Loader,
            Menu,
            Game
        }

        private StateMachine StateMachine;

        public GameSettings Settings;

        void Start()
        {
            // allways show background and camera
            DontDestroyOnLoad(this);

            StateMachine = new StateMachine();
            StateMachine.Add(GameState.MainMenu, MainMenuStart, null, null);
            StateMachine.Add(GameState.Level, GameStart, GameUpdate, null);

            StateMachine.SwitchState(GameState.MainMenu);

            EventManager.Instance.Events += OnApplicationEvents;
        }

        private void OnApplicationEvents(ApplicationCommand command)
        {

            switch(command.Type)
            {
                case CommandType.GameStart:
                    {
                        var _params = (GameStartParams)command.Params;
                        Settings.CurrentLevel = _params.Level;
                        StateMachine.SwitchState(GameState.Level);
                    }
                    break;

                case CommandType.MainMenu:
                    StateMachine.SwitchState(GameState.MainMenu);
                    break;

                case CommandType.SaveProgress:

                    break;
            }
        }

        // Update is called once per frame
        void MainMenuStart()
        {
            SceneManager.LoadSceneAsync((int)BuildScene.Menu);
        }

        void GameStart()
        {
            SceneManager.LoadSceneAsync((int)BuildScene.Game);
        }

        void GameUpdate()
        {

        }


        void Update()
        {
            StateMachine.Update();
        }
    }
}