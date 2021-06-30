using UnityEngine;
using UnityEngine.UI;

public class HoverUI : MonoBehaviour
{
    Text hoverText;
    RectTransform hoverRect;

    void Start()
    {
        hoverText = transform.Find("Text").GetComponent<Text>();
        hoverRect = GetComponent<RectTransform>();

        UI.Instance.HoverUI = this;
        hideUI();
    }

    private void Update()
    {
        if (isVisible()) updatePosition();
    }

    public void ShowUI(string text)
    {
        setText(text);
        updatePosition();
        changeVisibility(true);
    }

    public void HideUI()
    {
        hideUI();
    }

    void updatePosition()
    {
        hoverRect.transform.position = Input.mousePosition + new Vector3(125, 0, 0);
    }

    void hideUI()
    {
        changeVisibility(false);
    }

    void setText(string newText)
    {
        hoverText.text = newText;
    }

    void changeVisibility(bool visible)
    {
        gameObject.SetActive(visible);
    }

    bool isVisible()
    {
        return gameObject.activeSelf;
    }
}