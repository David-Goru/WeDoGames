using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyProjectile : Projectile
{
    Vector3 lastEnemyPosition;
    const float g = 9.8f;

    [SerializeField] float timeToReachTarget;
    float vz;
    float vy;
    float y0;
    float lastZ;
    float lastY;
    float angle;
    float time;

    public override void SetInfo(Transform target, float damage, TurretBehaviour turret)
    {
        base.SetInfo(target, damage, turret);
        lastEnemyPosition = target.position;
        initializeParameters();
    }

    void initializeParameters()
    {
        time = 0;
        lastZ = 0;
        lastY = transform.position.y;
        transform.position = new Vector3(transform.position.x, lastEnemyPosition.y, transform.position.z);
        float z = Vector3.Distance(transform.position, lastEnemyPosition);
        transform.position = new Vector3(transform.position.x, lastY, transform.position.z);
        y0 = transform.position.y;
        float h = y0 - lastEnemyPosition.y;
        float velocity = speed;
        while (float.IsNaN(Mathf.Acos(((g * z * z) / (velocity * velocity) - h) / Mathf.Sqrt(h * h + z * z))))
        {
            velocity++;
        }
        angle = (Mathf.Acos(((g * z * z) / (velocity * velocity) - h) / Mathf.Sqrt(h * h + z * z)) + Mathf.Atan2(z, h)) / 2;
        vz = velocity * Mathf.Cos(angle);
        vy = velocity * Mathf.Sin(angle);
        transform.LookAt(target);
        transform.localRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    protected override void updateProjectile()
    {
        time += Time.deltaTime;
        float z = vz * time;
        transform.Translate(Vector3.forward * (z - lastZ), Space.Self);
        lastZ = z;
        float y = y0 + (vy * time) - (g / 2) * (time * time);
        transform.Translate(Vector3.up * (y - lastY), Space.World);
        lastY = y;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
