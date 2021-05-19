using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyProjectile : Projectile
{
    [SerializeField] float radiusOfImpact = 2;
    Collider[] collidersCache = new Collider[64];
    LayerMask enemyLayer;

    Vector3 lastEnemyPosition;
    const float g = 9.8f;

    float vz;
    float vy;
    float y0;
    float lastZ;
    float lastY;
    float angle;
    float time;

    Quaternion lookAtTargetJustRotatingY;

    void Awake()
    {
        enemyLayer = LayerMask.GetMask("Enemy");
    }

    public override void SetInfo(Transform target, Transform turret,  TurretStats turretStats, IEnemyDamageHandler enemyDamageHandler)
    {
        base.SetInfo(target, turret, turretStats, enemyDamageHandler);
        lastEnemyPosition = target.position;
        initializeParameters();
    }

    void initializeParameters()
    {
        //reset parameters
        time = 0;
        lastZ = 0;
        lastY = transform.position.y;

        //Put the y at the same height of the target to get horizontal distance
        transform.position = new Vector3(transform.position.x, lastEnemyPosition.y, transform.position.z);

        //Calculate horizontal distance
        float z = Vector3.Distance(transform.position, lastEnemyPosition);

        //Reset y to his previous value
        transform.position = new Vector3(transform.position.x, lastY, transform.position.z);

        //Calculate vertical distance
        y0 = transform.position.y;
        float h = y0 - lastEnemyPosition.y;

        //Check if the speed is enough to reach target
        float velocity = speed;
        float sqrt = Mathf.Sqrt(h * h + z * z);
        while (float.IsNaN(Mathf.Acos(((g * z * z) / (velocity * velocity) - h) / sqrt)))
        {
            velocity++;
        }

        //calculate angle
        angle = (Mathf.Acos(((g * z * z) / (velocity * velocity) - h) / sqrt) + Mathf.Atan2(z, h)) / 2;

        //calculate velocities
        vz = velocity * Mathf.Cos(angle);
        vy = velocity * Mathf.Sin(angle);

        //Orientate projectile to target
        transform.LookAt(target);
        lookAtTargetJustRotatingY = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        transform.localRotation = lookAtTargetJustRotatingY;
    }

    protected override void updateProjectile()
    {
        time += Time.deltaTime;

        transform.localRotation = lookAtTargetJustRotatingY;
        float newZ = calculateHorizontalPosition();
        float newY = calculateVerticalPosition();
        calculateOrientation(newZ, newY);
        lastZ = newZ;
        lastY = newY;
    }

    float calculateHorizontalPosition()
    {
        float newZ = vz * time;
        transform.Translate(Vector3.forward * (newZ - lastZ), Space.Self);
        
        return newZ;
    }

    float calculateVerticalPosition()
    {
        float newY = y0 + (vy * time) - (g / 2) * (time * time);
        transform.Translate(Vector3.up * (newY - lastY), Space.World);
        
        return newY;
    }

    void calculateOrientation(float newZ, float newY)
    {
        Vector3 direction = new Vector3(0, newY, newZ) - new Vector3(0, lastY, lastZ);
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.localRotation = Quaternion.Euler(lookRotation.eulerAngles.x, lookAtTargetJustRotatingY.eulerAngles.y, lookRotation.eulerAngles.z);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Turret") && !other.CompareTag("Nexus"))
        {
            int nEnemies = Physics.OverlapSphereNonAlloc(transform.position, radiusOfImpact, collidersCache, enemyLayer);
            float damage = turretStats.SearchStatValue(StatType.DAMAGE);
            for (int i = 0; i < nEnemies; i++)
            {
                collidersCache[i].GetComponent<ITurretDamage>().OnTurretHit(turret, damage, enemyDamageHandler);
            }
            disable();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusOfImpact);
    }
}
