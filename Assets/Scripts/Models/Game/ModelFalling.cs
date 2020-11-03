using System;
using UnityEngine;

namespace Genesis.Tetris
{
    public class ModelFalling
    {

        public ModelPositionElement Element;
        public Action<ModelPositionElement> Events;

        public bool IsEmpty { get { return Element == null; } }

        public void CreateBlock(ModelElement val)
        {
            Element = new ModelPositionElement(val);
        }

        public void SetPosition(Vector2Int val)
        {
            Element.Position = val;
        }

        public void Dispose()
        {
            Element = null;
        }

        public void VerticalMove()
        {
            ModelPositionElement ghost = Element.Clone;
            ghost.Position += new Vector2Int(0, -1);
            Events.Invoke(ghost);
        }

        public void HorizontalMove(int delta)
        {
            ModelPositionElement ghost = Element.Clone;
            ghost.Position += new Vector2Int(delta, 0);
            Events.Invoke(ghost);
        }

        public void Rotate(bool clockwise)
        {
            ModelPositionElement ghost = Element.Clone;

            ghost.Blocks = GameUtils.Rotate(clockwise, Element.Blocks, Element.Width, Element.Height);
            ghost.Width = Element.Height;
            ghost.Height = Element.Width;

            Events.Invoke(ghost);
        }

    }
}
