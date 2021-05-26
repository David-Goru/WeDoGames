using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesActive : MonoBehaviour
{
    public List<Base_AI> enemiesList;

    private static EnemiesActive instance;

    public static EnemiesActive Instance
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
        enemiesList = new List<Base_AI>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Base_AI enemy in enemiesList)
        {
            enemy.EnemyUpdate();
        }
    }
}
