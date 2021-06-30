using UnityEngine;
using UnityEngine.UI;

public class GeneralInfoUI : MonoBehaviour
{
    [SerializeField] Text balanceText = null;
    [SerializeField] Text energyText = null;
    [SerializeField] Text waveText = null;

    void Start()
    {
        UI.Instance.GeneralInfoUI = this;
    }

    public void UpdateBalanceText(int balance)
    {
        balanceText.text = string.Format("{0}", balance);
    }

    public void UpdateEnergyText(int energy)
    {
        energyText.text = string.Format("{0}", energy);
    }

    public void UpdateWaveText(int wave)
    {
        waveText.text = string.Format("WAVE {0}", wave);
    }
}