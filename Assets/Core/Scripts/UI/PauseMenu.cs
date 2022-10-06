using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core.Scripts.UI
{
    public class PauseMenu : MonoBehaviour
    {
        // Variables
        public GameObject pausePanel;
        public Button pauseButton;

        public static bool pauseIsClicked = false;

        private void Start()
        {
            pausePanel.SetActive(false);
        }

        // Stop time and display Pause Menu
        public void PauseGame()
        {
            pauseIsClicked = true;

            if (pauseIsClicked)
                pauseButton.enabled = false;

            pausePanel.SetActive(true);

            // Freeze time
            Time.timeScale = 0f;
        }

        // Do the opposite of PauseGame()
        public void ResumeGame()
        {
            pauseIsClicked = false;

            if (!pauseIsClicked)
                pauseButton.enabled = true;

            pausePanel.SetActive(false);

            // Unfreeze time
            Time.timeScale = 1f;
        }

        public void BackToMenu()
        {
            Time.timeScale = 1f;

            SceneManager.LoadScene("Main Menu");
        }
    }
}
