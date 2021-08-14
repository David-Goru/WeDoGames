using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Economist", menuName = "Objectives/Economist", order = 1)]
public class Economist : Objective
{
    [SerializeField] int maxNumberOfTurrets = 0;

    public override void UpdateCompleteState()
    {
        if (Completed) return;
        Completed = Master.Instance.NumberOfTurrets <= maxNumberOfTurrets;
    }

    public override void SetDisplayText()
    {
        if (UIObject != null)
        {
            Transform uiText = UIObject.transform.Find("Text");
            if (uiText != null) uiText.GetComponent<Text>().text = getDisplayText();
        }
    }

    string getDisplayText()
    {
        return string.Format("End the game with maximum {0} turrets placed", maxNumberOfTurrets);
    }
}