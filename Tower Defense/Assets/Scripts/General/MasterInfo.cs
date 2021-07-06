using UnityEngine;

[CreateAssetMenu(fileName = "MasterInfo", menuName = "ScriptableObjects/MasterInfo", order = 0)]
public class MasterInfo : ScriptableObject
{
    [SerializeField] float initialBalance = 0.0f;
    [SerializeField] TurretInfo[] turretsSet = null;
    [SerializeField] TurretInfo[] initialTurretsSet = null;
    [SerializeField] Upgrade[] upgradesSet = null;
    [SerializeField] Pool[] enemiesSet = null;

    float currentBalance = 0.0f;

    public float Balance { get => currentBalance; set => currentBalance = value; }
    public Upgrade[] UpgradesSet { get => upgradesSet; }

    public TurretInfo[] GetTurretsSet() { return turretsSet; }
    public TurretInfo[] GetInitialTurretsSet() { return initialTurretsSet; }
    public Pool[] GetEnemiesSet() { return enemiesSet; }

    void resetBalance() { currentBalance = initialBalance; }

    public void InitializeVariables()
    {
        resetBalance();
    }
}