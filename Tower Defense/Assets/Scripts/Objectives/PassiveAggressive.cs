using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Passive-Aggressive", menuName = "Objectives/PassiveAggressive", order = 1)]
public class PassiveAggressive : Objective
{
    public override void UpdateCompleteState()
    {
        if (Completed) return;
        Completed = Master.Instance.NoActivesUsedInLastWave;
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
        return "Complete the wave without using any active";
    }
}