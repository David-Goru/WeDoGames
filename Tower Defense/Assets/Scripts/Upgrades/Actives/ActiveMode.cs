using UnityEngine;

public class ActiveMode : MonoBehaviour
{
    bool isActive;
    ActiveAction activeAction;
    LayerMask groundMask;

    private void Start()
    {
        groundMask = LayerMask.GetMask("Ground");
    }

    void Update()
    {
        if (isActive)
        {
            if (Input.GetMouseButtonDown(0)) doActive();
        }
    }

    public void SetActive(ActiveAction activeAction)
    {
        isActive = true;
        this.activeAction = activeAction;
    }

    public void doActive()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100, groundMask))
        {
            activeAction.UseActive(hit.point);
            isActive = false;
        }
    }
}