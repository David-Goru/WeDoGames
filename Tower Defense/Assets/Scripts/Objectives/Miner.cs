using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Miner", menuName = "Objectives/Miner", order = 1)]
public class Miner : Objective
{
    [SerializeField] TurretInfo extractorInfo = null;
    [SerializeField] int numberOfExtractors = 0;

    public override void UpdateCompleteState()
    {
        if (Completed) return;
        Completed = extractorInfo.NumberOfTurretsPlaced >= numberOfExtractors;
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
        return string.Format("End the game with at least {0} extractors", numberOfExtractors);
    }
}