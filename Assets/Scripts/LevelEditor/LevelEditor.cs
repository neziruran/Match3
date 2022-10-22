using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ToonBlast
{
    [CreateAssetMenu(fileName = "LevelEditor", menuName = "ScriptableObjects/LevelEditorObject", order = 1)]
    public class LevelEditor : ScriptableObject
    { 
        public int minimumPieceCount;
        public List<Level> levels = new List<Level>();
    }
    [System.Serializable]
    public class Level
    {
        public List<int> colorTargetCount;
        public int targetMoves;
    }
}