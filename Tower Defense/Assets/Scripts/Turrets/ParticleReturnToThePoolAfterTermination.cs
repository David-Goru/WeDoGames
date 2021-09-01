using UnityEngine;

public class ParticleReturnToThePoolAfterTermination : MonoBehaviour, IPooledObject
{
    ParticleSystem particles;
    ObjectPooler objectPooler;
    float timer = 0f;

    public void OnObjectSpawn()
    {
        timer = 0f;
    }

    void Awake()
    {
        particles = GetComponent<ParticleSystem>();
        objectPooler = ObjectPooler.GetInstance();
    }

    void Update()
    {
        print(particles.main.duration);
        if (timer >= particles.main.duration) objectPooler.ReturnToThePool(transform);
        else timer += Time.deltaTime;
    }
}