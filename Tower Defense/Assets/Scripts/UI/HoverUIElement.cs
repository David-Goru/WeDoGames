using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This is a class
/// </summary>
public class HoverUIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    string hoverText = "";

    public string HoverText { set => hoverText = value; }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverUI.Instance.Show(hoverText);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HoverUI.Instance.Hide();
    }
}