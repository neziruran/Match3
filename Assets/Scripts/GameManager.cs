using System.Collections.Generic;
using System.IO;
using System.Linq;
using Model;
using ToonBlast.Model;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


namespace ToonBlast
{
    public class GameManager : MonoBehaviour
    {
        [Header("Reference for Pool Particles")]
        [SerializeField] private RectTransform[] particleUIPool;
        [SerializeField] private GameObject[] particlePool;
        
        private int particlePoolCount;
        public readonly Dictionary<int, int[,]> levels = new Dictionary<int, int[,]>();
        
        public static int currentMoves;
        public static GameState gameState;
        public static Dictionary<int, LevelScore> playerScore;
        

        private void Start() {
            LoadScoresFromTheFile();
            EventTriggers.LevelRestart += LevelRestart;
            EventTriggers.Home += GameRestart;
            EventTriggers.GamePause += GamePauseOrUnPause;
            EventTriggers.onNextLevel += NextLevel;
        }

        private void OnDisable()
        {
            EventTriggers.LevelRestart -= LevelRestart;
            EventTriggers.Home -= GameRestart;
            EventTriggers.GamePause -= GamePauseOrUnPause;
            EventTriggers.onNextLevel -= NextLevel;
        }

        /// <summary>
        /// Here we are updating UI text How many moves left and
        /// We are checking for win / fail condition
        /// </summary>
        private void UpdateCurrentMoveValueInUI()
        {
            EventTriggers.OnUpdateMoveValue();
            CheckGameStatus();
        }
       
       
         
        /// <summary>
        /// We are taking all pieces which are matching
        /// And adding particles for those piece
        /// Increasing the click count - _currentMoves
        /// Updating UI values
        /// </summary>
        /// <param name="pieces"></param>
        public void AddToCollectedPiece(ResolveResult pieces, ref PieceTypeDatabase pieceTypeDatabase)
        {
            if (pieces.changes.Count <= 1) {
                return;
            }
            foreach (var piece in pieces.changes.Keys)
            {
                var waitTime = Random.Range(1f, 1.3f);
                
                //Saving score
                playerScore[LevelSelectionManager.CurrentLevel].attempted = currentMoves + 1;
                playerScore[LevelSelectionManager.CurrentLevel].individualColorScore[piece.pieceTypeNumber]++;
                
                //Checking piece is matching to Requirement and adding it to Requirement
                foreach (var iRequirement in RequirementsManager.Requirements.Where(iRequirement => iRequirement.pieceColorNumber == piece.pieceTypeNumber &&
                             iRequirement.gameObject.activeSelf)) {
                    iRequirement.PieceCollected(waitTime);
                }   
                
                //Finding PiecePosition and adding -2 to z value to make it come in front
                var piecePos = pieces.changes[piece].FromPos;
                var pos = new Vector3(piecePos.x, -piecePos.y,-piecePos.y - 2);
                if(!piece.powerPiece)
                    FetchParticles(pos, piece.pieceTypeNumber, waitTime, ref pieceTypeDatabase);
            }
            currentMoves++;
            UpdateCurrentMoveValueInUI();
        }
        /// <summary>
        /// Checking for game win using Requirements from LevelSelectionManager
        /// </summary>
        /// <returns></returns>
        private static bool CheckGameWin()
        {
            return RequirementsManager.Requirements.Where(iRequirement => iRequirement.gameObject.activeInHierarchy).All(irequirement => irequirement.IsCompleted);
        }
        /// <summary>
        /// When we need Particles we can send position, piece type, and what is the wait time for particle
        /// Here we are adding particle in required position changing the texture to piece type
        /// And Tweening to Requirement position in UI
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="type"></param>
        /// <param name="waitTime"></param>
        private void FetchParticles(Vector3 pos,int type, float waitTime, ref PieceTypeDatabase pieceTypeDatabase)
        {
            if (particlePoolCount >= particlePool.Length) {
                particlePoolCount = 0;
            }
            particlePool[particlePoolCount].transform.position = pos;
            particlePool[particlePoolCount].GetComponent<ParticleSystem>().Play();
            
            var targetRect  = RequirementsManager.GetPositionForRequirement(type)?.GetComponent<RectTransform>();
            if(targetRect != null && targetRect.gameObject.activeSelf && !targetRect.GetComponent<Requirement>().IsCompletedForUI) {
                
                particleUIPool[particlePoolCount].transform.position = pos;
                particleUIPool[particlePoolCount].GetComponent<Image>().sprite = pieceTypeDatabase.GetSpriteForPieceType(type);
                
                var finalPosition = targetRect.parent.localPosition + targetRect.localPosition;
                Tween.MoveParticle( particleUIPool[particlePoolCount], new Vector3(finalPosition.x, finalPosition.y,10), waitTime, 0, this);
            }
            particlePoolCount++;
        }

