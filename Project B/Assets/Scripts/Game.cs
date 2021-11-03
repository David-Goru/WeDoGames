using System;
using UnityEngine;

public class Game : MonoBehaviour
{
    private void Start()
    {
        IGameInitializer gameInitializer = GetComponentInChildren<IGameInitializer>();
        gameInitializer?.Initialize();
    }
}