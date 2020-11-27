using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform Goal;
    private NavMeshAgent _agent;
    private bool _followPlayer;
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetNewPosition();
    }

    internal void SetNewPosition(Vector3? point = null)
    {
        if (point.HasValue)
        {
            Goal.position = point.Value;
        }
        MoveTo(Goal.position);
    }

    internal void MoveTo(Vector3 pos)
    {
        _agent.destination = pos;
    }

    public void FollowPlayer()
    {
        _followPlayer = true;
    }

    public void AttackPlayer()
    {
        Debug.Log("Atack Playerul");
        UIController._.DialogController.ShowDialog(true, GameplayState.Failed);
    }

    void LateUpdate()
    {
        if (_followPlayer == true)
        {
            SetNewPosition(Game._.Player.transform.position);
        }
    }
}
