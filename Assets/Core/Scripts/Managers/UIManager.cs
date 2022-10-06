using Core.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        #region Variables
        public GameOver gameOver;

        public Text remainingText;
        public Text remainingSubText;
        public Text targetText;
        public Text targetSubText;
        public Text scoreText;
        public Text scoreSubText;
        #endregion

        #region Set HUD
        public void SetScore(int score)
        {
            scoreText.text = score.ToString();
            scoreSubText.text = "Score: ";
        }

        public void SetTarget(int target)
        {
            targetText.text = target.ToString();
        }

        public void SetRemaining(int remaining)
        {
            remainingText.text = remaining.ToString();
        }

        public void SetText()
        {
            remainingSubText.text = "Moves Remaining:";
            targetSubText.text = "Target Score:";
        }
        #endregion
        
        public void OnGameOver(int score)
        {
            gameOver.StartCoroutine(gameOver.ShowGameOver(score));
        }
    }
}
