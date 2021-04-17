using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This is a class
/// </summary>
public class HoverUIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    string hoverText = "";
    bool isHovering = false;

    public string HoverText { set => hoverText = value; }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        HoverUI.Instance.Show(hoverText);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        HoverUI.Instance.Hide();
    }

    private void OnDisable()
    {
        if (isHovering)
        {
            isHovering = false;
            HoverUI.Instance.Hide();
        }
    }
}