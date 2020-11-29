﻿using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.LevelService;
using UnityEngine;

public class OnlyLevel : MonoBehaviour, ILevel
{
    public Transform NavT;
    public int EnemiesCount;
    public List<Enemy> Enemies;
    private int _uniqueIdCount;

    public void StartLevel()
    {
        Debug.Log("Started level..");

        for (var i = 0; i < EnemiesCount; i++)
        {
            float x = Random.Range(-49.0f, 49.0f);
            float z = Random.Range(-49.0f, 49.0f);
            Vector3 startPos = new Vector3(x, 0, z);
            GameObject go = CreateFromPrefab(PrefabBank._.Enemy, startPos);
            Enemy enemy = go.GetComponent<Enemy>();
            enemy.Id = _uniqueIdCount;

            x = Random.Range(-49.0f, 49.0f);
            z = Random.Range(-49.0f, 49.0f);
            startPos = new Vector3(x, 0, z);
            go = CreateFromPrefab(PrefabBank._.EnemyGoal, startPos);
            go.transform.SetParent(NavT);
            enemy.PieceMover.Goal = go.GetComponent<PieceGoal>();
            enemy.PieceMover.Goal.ParentId = enemy.Id;
            enemy.PieceMover.Goal.GoalIndicator.gameObject.SetActive(false);

            Enemies.Add(enemy);

            _uniqueIdCount++;
        }
    }

    public GameObject CreateFromPrefab(GameObject targetPrefab, Vector3? originPosition = null)
    {
        if (originPosition == null)
        {
            originPosition = new Vector3(0, 0, 0);
        }
        return GameObject.Instantiate(targetPrefab, originPosition.Value, Quaternion.identity) as GameObject;
    }

}
