using UnityEngine;
using UnityEngine.UI;

public class EntityInfoUI : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] Entity currentEntity;

    void Start()
    {
        UI.Instance.EntityInfo = this;
        hideUI();
    }

    void Update()
    {
        if (!isVisible()) return;

        if (Input.GetMouseButton(0) && mouseIsOutOfUI()) hideUI();
        else if (entityIsNotAvailable()) hideUI();
        else updateUI();
    }

    public void ShowUI(Entity entity)
    {
        int yPos = hasUpperScreenSpaceAvailable(240) ? 120 : -120;
        transform.position = Input.mousePosition + new Vector3(0, yPos, 0);
        currentEntity = entity;
        showUI();
    }

    void updateUI()
    {
        transform.Find("Title").GetComponent<Text>().text = currentEntity.Title;
        transform.Find("HP").GetComponent<Text>().text = string.Format("{0}/{1}", currentEntity.CurrentHP, currentEntity.MaxHP);
    }

    void showUI()
    {
        updateUI();
        changeVisibility(true);
    }

    void hideUI()
    {
        changeVisibility(false);
    }

    void changeVisibility(bool visible)
    {
        gameObject.SetActive(visible);
    }

    bool isVisible()
    {
        return gameObject.activeSelf;
    }

    bool mouseIsOutOfUI()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        return !Physics.Raycast(ray, out hit) || hit.transform != currentEntity.transform;
    }

    bool entityIsNotAvailable()
    {
        return currentEntity.IsDead;
    }

    bool hasUpperScreenSpaceAvailable(int pixelsRequired)
    {
        return (Input.mousePosition.y + pixelsRequired) < Screen.height;
    }
}