using System.Collections.Generic;
using UnityEngine;

public class ActivesCooldownController : MonoBehaviour
{
    List<ActiveAction> activeList = new List<ActiveAction>();
    List<float> timers = new List<float>();
    
    private void Update()
    {
        updateCooldowns();
    }

    public void StartCooldown(ActiveAction active)
    {
        if (!CheckIfIsInCooldown(active))
        {
            activeList.Add(active);
            timers.Add(0f);
        }
    }

    private void updateCooldowns()
    {
        for (int i = activeList.Count - 1; i >= 0; i--)
        {
            timers[i] += Time.deltaTime;
            if(timers[i] >= activeList[i].activeCooldown)
            {
                timers.RemoveAt(i);
                activeList.RemoveAt(i);
            }
        }
    }

    public bool CheckIfIsInCooldown(ActiveAction active)
    {
        return activeList.Contains(active);
    }
}