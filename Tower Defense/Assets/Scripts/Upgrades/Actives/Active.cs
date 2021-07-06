using UnityEngine;

[CreateAssetMenu(fileName = "Active", menuName = "Upgrades/Active", order = 0)]
public class Active : Upgrade
{
    [SerializeField] ActiveAction activeAction = null;

    public ActiveAction ActiveAction { get => activeAction; }
}