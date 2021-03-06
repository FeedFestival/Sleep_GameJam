﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabBank : MonoBehaviour
{
    private static PrefabBank _prefabBank;
    public static PrefabBank _ { get { return _prefabBank; } }

    public GameObject GamePrefab;
    public GameObject MusicManagerPrefab;
    public GameObject TimerPrefab;

    public GameObject Enemy;
    public GameObject EnemyGoal;
    public GameObject HealthBar;
    public GameObject Projectile;
    void Awake()
    {
        _prefabBank = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
