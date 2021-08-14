using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Limited Resources", menuName = "Objectives/LimitedResources", order = 1)]
public class LimitedResources : Objective
{
    [SerializeField] List<TurretInfo> turretsTypes = null;

    public override void UpdateCompleteState()
    {
        if (Completed) return;

        Completed = true;

        foreach (TurretInfo turret in Master.Instance.MasterInfo.GetTurretsSet())
        {
            if (turret.NumberOfTurretsPlaced > 0 && !turretsTypes.Contains(turret)) Completed = false;
        }
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
        string turrets = "";
        foreach (TurretInfo turret in turretsTypes) turrets += turret.name + "/";
        turrets = turrets.Remove(turrets.Length - 1);
        return string.Format("End the game with only those turrets: {0}", turrets);
    }
}