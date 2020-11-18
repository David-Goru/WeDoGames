using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Base_AI : MonoBehaviour, IPooledObject, ITurretDamage
{

    [SerializeField] private float health = 0f;
    [SerializeField] private float damage = 0f;
    public float Damage
    {
        get
        {
            return damage;
        }
    }
    [SerializeField] private float attackSpeed = 0f;
    public float AttackSpeed
    {
        get
        {
            return attackSpeed;
        }
    }

    private NavMeshAgent agent;
    private Animator anim;
    private State currentState;

    public Transform Goal;
    public Transform currentTurret;
    public IEnemyDamage currentTurretDamage;
    public bool IsTargetTrigger;

    public void OnObjectSpawn()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentState = new Move(this, anim, Goal, agent);
    }

    /*
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentState = new Move(this, anim, Goal, agent);
    }
    */

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.Process();

        if (currentTurret != null && !currentTurret.gameObject.activeSelf)
        {
            currentTurret = null;
        }
    }

    private void checkDeath()
    {
        if(health <= 0)
        {
            ObjectPooler.GetInstance().ReturnToThePool(this.transform);
        }
    }

    public void OnTurretHit(Transform turretTransform, float damage, IEnemyDamage enemyDamage)
    {
        health -= damage;
        currentTurretDamage = enemyDamage;
        if (currentTurret == null)
        {
            currentTurret = turretTransform;
            currentState.OnTurretHit(turretTransform, damage, enemyDamage);
        }
        checkDeath();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Nexus"))
        {
            Debug.Log("NEXUS TRIGGER");
            IsTargetTrigger = true;
        }
        else if (other.CompareTag("Turret"))
        {
            Debug.Log("TURRET TRIGGER");
            if(currentTurret != null)
            {
                IsTargetTrigger = true;
            }
        }
    }
}
