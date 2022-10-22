using ToonBlast;
using UnityEngine;
using UnityEngine.UI;
public class LevelSelectionPage : MonoBehaviour
{
    [SerializeField] private GameObject[] levelIcons;
    [SerializeField] private Text[] levelTexts;
    [SerializeField] private Button[] levelButtons;
    private const int TotalPinsPerPage = 3;
    public int levelNumber;
    /// <summary>
    /// Setting up LevelSelection page
    /// After Menu page > we see Level Select Page
    /// </summary>
    /// <param name="pageNo"></param>
    /// <param name="pinCount"></param>
    public void SetUp(int pageNo, int pinCount)
    {
        for (var i = 0; i < pinCount; i++)
        {
            levelIcons[i].gameObject.SetActive(true);
            levelNumber = (TotalPinsPerPage * pageNo) + i + 1;
            levelTexts[i].text = levelNumber.ToString();
            var number = levelNumber;
            levelButtons[i].onClick.AddListener(() =>
            {
                LevelSelectionManager.Instance.OnLevelSelect(number);
            });
        }
    }
    
    
    
    

}
