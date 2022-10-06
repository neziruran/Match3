using Core.Scripts.Tiles;
using UnityEngine;

namespace Core.Scripts.Managers
{
    public class LevelManager : MonoBehaviour
    {
        #region Variables
        public UIManager HUD;
        
        protected int CurrentScore;

        private bool _gameOver = false;
        #endregion

        #region Getters & Setters
        public static LevelManager Instance { get; private set; }
        
        public bool GetGameOver => _gameOver;

        #endregion

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            HUD.SetScore(CurrentScore);
        }
        
        protected void GameOver()
        {
            //Debug.Log("You Lose!");
            HUD.OnGameOver(CurrentScore);
            _gameOver = true;
        }

        public virtual void OnPlayerMove()
        {
            // EMPTY 
        }

        public virtual void OnPieceCleared(Tile tile)
        {
            CurrentScore += tile.score;

            HUD.SetScore(CurrentScore);
        }
    }
}
