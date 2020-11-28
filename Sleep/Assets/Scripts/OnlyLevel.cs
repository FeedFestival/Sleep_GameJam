using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.LevelService;
using UnityEngine;

public class OnlyLevel : MonoBehaviour, ILevel
{
    public Transform NavT;
    public int EnemiesCount;
    public List<Enemy> Enemies;

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

            x = Random.Range(-49.0f, 49.0f);
            z = Random.Range(-49.0f, 49.0f);
            startPos = new Vector3(x, 0, z);
            go = CreateFromPrefab(PrefabBank._.EnemyGoal, startPos);
            go.transform.SetParent(NavT);
            enemy.Goal = go.GetComponent<PieceGoal>();
            enemy.Goal.GoalIndicator.gameObject.SetActive(false);

            Enemies.Add(enemy);
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
