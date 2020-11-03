using System;

namespace Genesis.Tetris
{
    public class GameUtils
    {
        static public bool[] Rotate(bool clockwise, bool[] values, int width, int height)
        {
            bool[] result;

            result = new bool[values.Length];

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    int index;

                    index = row * width + col;

                    if (values[index])
                    {
                        int newRow;
                        int newCol;
                        int newIndex;

                        if (clockwise)
                        {
                            newRow = col;
                            newCol = height - (row + 1);
                        }
                        else
                        {
                            newRow = width - (col + 1);
                            newCol = row;
                        }

                        newIndex = newRow * height + newCol;

                        result[newIndex] = true;
                    }
                }
            }

            return result;
        }

        internal static void CopyRowUnsafe(bool[] src, int i, bool[] dsn, int j, int len)
        {
            for (int k = 0; k < len; ++k)
            {
                dsn[j + k] = src[i + k];
            }

        }

        internal static bool[] FlipArray()
        {
            throw new NotImplementedException();
        }

        public static bool[] FlipArray(bool[] blocks, int width, int height)
        {
            var result = new bool[width * height];

            for (int j = 0; j < height; ++j)
            {
                for (int i = 0; i < width; ++i)
                {
                    result[(height - j - 1) * width + i] = blocks[j * width + i];
                }
            }

            return result;
        }
    }

}