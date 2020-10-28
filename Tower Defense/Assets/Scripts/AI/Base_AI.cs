using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Base_AI : MonoBehaviour, ITurretDamage
{

    private NavMeshAgent agent;
    private Animator anim;
    private State currentState;

    public Transform Goal;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentState = new Move(this, anim, Goal, agent);
    }

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.Process();
    }

    public void OnTurretHit(Transform turretTransform, float damage, IEnemyDamage enemyDamage)
    {
        currentState.OnTurretHit(turretTransform, damage, enemyDamage);
    }
}
