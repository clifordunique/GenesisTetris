using System;
using System.IO;

namespace Genesis.Tetris
{
    public enum CommandType
    {
        GameStart,
        MainMenu,
        SaveProgress
    }

    public class ApplicationCommand
    {
        public CommandType Type;
        public CommandParams Params;
    }

    public abstract class CommandParams
    {
    }

    public class GameStartParams : CommandParams
    {
        public int Level;
    }

    public class SaveProgressParams : CommandParams
    {
        public int Level;
        public int Score;
        public ModelGameField Field;
        public ModelPositionElement Block;
    }

}
