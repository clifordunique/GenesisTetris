using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.Tetris
{
    [CreateAssetMenu(fileName = "LevelSettings", menuName = "Tetris/LevelSettings", order = 2)]
    public class LevelSettings : ScriptableObject
    {

        public string Name;
        public List<ModelElement> Blocks;
    }
}
