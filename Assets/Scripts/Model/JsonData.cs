using System.Collections.Generic;

namespace ToonBlast.Model
{
    [System.Serializable]
    public struct Scores {
        public List<LevelScore> levelScores;
    }
    [System.Serializable]
    public class LevelScore {
        public int levelID;
        public int score;
        public List<int> individualColorScore;
        public int attempted;

        public LevelScore(int levelID, int score,  List<int> individualColorScore, int attempted) {
            this.levelID = levelID;
            this.attempted = attempted;
            this.individualColorScore = individualColorScore;
            this.score = score;
        }

        public void Reset()
        {
            attempted = 0;
            score = 0;
            individualColorScore.Clear();
            for (var i = 0; i < System.Enum.GetNames(typeof(PieceTypes)).Length; i++) {
                individualColorScore.Add(0);
            }
            
        }

    }
    [System.Serializable]
    public struct LevelDesigns {
        public List<LevelDesign> levels;
    }

    [System.Serializable]
    public class LevelDesign {
        public int levelNumber;
        public int width;
        public int height;
        public List<int> pieceTypes = new List<int>();
    }


}


