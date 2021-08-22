using UnityEngine;

public class SBTriggerAnimation : MonoBehaviour, ITurretShotBehaviour
{
    [SerializeField] Animator anim = null;
    [SerializeField] string parameter = "";

    public void OnShot()
    {
        anim.SetTrigger(parameter);
    }
}