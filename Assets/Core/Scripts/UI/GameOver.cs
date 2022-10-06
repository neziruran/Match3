using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core.Scripts.UI
{
    public class GameOver : MonoBehaviour
    {
        #region Variables

        private const int MainMenuIndex = 0;
        
        public GameObject screenParent;
        public GameObject replayButton;

        public AnimationClip gameOverAnimation;
        public Text scoreText;

        public float waitToSpawn = 1;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            screenParent.SetActive(false);
        }

        #region Win/Lose Screen
        public IEnumerator ShowGameOver(int score)
        {
            yield return new WaitForSeconds(waitToSpawn);

            screenParent.SetActive(true);

            scoreText.text = score.ToString();

            replayButton.SetActive(true);

            Animator animator = GetComponent<Animator>();

            if (animator)
            {
                animator.Play(gameOverAnimation.name);
            }
        }
        #endregion

        #region UI Button Events
        public void OnReplayClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        public void BackToMenu()
        {
            SceneManager.LoadScene(MainMenuIndex);
        }
        #endregion
    }
}
