using System.Collections;
using Match3.Model;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
public class Requirement : MonoBehaviour
{
    public Text requirementText;
    public Image pieceImage;
    public Image tickMarkImage;
    public int pieceColorNumber;
    public int targetCount;

    private int _requirementCount;
    private int _currentRequirementCount;


    public bool IsCompleted => _currentRequirementCount == targetCount;
    public bool IsCompletedForUI => _requirementCount  == targetCount;
    public void SetUpValues(Sprite sprite,int piece, int count) {
        pieceColorNumber = piece;
        targetCount = count;
        pieceImage.sprite = sprite;
        requirementText.text = targetCount.ToString();
    }

    public void PieceCollected(float waitTime) {
        if (_currentRequirementCount == targetCount) {
            return;
        }
        _currentRequirementCount++;
        StartCoroutine(UpdateText(waitTime));
    }

    private IEnumerator UpdateText(float waitTime = 0) {
        yield return new WaitForSeconds(waitTime);
        if (_requirementCount >= targetCount) yield break;
        _requirementCount++;
        requirementText.text = (targetCount - _requirementCount).ToString();
        if (_requirementCount >= targetCount) {
            requirementText.enabled = false;
            tickMarkImage.enabled = true;
        }
    }
    public void ResetCurrentProgress() {
        _requirementCount = 0;
        _currentRequirementCount = 0;
        requirementText.text = targetCount.ToString();
        requirementText.enabled = true;
        tickMarkImage.enabled = false;
    }


}
