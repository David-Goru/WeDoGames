using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

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
    //NavMeshAgent agent;
    Animator anim;
    State currentState;

    public Transform Goal;
    public Transform currentTurret;
    public IEnemyDamageHandler currentTurretDamage;
    public bool IsTargetTrigger;

    //A*

    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    private Vector3[] path;
    private int targetIndex;

    private Vector3 currentWaypoint;

    public void OnObjectSpawn()
    {
        Goal = GameObject.FindGameObjectWithTag("Nexus").transform;
        health = myHealth;
        //agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentState = new Move(this, anim, Goal);
        currentTurret = null;
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Game") return;
        Goal = GameObject.FindGameObjectWithTag("Nexus").transform;
        health = myHealth;
        //agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentState = new Move(this, anim, Goal);
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

    public void OnTurretHit(Transform turretTransform, float damage, IEnemyDamageHandler enemyDamage)
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
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            if (path.Length > 0)
            {
                StopCoroutine("FollowPath"); //This is for stopping the coroutine in case it's already running
                StartCoroutine("FollowPath");

            }
        }
    }

    IEnumerator FollowPath()
    {
        currentWaypoint = path[0];
        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length) //Path finished
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(currentWaypoint - transform.position), rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    //For visual hint
    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawCube(path[i], Vector3.one / 3);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
