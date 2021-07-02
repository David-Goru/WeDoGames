using UnityEngine;
using UnityEngine.UI;

public class GeneralInfoUI : MonoBehaviour
{
    [SerializeField] Text balanceText = null;
    [SerializeField] Text nexusLifeText = null;
    [SerializeField] Text waveText = null;
    [SerializeField] Text waveTimerText = null;

    void Start()
    {
        UI.Instance.GeneralInfoUI = this;
    }

    public void UpdateBalanceText(int balance)
    {
        balanceText.text = string.Format("{0}", balance);
    }

    public void UpdateNexusLifeText(int nexusLife)
    {
        nexusLifeText.text = string.Format("{0} HP", nexusLife);
    }

    public void UpdateWaveText(int wave)
    {
        waveText.text = string.Format("WAVE {0}", wave);
    }

    public void UpdateWaveTimerText(int secondsRemaining)
    {
        waveTimerText.text = string.Format("{0}", secondsRemaining);
    }
}