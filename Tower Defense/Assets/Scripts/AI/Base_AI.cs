using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Base_AI : MonoBehaviour, ITurretDamage, IPooledObject
{
    [SerializeField] float myHealth = 0f;
    [SerializeField] float damage = 0f;
    public float Damage
    {
        get
        {
            return damage;
        }
    }
    [SerializeField] float attackSpeed = 0f;
    public float AttackSpeed
    {
        get
        {
            return attackSpeed;
        }
    }

    float health;
    NavMeshAgent agent;
    Animator anim;
    State currentState;

    public Transform Goal;
    public Transform currentTurret;
    public IEnemyDamage currentTurretDamage;
    public bool IsTargetTrigger;

    public void OnObjectSpawn()
    {
        Goal = GameObject.FindGameObjectWithTag("Nexus").transform;
        health = myHealth;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentState = new Move(this, anim, Goal, agent);
        currentTurret = null;
    }

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.Process();

        if (currentTurret != null && !currentTurret.gameObject.activeSelf)
        {
            currentTurret = null;
        }
    }

    void checkDeath()
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

    void OnTriggerEnter(Collider other)
    {
        if (currentTurret == other.transform || currentTurret == null && other.transform == Goal) IsTargetTrigger = true;

        /*if (other.CompareTag("Nexus"))
        {
            //Debug.Log("NEXUS TRIGGER");
            IsTargetTrigger = true;
        }
        else if (other.CompareTag("Turret"))
        {
            //Debug.Log("TURRET TRIGGER");
            if(currentTurret != null)
            {
                IsTargetTrigger = true;
            }
        }*/
    }
}
