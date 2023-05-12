using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Match3
{
    public class UIManager : MonoBehaviour
    {
        
        [Header("UI Element for showing moves count")]
        [SerializeField] private Text targetMovesText;
        
        [Header("Menu Elements")]
        [Tooltip("Used For Menu Entry Animation")]
        [SerializeField] private GameObject title;
        [Tooltip("Used For Menu Entry Animation")]
        [SerializeField] private GameObject startButton;
        [Header("UI Elements for Game Win")]
        [SerializeField] private GameObject levelWinPage;
        [Header("UI Elements for Game Fail")]
        [SerializeField] private GameObject levelFailPage;

        
        [Tooltip("We are using these for populating levels icons dynamically")]
        [Header("Level Selection Page Elements")]
        [SerializeField] private GameObject levelSelectionPage;
        [SerializeField] private GameObject levelsPageLocation;
         

         
        
        
        [Header("UI element target for updating Current level value")]
        [SerializeField] private Text levelInfoText;
        
        private void Awake()
        {
            EventTriggers.PlayMenuAnimation += MenuAnimation;
            EventTriggers.PlayGameWin += GameWinAnimation;
            EventTriggers.PlayGameLoss += GameFailAnimation;
            EventTriggers.UpdateMoves += UpdateCurrentMoveValueInUI;
            EventTriggers.onSetupLevelPage += SetUpLevelsPage;
            EventTriggers.UpdateLevelInfo += UpdateLevelInfoUI;
        }

        private void OnDisable()
        {
            EventTriggers.PlayMenuAnimation -= MenuAnimation;
            EventTriggers.PlayGameWin -= GameWinAnimation;
            EventTriggers.PlayGameLoss -= GameFailAnimation;
            EventTriggers.UpdateMoves -= UpdateCurrentMoveValueInUI;
            EventTriggers.onSetupLevelPage -= SetUpLevelsPage;
            EventTriggers.UpdateLevelInfo -= UpdateLevelInfoUI;
        }
        /// <summary>
        /// Basic animation setup for Menu Page
        /// moving object y value for animation
        /// </summary>
        private void MenuAnimation()
        {
            var pos = title.transform.localPosition;
            pos.y += 500;
            title.transform.localPosition = pos;
            pos.y -= 500;
            Tween.Move(title.transform, pos,1.8f, 0.5f, this);

            pos = startButton.transform.localPosition;
            pos.y -= 200;
            startButton.transform.localPosition = pos;
            pos.y += 200;
            Tween.Move(startButton.transform, pos,1.8f, 0.5f, this);
        }
        
        /// <summary>
        /// Once game state is fail
        /// Activating the GameFail page and moving it down using tween animation
        /// </summary>
        private void GameFailAnimation() {
             
            var currentLocation = levelFailPage.transform.position;
            currentLocation.y += 10;
            levelFailPage.transform.GetChild(0).position = currentLocation;
            Tween.Move(levelFailPage.transform.GetChild(0).transform, Vector3.zero, 1f , 1.3f, this );
            levelFailPage.SetActive(true);
            
            var animImageColor = levelFailPage.GetComponent<Image>();
            animImageColor.color = Color.clear;
            Tween.FadeImage(animImageColor,new Color(0,0,0,0.75f),0.5f,1f, this);
             
        }
        /// <summary>
        /// Once game state win
        /// Activating the GameWin page and moving it to down using tween animation
        /// </summary>
        private void GameWinAnimation() {
           
            var currentLocation = levelWinPage.transform.GetChild(0).position;
            currentLocation.y += 10;
            levelWinPage.transform.GetChild(0).position = currentLocation;
            Tween.Move(levelWinPage.transform.GetChild(0).transform, Vector3.zero, 1f , 1.3f, this );
            levelWinPage.SetActive(true);
            
            var animImageColor = levelWinPage.GetComponent<Image>();
            animImageColor.color = Color.clear;
            Tween.FadeImage(animImageColor,new Color(0,0,0,0.75f),0.5f,1f,this);
           
        }
        
        
        private void UpdateCurrentMoveValueInUI()
        {
            targetMovesText.text = (GameManager.currentMoves + "/"+ LevelSelectionManager.Instance.GetTotalMovesCount());
        }

        private void UpdateLevelInfoUI()
        {
            levelInfoText.text = "Level " + (LevelSelectionManager.CurrentLevel + 1);        
        }

        /// <summary>
        /// using level editor scriptable object
        /// setting up levels selection page
        /// this is used in menu
        /// </summary>
        private void SetUpLevelsPage() 
        {
            var totalPagesCount = (LevelSelectionManager.Instance.levelEditor.levels.Count / 3);
            var lastPageActivatedPins = LevelSelectionManager.Instance.levelEditor.levels.Count % 3;
            for (var i = 0; i < totalPagesCount; i++)
            {
                var item = Instantiate(levelSelectionPage, levelsPageLocation.transform).GetComponent<LevelSelectionPage>();
                item.SetUp(i, 3);
            }

            if (lastPageActivatedPins == 0) return;
            {
                var item = Instantiate(levelSelectionPage, levelsPageLocation.transform).GetComponent<LevelSelectionPage>();
                item.SetUp(totalPagesCount, lastPageActivatedPins);
            }
        }
        
    }
}