using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstEnemyPiece : MonoBehaviour, IPiece
{
    public AttackIndicator BasicAttack1;
    private AfterAttack _afterAttack;

    public void Attack(AfterAttack afterAttack)
    {
        _afterAttack = afterAttack;

        BasicAttack1.ShowSprite();

        BasicAttack1.Windup(1f, ExecuteAttack);
    }

    public void ExecuteAttack()
    {
        Debug.Log("Execute Attack");

        _afterAttack();
    }
}
