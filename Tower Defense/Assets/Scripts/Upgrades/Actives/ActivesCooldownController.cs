using System.Collections.Generic;
using UnityEngine;

public class ActivesCooldownController : MonoBehaviour
{
    List<ActiveAction> activesInCoolDown = new List<ActiveAction>();
    List<float> timers = new List<float>();
    
    void Update()
    {
        updateCooldowns();
    }

    public void StartCooldown(ActiveAction active)
    {
        if (!CheckIfIsInCooldown(active))
        {
            activesInCoolDown.Add(active);
            timers.Add(0f);
            active.cooldownUI.StartCooldown();
            active.cooldownUI.SetCooldownText(Mathf.CeilToInt(active.activeCooldown));
        }
    }

    void updateCooldowns()
    {
        for (int i = activesInCoolDown.Count - 1; i >= 0; i--)
        {
            timers[i] += Time.deltaTime;
            if (timers[i] >= activesInCoolDown[i].activeCooldown)
            {
                activesInCoolDown[i].cooldownUI.EndCooldown();
                timers.RemoveAt(i);
                activesInCoolDown.RemoveAt(i);
            }
            else activesInCoolDown[i].cooldownUI.SetCooldownText(Mathf.CeilToInt(activesInCoolDown[i].activeCooldown - timers[i]));
        }
    }

    public bool CheckIfIsInCooldown(ActiveAction active)
    {
        return activesInCoolDown.Contains(active);
    }
}