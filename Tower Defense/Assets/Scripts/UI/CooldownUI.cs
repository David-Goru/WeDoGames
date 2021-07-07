using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour
{
    [SerializeField] Color enabledColor = new Color();
    [SerializeField] Color disabledColor = new Color();
    [SerializeField] Image UIimage = null;
    [SerializeField] Text cooldownText = null;

    public void StartCooldown()
    {
        UIimage.color = disabledColor;
    }

    public void EndCooldown()
    {
        hideCooldownText();
        UIimage.color = enabledColor;
    }

    public void SetCooldownText(int remainingTime)
    {
        cooldownText.text = string.Format("{0}", remainingTime);
    }

    void hideCooldownText()
    {
        cooldownText.text = "";
    }
}