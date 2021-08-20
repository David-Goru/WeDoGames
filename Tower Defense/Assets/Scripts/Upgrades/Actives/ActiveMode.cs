﻿using System;
using UnityEngine;

public class ActiveMode : MonoBehaviour
{
    bool isActive;
    ActiveAction activeAction;
    LayerMask groundMask;
    [SerializeField] ActivesCooldownController activesCooldownController = null;
    [SerializeField] GameObject activeAreaPrefab = null;
    [SerializeField] AudioClip startActiveSound = null;
    GameObject activeArea = null;

    private void Start()
    {
        groundMask = LayerMask.GetMask("Ground");
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
                    if (activeArea == null)
                    {
                        activeArea = Instantiate(activeAction.ActiveAreaGO, Vector3.zero, Quaternion.identity);
                        activeArea.transform.localScale = activeAction.activeRange * 2 * Vector3.one;
                    }

                    activeArea.transform.position = hit.point;
                }
                else
                {
                    Destroy(activeArea);
                    activeArea = null;
                }
            }
        }
    }

    public void SetActive(ActiveAction activeAction)
    {
        if (!activesCooldownController.CheckIfIsInCooldown(activeAction))
        {
            Master.Instance.RunSound(startActiveSound);
            isActive = true;
            this.activeAction = activeAction;
        }
    }

    void doActive()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100, groundMask))
        {
            Master.Instance.NoActivesUsedInLastWave = false;
            activeAction.UseActive(hit.point);
            isActive = false;
            activesCooldownController.StartCooldown(activeAction);
            activeArea.SetActive(false);
        }
    }
}