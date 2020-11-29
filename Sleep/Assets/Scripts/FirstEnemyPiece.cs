using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstEnemyPiece : MonoBehaviour, IPiece
{
    public Animator Animator;
    public AttackIndicator BasicAttack1;
    public float BasicAttack1WindupSpeed;
    // public float BasicAttack1;
    private AfterAttack _afterAttack;
    public Transform CastBox;
    public Vector3 center;
    public Vector3 halfExtents;
    public Vector3 direction;
    public Vector3 eulerRot;

    private int _layerMask;

    void Start()
    {
        _layerMask = CreateLayerMask(aExclude: false, LayerMask.NameToLayer("Damageble"));
    }

    public void SetState(PieceState pieceState)
    {
        switch (pieceState)
        {
            case PieceState.Attack:
                Animator.SetBool(PieceState.Attack.ToString(), true);
                Animator.SetBool(PieceState.AttackWindup.ToString(), false);
                break;
            case PieceState.AttackWindup:
                Animator.SetBool(PieceState.AttackWindup.ToString(), true);
                Animator.SetBool(PieceState.IsMoving.ToString(), false);
                break;
            case PieceState.IsMoving:
                Animator.SetBool(PieceState.IsMoving.ToString(), true);
                break;
            case PieceState.Idle:
            default:
                Animator.SetBool(PieceState.IsMoving.ToString(), false);
                break;
        }
    }

    public void Attack(AfterAttack afterAttack)
    {
        _afterAttack = afterAttack;

        SetState(PieceState.AttackWindup);

        BasicAttack1.ShowSprite();
        BasicAttack1.Windup(BasicAttack1WindupSpeed, ExecuteAttack);
    }

    public void ExecuteAttack()
    {
        SetState(PieceState.Attack);

        CheckHit();

        Timer._.iWait(() =>
        {
            Animator.SetBool(PieceState.Attack.ToString(), false);
            _afterAttack();
        }, 1f);
    }

    public void CheckHit()
    {
        center = CastBox.position;
        halfExtents = CastBox.localScale;
        direction = CastBox.position - transform.position;
        Quaternion orientation = CastBox.rotation;
        eulerRot = orientation.eulerAngles;
        var hits = Physics.BoxCastAll(center, halfExtents, direction, orientation, 0f, _layerMask);
        foreach (var hit in hits)
        {
            Debug.Log("Hit : " + hit.collider.name);
            DamageTaker damageTaker = hit.collider.gameObject.GetComponent<DamageTaker>();
            if (damageTaker != null)
            {
                damageTaker.Stats.CurrentHealth = damageTaker.Stats.CurrentHealth - 15;
            }
        }
    }

    private int CreateLayerMask(bool aExclude, params int[] aLayers)
    {
        int v = 0;
        foreach (var L in aLayers)
            v |= 1 << L;
        if (aExclude)
            v = ~v;
        return v;
    }
}

public enum PieceState
{
    Idle, IsMoving, AttackWindup, Attack
}
