using UnityEngine;
using UnityEngine.UI;

public class EntityInfoUI : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] Entity currentEntity;
    [SerializeField] AudioClip clickEntitySound = null;

    void Start()
    {
        UI.Instance.EntityInfo = this;
        hideUI();
    }

    void Update()
    {
        if (!isVisible()) return;

        if (Input.GetMouseButton(0) && mouseIsOutOfEntity()) hideUI();
        else if (entityIsNotAvailable()) hideUI();
        else updateUI();
    }

    public void ShowUI(Entity entity)
    {
        if (Master.Instance.DoingAction()) return;
        if (currentEntity != null) currentEntity.CloseUI();

        Master.Instance.RunSound(clickEntitySound);
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
        if (currentEntity != null) currentEntity.ShowUI();
        changeVisibility(true);
    }

    public void HideUI()
    {
        hideUI();
    }

    void hideUI()
    {
        if (currentEntity != null) currentEntity.CloseUI();
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

    bool mouseIsOutOfEntity()
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