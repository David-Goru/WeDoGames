using UnityEngine;
using UnityEngine.EventSystems;

public class HoverElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    string hoverText = "";
    bool isHovering = false;

    public string HoverText { set => hoverText = value; }

    public void OnPointerEnter(PointerEventData eventData)
    {
        startHovering();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        stopHovering();
    }

    public void SetHoverText(string text)
    {
        setText(text);
    }

    void setText(string text)
    {
        HoverText = text;
    }

    void OnDisable()
    {
        if (isHovering) stopHovering();
    }

    void startHovering()
    {
        isHovering = true;
        UI.ShowHoverUI(hoverText);
    }

    void stopHovering()
    {
        isHovering = false;
        UI.HideHoverUI();
    }
}