using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyProjectile : Projectile
{
    Vector3 lastEnemyPosition;
    const float g = 9.8f;

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
        transform.localRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    protected override void updateProjectile()
    {
        time += Time.deltaTime;

        calculateHorizontalPosition();
        calculateVerticalPosition();
    }

    void calculateHorizontalPosition()
    {
        float z = vz * time;
        transform.Translate(Vector3.forward * (z - lastZ), Space.Self);
        lastZ = z;
    }

    void calculateVerticalPosition()
    {
        float y = y0 + (vy * time) - (g / 2) * (time * time);
        transform.Translate(Vector3.up * (y - lastY), Space.World);
        lastY = y;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
