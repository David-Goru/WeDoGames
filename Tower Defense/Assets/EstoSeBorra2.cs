using UnityEngine;

public class EstoSeBorra2 : MonoBehaviour
{
    bool isRotating = false;
    Quaternion lookRotation;
    float speed = 16f;
    bool firstTime = true;
    Animator anim;
    [SerializeField] bool jump;
    [SerializeField] bool mirror;

    private void Awake()
    {
        anim = GetComponentInParent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!firstTime)
            {
                speed *= 40f;
                if (jump)
                {
                    anim.SetBool("isJumping", true);
                    if (mirror)
                        anim.SetBool("Mirror", true);
                }
            }
            firstTime = false;
            isRotating = true;
            lookRotation = Quaternion.LookRotation(-transform.forward);
        }
        if (isRotating)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, speed * Time.deltaTime);
        }
    }
}
