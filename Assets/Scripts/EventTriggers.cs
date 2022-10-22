using UnityEngine;

namespace ToonBlast
{
    public class EventTriggers : MonoBehaviour
    {
        // Event triggers
        public delegate void OnMenuAnimation();
        public static event OnMenuAnimation PlayMenuAnimation;
        
        public delegate void OnGameWin();
        public static event OnGameWin PlayGameWin;
        
        public delegate void OnGameLoss();
        public static event OnGameLoss PlayGameLoss;
        
        public delegate void OnUpdateMoves();
        public static event OnUpdateMoves UpdateMoves;
        
        public delegate void OnUpdateLevelInfo();
        public static event OnUpdateLevelInfo UpdateLevelInfo;
        
        public delegate void OnGamePause();
        public static event OnGamePause GamePause;
        
        public delegate void OnLevelRestart();
        public static event OnLevelRestart LevelRestart;
        
        public delegate void OnHome();
        public static event OnHome Home;

        public delegate void OnNextLevel();
        public static event OnNextLevel onNextLevel;
        
        public delegate void OnSetupLevelPage();
        public static event OnSetupLevelPage onSetupLevelPage;
        

        public static void OnUpdateMoveValue()
        {
            UpdateMoves();
        }
        public static void MenuAnimation()
        {
            PlayMenuAnimation();
        }
        public static void GameWin()
        {
            PlayGameWin();
        } 
        public static void GameLoss()
        {
            PlayGameLoss();
        }

        public static void GamePaused()
        {
            GamePause();
        }
        
        public static void GameRestart()
        {
            LevelRestart();
        }
        
        public static void GoHome()
        {
            Home();
        }

        public static void NextLevel()
        {
            onNextLevel();
        }

        public static void SetupLevelPage()
        {
            onSetupLevelPage();
        }

        public static void SetupLevelInfo()
        {
            UpdateLevelInfo();
        }
    }
}