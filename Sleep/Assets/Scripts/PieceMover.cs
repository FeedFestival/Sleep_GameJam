using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PieceMover : MonoBehaviour
{
    public int ParentId;
    public bool IsMoving;
    public bool FollowingPlayer;
    public PieceGoal Goal;
    public delegate void ReachedGoal();
    public delegate void CloseToTarget(TargetType targetType);
    public NavMeshAgent NavAgent;
    private IPiece _piece;
    public ReachedGoal EmitReachedGoal;
    public CloseToTarget EmitCloseToTarget;
    public Vector3 SteeringTarget;

    public void Init(int parentId, IPiece piece, ReachedGoal reachedGoal = null, CloseToTarget closeToTarget = null)
    {
        ParentId = parentId;
        _piece = piece;
        EmitReachedGoal = reachedGoal;
        EmitCloseToTarget = closeToTarget;
        NavAgent = GetComponent<NavMeshAgent>();
    }

    public void GoTo(Vector3? point = null)
    {
        if (point.HasValue)
        {
            Goal.transform.position = point.Value;
        }
        MoveTo(Goal.transform.position);
    }

    public void Stop()
    {
        FollowingPlayer = false;
        IsMoving = false;
        NavAgent.isStopped = true;
    }

    internal void MoveTo(Vector3 pos)
    {
        IsMoving = true;
        if (NavAgent.isStopped)
        {
            NavAgent.isStopped = false;
        }
        _piece.SetState(PieceState.IsMoving);
        bool canMove = NavAgent.SetDestination(pos);
        if (canMove == false) {
            Debug.Log("Can't move there");
        }
        // NavAgent.destination = pos;
    }

    public void DidWeReachDestionation()
    {
        NavAgent.isStopped = true;
        if (EmitReachedGoal != null)
        {
            EmitReachedGoal();
        }
    }
}
