using System;
using System.IO;
using UnityEngine;

namespace Genesis.Tetris
{
    public class ModelGameField : ModelElement
    {
        public void SplitFallBlock(ModelPositionElement element)
        {
            // fill element
            element.EachElement((int i, int j, bool filled) =>
            {
                if (filled)
                {
                    var position = element.Position + new Vector2Int(i, j);
                    int index = position.y * Width + position.x;
                    Blocks[index] = true;
                }
            });

            // check rows
            var cloneBblocks = new bool[Height * Width];
            int rowIndex = 0;
            for (int j = 0; j < Height; ++j)
            {
                var complete = 0;
                for (int i = 0; i < Width; ++i)
                {
                    if (Blocks[j * Width + i])
                    {
                        complete++;
                    }
                }
                if (complete < Width)
                {
                    GameUtils.CopyRowUnsafe(Blocks, j * Width, cloneBblocks, Width * rowIndex++, Width);
                }
            }
            Blocks = cloneBblocks;
        }

        private bool IntersectBottom(ModelPositionElement element)
        {
            return element.Position.y < 0;
        }

        public bool IntersectWalls(ModelPositionElement element)
        {
            if (element.Position.x >= 0)
            {
                if (element.Position.x + element.Width <= Width)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IntersectBlocks(ModelPositionElement element)
        {
            if (IntersectBottom(element))
                return true;

            bool intersect = false;

            element.EachElement((int i, int j, bool filled) =>
            {
                if (filled)
                {
                    var position = element.Position + new Vector2Int(i, j);
                    var index = position.y * Width + position.x;
                    if (index >= 0)
                    {
                        if (index < Blocks.Length)
                        {
                            if (Blocks[index])
                            {
                                intersect = true;
                            }
                        }
                    }
                }
            });

            return intersect;
        }

    }
}