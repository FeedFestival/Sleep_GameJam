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

    public void Init(int parentId, IPiece piece, ReachedGoal reachedGoal, CloseToTarget closeToTarget)
    {
        ParentId = parentId;
        _piece = piece;
        EmitReachedGoal = reachedGoal;
        EmitCloseToTarget = closeToTarget;
        NavAgent = GetComponent<NavMeshAgent>();
    }

    void LateUpdate()
    {
        
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
            NavAgent.isStopped = true;
        }
        _piece.SetState(PieceState.IsMoving);
        NavAgent.destination = pos;
    }

    private void DidWeReachDestionation()
    {
        // Check if we've reached the destination
        if (!NavAgent.pathPending)
        {
            if (NavAgent.remainingDistance <= NavAgent.stoppingDistance)
            {
                if (!NavAgent.hasPath || NavAgent.velocity.sqrMagnitude == 0f)
                {
                    EmitReachedGoal();
                }
            }
        }
    }
}
