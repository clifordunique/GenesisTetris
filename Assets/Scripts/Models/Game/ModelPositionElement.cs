using System.IO;
using UnityEngine;

namespace Genesis.Tetris
{
    public class ModelPositionElement : ModelElement
    {
        public Vector2Int Position;

        public ModelPositionElement()
        {

        }

        public ModelPositionElement(ModelElement val)
        {
            Width = val.Width;
            Height = val.Height;
            Color = val.Color;
            Blocks = GameUtils.FlipArray(val.Blocks, Width, Height);
        }

        public override void Serialize(BinaryWriter binary)
        {
            base.Serialize(binary);

            // position
            binary.Write((byte)Position.x);
            binary.Write((byte)Position.y);

            // color
            binary.Write(Color.r);
            binary.Write(Color.g);
            binary.Write(Color.b);
            binary.Write(Color.a);
        }

        public override void Deserialize(BinaryReader binary)
        {
            base.Deserialize(binary);

            // position
            Position.x = binary.ReadByte();
            Position.y = binary.ReadByte();

            // color
            Color.r = binary.ReadSingle();
            Color.g = binary.ReadSingle();
            Color.b = binary.ReadSingle();
            Color.a = binary.ReadSingle();
        }

        public ModelPositionElement Clone
        {
            get
            {
                return new ModelPositionElement
                {
                    Blocks = Blocks,
                    Color = Color,
                    Height = Height,
                    Width = Width,
                    Position = Position
                };
            }
        }
    }
}