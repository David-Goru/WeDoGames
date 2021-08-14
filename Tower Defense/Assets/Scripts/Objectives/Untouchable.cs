using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Untouchable", menuName = "Objectives/Untouchable", order = 1)]
public class Untouchable : Objective
{
    public override void UpdateCompleteState()
    {
        if (Completed) return;
        Completed = Nexus.Instance.IsFullHealth;
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
        return "End the game with a full HP Nexus";
    }
}