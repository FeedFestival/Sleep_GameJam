using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.LevelService;
using UnityEngine;
using UnityEngine.AI;

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
            Vector3 startPos = GetRandomPoint();
            GameObject go = CreateFromPrefab(PrefabBank._.Enemy, startPos);
            Enemy enemy = go.GetComponent<Enemy>();
            enemy.Id = _uniqueIdCount;

            startPos = GetRandomPoint();
            go = CreateFromPrefab(PrefabBank._.EnemyGoal, startPos);
            go.transform.SetParent(NavT);
            enemy.PieceMover.Goal = go.GetComponent<PieceGoal>();
            enemy.PieceMover.Goal.ParentId = enemy.Id;
            enemy.PieceMover.Goal.GoalIndicator.gameObject.SetActive(false);

            Enemies.Add(enemy);

            _uniqueIdCount++;
        }
    }

    public Vector3 GetRandomPoint()
    {
        float x = Random.Range(-49.0f, 49.0f);
        float z = Random.Range(-49.0f, 49.0f);
        Vector3 randomPoint = new Vector3(x, 0, z);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 5.0f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else if (NavMesh.FindClosestEdge(randomPoint, out hit, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            Debug.Log("Can't find a place for you");
            return Vector3.zero;
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
