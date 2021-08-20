using UnityEngine;

public class ASObjectActivation : MonoBehaviour, ITurretAttackState
{
    [SerializeField] GameObject obj = null;
    [SerializeField] bool initialState = false;

    void OnEnable()
    {
        obj.SetActive(initialState);
    }

    public void OnAttackEnter()
    {
        obj.SetActive(!initialState);
    }

    public void OnAttackExit()
    {
        obj.SetActive(initialState);
    }
}