        private void CheckGameStatus()
        {
            var checkGameStatus = CheckGameWin();
            if (LevelSelectionManager.Instance.GetTotalMovesCount() <= currentMoves)
            {
                if (checkGameStatus)
                {
                   // GameWin();
                   gameState = GameState.Win;
                   EventTriggers.GameWin();
                }
                else
                {
                   // GameFail();
                   gameState = GameState.Lose;
                   EventTriggers.GameLoss();
                }
            }
            else
            {
                if (checkGameStatus)
                {
                    // GameWin();
                    gameState = GameState.Win;
                    EventTriggers.GameWin();
                }
            }
        }

          
        /// <summary>
        /// Game Restart will make current click count to "0"
        /// and Restarting all Requirements
        /// </summary>
        public void GameRestart() {
            currentMoves = 0;
            RequirementsManager.ClearRequirements();
        }

        public void LevelRestart()
        {
            gameState = GameState.Started;
            currentMoves = 0;
            UpdateCurrentMoveValueInUI();
        }
        public void NextLevel()
        {
            gameState = GameState.Started;
            currentMoves = 0;
        }


        public void GamePauseOrUnPause()
        {
            gameState = (gameState == GameState.Paused) ? GameState.Started : GameState.Paused;
        }

        /// <summary>
        /// Once Game state is win or fail
        /// We are writing to Json
        /// Saving level score values to "Match3GameScore.txt" in project folder
        /// </summary>
        private static void WriteToJson()
        {
            var scores = new Scores {
                levelScores = new List<LevelScore>()
            };
            foreach (var item in playerScore) {
                scores.levelScores.Add(new LevelScore(item.Key,item.Value.score,item.Value.individualColorScore, item.Value.attempted ));
            }
            var score = new Scores(){levelScores = scores.levelScores};
            var val = JsonUtility.ToJson(score);
            File.WriteAllText(Application.dataPath+"/Match3GameScore.txt",val);
        }
        /// <summary>
        /// When Game starts we are loading scores from text file "Match3GameScore.txt"
        /// </summary>
        private void LoadScoresFromTheFile()
        {
            var fileName = Application.dataPath + "/Match3GameScore.txt";
            if(File.Exists(fileName)) {
                Scores scores;
                using (var streamReader = File.OpenText(fileName)) {
                    var text = streamReader.ReadToEnd();
                    if (string.IsNullOrEmpty(text)) {
                        scores = new Scores
                        {
                            levelScores = new List<LevelScore>()
                        };
                    }
                    else {
                        scores = JsonUtility.FromJson<Scores>(text);    
                    }
                    
                }
                playerScore = new Dictionary<int, LevelScore>(scores.levelScores.Count);
                foreach (var item in scores.levelScores.Where(item => !playerScore.ContainsKey(item.levelID))) {
                    playerScore.Add(item.levelID, item);
                }
            }
            //MenuAnimation();
            EventTriggers.MenuAnimation();
        }
        
        public void LoadLevelsFromFile()
        {
            var fileName = Application.dataPath + "/Levels.txt";
            if(File.Exists(fileName)) {
                LevelDesigns scores;
                using (var streamReader = File.OpenText(fileName)) {
                    var text = streamReader.ReadToEnd();
                    if (string.IsNullOrEmpty(text)) {
                        scores = new LevelDesigns() {
                            levels = new List<LevelDesign>()
                        };
                    } else {
                        scores = JsonUtility.FromJson<LevelDesigns>(text);    
                    }
                }
				
                foreach (var item in scores.levels)
                {
                    var data = new int[item.width,item.height];
                    for (var i = 0; i < item.width; i++)
                    {
                        for (var j = 0; j < item.height; j++)
                        {
                            data[i, j] = item.pieceTypes[i * item.width + j];
                        }
                    }
                    if(!levels.ContainsKey(item.levelNumber))
                        levels.Add(item.levelNumber, data);
                }
            }
        }
        
    }

    public enum GameState
    {
        Started,
        Paused,
        Win,
        Lose
    }
    public enum PieceTypes
    { 
        Blue,
        Orange,
        Pink,
        Red,
        Yellow
    }
    
    public enum SpecialPieceTypes
    {
        HorizontalPower,
        VerticalPower,
        BombPower
    }
}