using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    [SerializeField] Color fullHealthColor;
    [SerializeField] Color noHealthColor;
    [SerializeField] Image sliderImage;

    void OnEnable()
    {
        sliderImage.fillAmount = 1f;
        sliderImage.color = fullHealthColor;
        transform.rotation = Quaternion.Euler(90f, 0f, -PlayerCamera.currentRotation + 90f);
    }

    public void SetFillAmount(float value)
    {
        sliderImage.fillAmount = value;
        sliderImage.color = Color.Lerp(noHealthColor, fullHealthColor, value);
    }
}