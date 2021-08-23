using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveEnemies : MonoBehaviour
{
    public List<BaseAI> enemiesList;

    private static ActiveEnemies instance;

    public static ActiveEnemies Instance
    {
        get
        {
            return instance;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) instance = this;
        enemiesList = new List<BaseAI>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(BaseAI enemy in enemiesList)
        {
            enemy.EnemyUpdate();
        }
    }
}
