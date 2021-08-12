using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Faster Than Light", menuName = "Objectives/FasterThanLight", order = 1)]
public class FasterThanLight : Objective
{
    [SerializeField] float maximumTime = 0.0f;

    public override bool HasBeenCompleted()
    {
        return Master.WaveCompletedInLessThan(maximumTime);
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
        return string.Format("Complete the wave in less than {0} seconds", maximumTime);
    }
}