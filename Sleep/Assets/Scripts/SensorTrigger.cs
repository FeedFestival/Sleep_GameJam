using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorTrigger : MonoBehaviour
{
    public PieceMover PieceMover;
    public SensorType SensorType;
    public SpriteRenderer Indicator;
    public bool PlayerInside;

    void OnTriggerEnter(Collider other)
    {
        if (SensorType == SensorType.NavGoal)
        {
            PieceGoal pieceGoal = other.gameObject.GetComponent<PieceGoal>();
            if (pieceGoal == null)
            {
                return;
            }
            // Debug.Log("pieceGoal.ParentId: " + pieceGoal.ParentId);
            // Debug.Log("PieceMover.Parent.Id: " + PieceMover.Parent.Id);
            if (PieceMover.Parent != null && (pieceGoal.ParentId == PieceMover.Parent.Id)
                || PieceMover.Parent == null && (pieceGoal.ParentId == PieceMover.Id))
            {
                PieceMover.DidWeReachDestionation();
            }
            return;
        }
        if (other.tag == "Player")
        {
            if (SensorType == SensorType.Visual)
            {
                PieceMover.FollowPlayer();
            }
            else if (SensorType == SensorType.AtackRange)
            {
                if (PieceMover.EmitCloseToTarget != null)
                {
                    PieceMover.EmitCloseToTarget(TargetType.AttackTarget);
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (SensorType == SensorType.Visual)
            {
                PieceMover.FollowPlayer(false);
            }
        }
    }

    public void ShowIndicator(bool show = true)
    {
        if (Indicator == null)
        {
            return;
        }
        Indicator.gameObject.SetActive(show);
    }
}

public enum SensorType
{
    Visual, Hearing, AtackRange, NavGoal
}