using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform Goal;
    private NavMeshAgent _agent;
    private bool _followPlayer;
    public bool IsMoving;
    private Vector3? _dirToPlayer;
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
        IsMoving = true;
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

    // void OnDrawGizmosSelected()
    // {
    //     // Draws a 5 unit long red line in front of the object
    //     Gizmos.color = Color.red;
    //     Vector3 direction = transform.TransformDirection(Vector3.forward) * 5;
    //     Gizmos.DrawRay(transform.position, direction);
    // }

    void LateUpdate()
    {
        if (_followPlayer == true)
        {
            if (CanWeSeePlayer())
            {
                SetNewPosition(Game._.Player.transform.position);
            }
            else
            {
                _dirToPlayer = null;
            }
        }

        if (IsMoving)
        {
            DidWeReachDestionation();
        }

        if (_dirToPlayer.HasValue)
        {
            Debug.DrawRay(transform.position, _dirToPlayer.Value, Color.cyan);
        }
    }

    private void DidWeReachDestionation()
    {
        // Check if we've reached the destination
        if (!_agent.pathPending)
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                {
                    ReachedGoal();
                }
            }
        }
    }

    private bool CanWeSeePlayer()
    {
        RaycastHit hit;
        _dirToPlayer = Game._.Player.transform.position - transform.position;
        Ray ray = new Ray(transform.position, _dirToPlayer.Value);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }

    private void ReachedGoal()
    {
        IsMoving = false;
        if (_dirToPlayer.HasValue == false)
        {
            _followPlayer = false;

            float x = Random.Range(-49.0f, 49.0f);
            float z = Random.Range(-49.0f, 49.0f);
            Vector3 startPos = new Vector3(x, 0, z);
            SetNewPosition(startPos);
        }
    }
}
