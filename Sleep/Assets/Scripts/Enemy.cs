using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public PieceGoal Goal;
    public GameObject Piece;
    private IPiece _piece;
    private NavMeshAgent _agent;
    private bool _followPlayer;
    public bool IsMoving;
    private Vector3? _dirToPlayer;
    public SensorTrigger VisualSensorTrigger;
    public SensorTrigger AttackRangeSensorTrigger;
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _piece = Piece.GetComponent<IPiece>();

        VisualSensorTrigger.ShowIndicator(false);
        AttackRangeSensorTrigger.ShowIndicator(false);

        SetNewPosition();
    }

    internal void SetNewPosition(Vector3? point = null)
    {
        if (point.HasValue)
        {
            Goal.transform.position = point.Value;
        }
        MoveTo(Goal.transform.position);
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
        _followPlayer = false;
        ReachedGoal();
        _agent.isStopped = true;

        // do attack
        Vector3 dir = Game._.Player.transform.position - transform.position;
        Vector3 rot = Quaternion.LookRotation(dir, Vector3.up).eulerAngles;
        // Debug.Log("rot: " + rot);
        // Debug.Log("Piece.transform.eulerAngles: " + Piece.transform.eulerAngles);
        Piece.transform.eulerAngles = rot;

        _piece.Attack(AfterAttack);

        _dirToPlayer = null;
    }

    private void AfterAttack()
    {
        Debug.Log("After Attack");
        _agent.isStopped = false;
        bool canWeSeePlayer = CanWeSeePlayer();
        Debug.Log("canWeSeePlayer: " + canWeSeePlayer);
        if (canWeSeePlayer)
        {
            _followPlayer = true;
            SetNewPosition(Game._.Player.transform.position);
        }
        else
        {
            ReachedGoal();
        }
    }

    void LateUpdate()
    {
        if (_followPlayer == true)
        {
            bool canWeSeePlayer = CanWeSeePlayer();
            // Debug.Log("canWeSeePlayer: " + canWeSeePlayer);
            if (canWeSeePlayer)
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
