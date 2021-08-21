using UnityEngine;

public class ASObjectActivation : EffectComponent, ITurretAttackState
{
    [SerializeField] GameObject[] objs = null;
    [SerializeField] bool initialState = false;

    public override void InitializeComponent()
    {
        setObjectActivation(initialState);
    }

    public override void UpdateComponent()
    {
    }

    void setObjectActivation(bool active)
    {
        foreach (GameObject obj in objs)
        {
            obj.SetActive(active);
        }
    }

    public void OnAttackEnter()
    {
        setObjectActivation(!initialState);
    }

    public void OnAttackExit()
    {
        setObjectActivation(initialState);
    }
}
