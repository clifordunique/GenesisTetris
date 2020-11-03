using System;

namespace Genesis.Tetris
{
    public class EventManager
    {
        // singleton
        private static EventManager _instance;
        public static EventManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EventManager();
                }
                return _instance;
            }
        }

        public Action<ApplicationCommand> Events;
    }
}
