using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "AFK", menuName = "Objectives/AFK", order = 1)]
public class AFK : Objective
{
    [SerializeField] int wavesWithoutBuilding = 0;

    public override void UpdateCompleteState()
    {
        if (Completed) return;
        Completed = Master.Instance.WavesWithoutBuildingTurrets >= wavesWithoutBuilding;
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
        return string.Format("Don't place any turret for {0} waves", wavesWithoutBuilding);
    }
}