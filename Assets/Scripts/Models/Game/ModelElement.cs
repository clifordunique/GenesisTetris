using System;
using System.IO;
using UnityEngine;

namespace Genesis.Tetris
{
    [Serializable]
    public class ModelElement
    {
        public int Width;
        public int Height;
        public Color Color;
        public bool[] Blocks;

        public void EachElement(Action<int, int, bool> callback)
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    callback.Invoke(i, j, Blocks[j * Width + i]);
                }
            }
        }

        public virtual void Serialize(BinaryWriter binary)
        {
            binary.Write((byte)Width);
            binary.Write((byte)Height);

            EachElement((int i, int j, bool filled) =>
            {
                binary.Write(filled);
            });
        }

        public virtual void Deserialize(BinaryReader binary)
        {
            Width = binary.ReadByte();
            Height = binary.ReadByte();
            Blocks = new bool[Width * Height];

            EachElement((int i, int j, bool filled) =>
            {
                Blocks[j * Width + i] = binary.ReadBoolean();
            });
        }
    }
}
