using UnityEngine;

public class SpawnDeathParticles : TurretNoHealth
{
    [SerializeField] Pool deathVFX;
    [SerializeField] Transform particlesPos;
    ObjectPooler objectPooler;

    void Awake()
    {
        objectPooler = ObjectPooler.GetInstance();
    }

    public override void OnTurretNoHealth()
    {
        objectPooler.SpawnObject(deathVFX.tag, particlesPos.position);
    }
}