using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorTrigger : MonoBehaviour
{
    public PieceMover PieceMover;
    public SensorType SensorType;
    public SpriteRenderer Indicator;

    void OnTriggerEnter(Collider other)
    {
        if (SensorType == SensorType.NavGoal)
        {
            PieceGoal pieceGoal = other.gameObject.GetComponent<PieceGoal>();
            if (pieceGoal == null)
            {
                return;
            }
            if (pieceGoal.ParentId == PieceMover.ParentId)
            {
                PieceMover.EmitReachedGoal();
            }
            return;
        }
        if (other.tag == "Player")
        {
            if (SensorType == SensorType.Visual)
            {
                PieceMover.FollowingPlayer = true;
            }
            else if (SensorType == SensorType.AtackRange)
            {
                PieceMover.EmitCloseToTarget(TargetType.AttackTarget);
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