using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void AfterAttack();

public interface IPiece
{
    void SetState(PieceState pieceState);
    void Attack(AfterAttack afterAttack);
}
