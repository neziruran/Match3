using System.Collections.Generic;
using System.Linq;
using Match3.Model;
using Model;
using UnityEngine;
using UnityEngine.UI;

namespace Match3
{
    public class LevelSelectionManager : MonoBehaviour
    {
        [Header("Level Editor for fetching Levels from Scriptable object")]
        public LevelEditor levelEditor;
        
        [Header("Requirement prefab for showing requirements in Game Page")]
        [SerializeField] private GameObject requirement;
        [Header("Parent for Requirement prefab After Initializing")]
        [SerializeField] private GameObject levelRequirementLocation;
        [Header("Board to activate once level is selected")]
        [SerializeField] private Boot board;
        [Header("PieceTypeDatabase prefab for fetching sprite")]
        [SerializeField] public PieceTypeDatabase pieceTypeDatabase;

        public static int CurrentLevel;
        

        [Tooltip("We are using these for populating levels icons dynamically")]
        [Header("Level Selection Page Elements")]
        [SerializeField] private GameObject levelsPageParent;

       
        
        public static LevelSelectionManager Instance { get; private set; }

        private void Awake() 
        { 
            if (Instance != null && Instance != this) { 
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
        }
        private void Start() {
            EventTriggers.SetupLevelPage();
            EventTriggers.onNextLevel += NextLevel;
            EventTriggers.LevelRestart += ResetLevel;
        }
        
        private void OnDisable() {
            Instance = null;
            EventTriggers.onNextLevel -= NextLevel;
            EventTriggers.LevelRestart -= ResetLevel;
        }

        
        /// <summary>
        /// Here we are resetting all requirements 
        /// </summary>
       
        
        /// <summary>
        /// When we select level from LevelSelection page
        /// Setting up level requirement
        /// Setting board
        /// Activating board
        /// Setting player score object
        /// </summary>
        /// <param name="levelNumber"></param>
        public void OnLevelSelect(int levelNumber)
        {
            GameManager.gameState = GameState.Started;
            
            CurrentLevel = levelNumber - 1;
             
            EventTriggers.OnUpdateMoveValue(); 
            EventTriggers.SetupLevelInfo();
            SetUpLevelRequirements();
            levelsPageParent.gameObject.SetActive(false);
            board.gameObject.SetActive(true);
            board.SetUp();

            if (!GameManager.playerScore.ContainsKey(CurrentLevel)) {
                GameManager.playerScore.Add(CurrentLevel, new LevelScore(CurrentLevel,0,new List<int>(),0));
            } 
            GameManager.playerScore[CurrentLevel].Reset();
        }

        public void ResetLevel()
        {
            RequirementsManager.ResetRequirements();
            board.SetUp();
        }
        public void NextLevel()
        {
            RequirementsManager.ClearRequirements();
            CurrentLevel = ++CurrentLevel <levelEditor.levels.Count? CurrentLevel : 0;
            OnLevelSelect(CurrentLevel+1);
        }

        public int GetTotalMovesCount()
        {
            return levelEditor.levels[CurrentLevel].targetMoves;
        }

        public void SetUpLevelRequirements()
        {
            var colorTarget = levelEditor.levels[CurrentLevel].colorTargetCount;
            RequirementsManager.ClearRequirements();
            for (var i = 0; i < colorTarget.Count; i++)
            {
                if (colorTarget[i] <= 0) continue;
                Requirement piece = null;
                if (RequirementsManager.Requirements.Count > i) {
                    piece = RequirementsManager.Requirements[i];
                }else{
                    piece = Instantiate(requirement, levelRequirementLocation.transform).GetComponent<Requirement>();
                    RequirementsManager.Requirements.Add(piece);
                }
                piece.gameObject.SetActive(true);
                piece.SetUpValues(pieceTypeDatabase.GetSpriteForPieceType(i), i, colorTarget[i]);
            }
            levelRequirementLocation.gameObject.SetActive(true);
        }
    }
}
