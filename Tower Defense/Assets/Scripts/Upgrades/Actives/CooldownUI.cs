using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is a class
/// </summary>
public class CooldownUI : MonoBehaviour
{
    [SerializeField] Color enabledColor = new Color();
    [SerializeField] Color disabledColor = new Color();
    [SerializeField] Image UIimage = null;

    public void startCooldown()
    {
        UIimage.color = disabledColor;
    }

    public void endCooldown()
    {
        UIimage.color = enabledColor;
    }
}