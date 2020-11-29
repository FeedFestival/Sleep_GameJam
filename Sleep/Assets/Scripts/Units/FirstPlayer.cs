using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPlayer : MonoBehaviour, IPiece
{
    public Animator Animator;
    public AttackIndicator BasicAttack1;
    public float BasicAttack1WindupSpeed;
    // public float BasicAttack1;
    private AfterAttack _afterAttack;

    public void SetState(PieceState pieceState)
    {
        // switch (pieceState)
        // {
        //     case PieceState.Attack:
        //         Animator.SetBool(PieceState.Attack.ToString(), true);
        //         Animator.SetBool(PieceState.AttackWindup.ToString(), false);
        //         break;
        //     case PieceState.AttackWindup:
        //         Animator.SetBool(PieceState.AttackWindup.ToString(), true);
        //         Animator.SetBool(PieceState.IsMoving.ToString(), false);
        //         break;
        //     case PieceState.IsMoving:
        //         Animator.SetBool(PieceState.IsMoving.ToString(), true);
        //         break;
        //     case PieceState.Idle:
        //     default:
        //         Animator.SetBool(PieceState.IsMoving.ToString(), false);
        //         break;
        // }
    }

    public void Attack(AfterAttack afterAttack)
    {
        _afterAttack = afterAttack;

        SetState(PieceState.AttackWindup);

        // BasicAttack1.ShowSprite();
        // BasicAttack1.Windup(BasicAttack1WindupSpeed, ExecuteAttack);
    }

    public void ExecuteAttack()
    {
        // SetState(PieceState.Attack);

        // Timer._.iWait(() =>
        // {
        //     Animator.SetBool(PieceState.Attack.ToString(), false);
        //     _afterAttack();
        // }, 1f);
    }
}
