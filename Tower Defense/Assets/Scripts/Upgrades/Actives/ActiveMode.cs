using System;
using UnityEngine;

public class ActiveMode : MonoBehaviour
{
    bool isActive;
    ActiveAction activeAction;
    LayerMask groundMask;
    [SerializeField] ActivesCooldownController activesCooldownController = null;
    [SerializeField] GameObject activeAreaPrefab = null;
    GameObject activeArea = null;

    private void Start()
    {
        groundMask = LayerMask.GetMask("Ground");
        activeArea = Instantiate(activeAreaPrefab, Vector3.zero, Quaternion.identity);
    }

    void Update()
    {
        if (isActive)
        {
            if (Input.GetMouseButtonDown(0)) doActive();
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100f, groundMask))
                {
                    if(!activeArea.activeSelf)
                        activeArea.SetActive(true);
                    activeArea.transform.position = hit.point;
                }
                else
                {
                    activeArea.SetActive(false);
                }
            }
        }
    }

    public void SetActive(ActiveAction activeAction)
    {
        if (!activesCooldownController.CheckIfIsInCooldown(activeAction))
        {
            isActive = true;
            this.activeAction = activeAction;
            setActiveArea();
        }
    }

    void setActiveArea()
    {
        activeArea.SetActive(true);
        activeArea.transform.localScale = activeAction.activeRange * Vector3.one;
        activeArea.GetComponent<MeshRenderer>().material.SetColor("_Color", activeAction.activeAreaColor);
    }

    void doActive()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100, groundMask))
        {
            activeAction.UseActive(hit.point);
            isActive = false;
            activesCooldownController.StartCooldown(activeAction);
            activeArea.SetActive(false);
        }
    }
}