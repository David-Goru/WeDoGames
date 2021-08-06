using UnityEngine;

/// <summary>
/// This is a class
/// </summary>
public class FairyProjectile : MonoBehaviour, IPooledObject
{
    Transform target;
    float speed = 5f;
    int damage;

    IEnemyDamageHandler enemyDamageHandler;

    public void OnObjectSpawn()
    {
        
    }

    public void SetInfo(Transform _target, int _damage)
    {
        target = _target;
        damage = _damage;
    }

    void Update()
    {
        if (!target.gameObject.activeSelf) disable();
        chaseEnemy();
    }

    void chaseEnemy()
    {
        transform.LookAt(target);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == target)
        {
            if (target.CompareTag("Turret"))
            {
                enemyDamageHandler = collision.collider.GetComponentInChildren<IEnemyDamageHandler>();
                if (enemyDamageHandler != null) enemyDamageHandler.OnEnemyHit(damage);
            }
            else if (target.CompareTag("Nexus")) Nexus.Instance.GetHit(damage);

            disable();
        }
    }

    void disable()
    {
        ObjectPooler.GetInstance().ReturnToThePool(this.transform);
    }
}