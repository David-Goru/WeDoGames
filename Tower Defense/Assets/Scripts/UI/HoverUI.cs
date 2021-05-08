using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is a class
/// </summary>
public class HoverUI : MonoBehaviour
{
    [Tooltip("Object where the text will be displayed")]
    public GameObject HoverPanel;

    Text hoverText;
    RectTransform hoverRect;

    public static HoverUI Instance;

    void Start()
    {
        Instance = this;

        if (HoverPanel == null) Debug.Log("HoverUI text not defined.");
        else
        {
            hoverText = HoverPanel.transform.Find("Text").GetComponent<Text>();
            hoverRect = HoverPanel.GetComponent<RectTransform>();
        }
    }

    private void Update()
    {
        if (HoverPanel.activeSelf) UpdatePosition();
    }

    public void Show(string text)
    {
        if (HoverPanel == null) return;

        hoverText.text = text;
        hoverRect.transform.position = Input.mousePosition + new Vector3(0, -25, 0);
        HoverPanel.SetActive(true);
    }

    public void UpdatePosition()
    {
        hoverRect.transform.position = Input.mousePosition + new Vector3(125, 0, 0);
    }

    public void Hide()
    {
        if (HoverPanel == null) return;

        HoverPanel.SetActive(false);
    }
}