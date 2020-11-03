using System.Collections.Generic;
using UnityEngine;

namespace Genesis.Tetris
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Tetris/GameSettings", order = 1)]
    public class GameSettings : ScriptableObject
    {

        static public GameSettings Instance;
        public string ApplicationName;
        public GameObject BlockPrefab;
        public List<LevelSettings> Levels;

        [HideInInspector] public int CurrentLevel = 0;
        [HideInInspector] public float BlockSize = 0.315f;

        public GameSettings()
        {
            Instance = this;
        }
    }
}
