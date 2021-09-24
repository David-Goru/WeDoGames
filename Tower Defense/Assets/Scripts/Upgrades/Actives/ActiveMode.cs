using UnityEngine;
using UnityEngine.EventSystems;

public class ActiveMode : MonoBehaviour
{
    bool isActive;
    ActiveAction activeAction;
    LayerMask groundMask;
    [SerializeField] ActivesCooldownController activesCooldownController = null;
    [SerializeField] AudioClip startActiveSound = null;
    GameObject activeArea = null;

    public bool IsActive { get => isActive; set => isActive = value; }

    private void Start()
    {
        groundMask = LayerMask.GetMask("Terrain");
    }

    public void ResetCooldowns(float time)
    {
        activesCooldownController.ResetCooldowns(time);
    }

    void Update()
    {
        if (isActive)
        {
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape)) StopActiveMode();
            else if (activeButtonPressed() && !EventSystem.current.IsPointerOverGameObject()) doActive();
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
            Master.Instance.StopAllActions();
            Master.Instance.RunSound(startActiveSound);
            isActive = true;
            this.activeAction = activeAction;
        }
    }

    public void StopActiveMode()
    {
        if (isActive == false) return;

        if (activeArea != null)
        {
            Destroy(activeArea);
            activeArea = null;
        }
        isActive = false;
    }

    bool activeButtonPressed()
    {
        return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
    }

    void doActive()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100, groundMask))
        {
            Master.Instance.NoActivesUsedInLastWave = false;
            activeAction.UseActive(hit.point);
            activesCooldownController.StartCooldown(activeAction);
            StopActiveMode();
        }
    }
}