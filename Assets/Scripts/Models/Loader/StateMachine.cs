using System;
using System.Collections.Generic;

namespace Genesis.Tetris
{
    public enum GameState : byte
    {
        Undefined,
        MainMenu,
        Level
    }
    public class StateMachine
    {

        public GameState State { get; private set; }
        private readonly Dictionary<GameState, Action[]> States;

        public StateMachine()
        {
            States = new Dictionary<GameState, Action[]>();
        }

        public void Add(GameState state, Action start, Action update, Action exit)
        {
            States.Add(state, new Action[3] { start, update, exit });
        }

        public void SwitchState(GameState state)
        {
            if (state != State)
            {
                if (States.ContainsKey(State))
                {
                    States[State][2]?.Invoke();
                }
                State = state;
                if (States.ContainsKey(state))
                {
                    States[state][0]?.Invoke();
                }
            }
        }

        public void Update()
        {
            if (States.ContainsKey(State))
            {
                States[State][1]?.Invoke();
            }
        }
    }
}
