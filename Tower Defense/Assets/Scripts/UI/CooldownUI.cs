using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour
{
    [SerializeField] Text cooldownText = null;
    [SerializeField] Image cooldownBG = null;

    public void StartCooldown()
    {
        cooldownBG.gameObject.SetActive(true);
    }

    public void EndCooldown()
    {
        hideCooldownText();
        cooldownBG.gameObject.SetActive(false);
        cooldownBG.fillAmount = 1;
    }

    public void UpdateCooldown(float requiredTime, float currentTime = 0.0f)
    {
        cooldownText.text = string.Format("{0}", Mathf.CeilToInt(requiredTime - currentTime));
        cooldownBG.fillAmount = 1.0f - currentTime / requiredTime;
    }

    void hideCooldownText()
    {
        cooldownText.text = "";
    }
}