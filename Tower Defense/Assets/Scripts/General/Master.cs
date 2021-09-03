using UnityEngine;
using System.Collections.Generic;

public class Master : MonoBehaviour
{
    [Header("References")]
    [SerializeField] MasterInfo masterInfo = null;
    [SerializeField] Audio audioScript = null;

    public ActiveMode ActiveMode;
    public Grid grid;
    public MasterInfo MasterInfo { get => masterInfo; }
    [System.NonSerialized] public BuildObject BuildObject;

    public static Master Instance;
    public int WavesWithoutBuildingTurrets = 0;
    public bool NoActivesUsedInLastWave = false;
    public List<GameObject> ActiveTurrets;

    void Start()
    {
        if (Instance == null) Instance = this;
        if (audioScript == null) Debug.Log("Audio script not specified at Master script");

        ActiveTurrets = new List<GameObject>();
        MasterInfo.InitializeVariables();

        UI.AddChatCommand("addMoney", addMoney);
        UI.AddChatCommand("pause", pause);
        UI.AddChatCommand("resume", resume);
    }

    void Update()
    {
        UI.UpdateUI();
    }

    public float GetBalance() { return MasterInfo.Balance; }

    public bool CheckIfCanAfford(float amount) { return MasterInfo.Balance >= Mathf.Abs(amount); }

    public bool UpdateBalance(float amount)
    {
        if (amount < 0 && !CheckIfCanAfford(amount)) return false;

        MasterInfo.Balance += amount;
        UI.UpdateBalanceText(Mathf.RoundToInt(MasterInfo.Balance));

        return true;
    }

    public static void StartBuilding(TurretInfo buildingInfo)
    {
        if (Instance == null) return;

        if (Instance.BuildObject != null) Instance.BuildObject.StartBuilding(buildingInfo);
    }

    public static bool WaveCompletedInLessThan(float time)
    {
        if (Instance == null) return false;

        return Instance.GetComponent<Waves>().WaveCompletedInLessThan(time);
    }

    public bool DoingAction()
    {
        return BuildObject.enabled || ActiveMode.IsActive;
    }

    public void StopAllActions()
    {
        StopBuilding();
        StopActiveMode();
    }

    public void StopBuilding()
    {
        BuildObject.StopBuilding();
    }

    public void StopActiveMode()
    {
        ActiveMode.StopActiveMode();
    }
    
    public void RunSound(AudioClip clip)
    {
        if (audioScript == null) return;
        audioScript.RunSound(clip);
    }

    void addMoney(string[] parameters)
    {
        if (parameters.Length == 0) return;

        int money;
        int.TryParse(parameters[0], out money);

        UpdateBalance(money);
    }

    void pause(string[] parameters)
    {
        Time.timeScale = 0;
    }

    void resume(string[] parameters)
    {
        Time.timeScale = 1;
    }
}