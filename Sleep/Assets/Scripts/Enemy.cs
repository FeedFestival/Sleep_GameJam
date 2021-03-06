﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Id;
    public EnemyState EnemyState;
    public GameObject Piece;
    public Transform Avatar;
    private IPiece _piece;
    private Vector3? _dirToPlayer;
    public SensorTrigger VisualSensorTrigger;
    public SensorTrigger AttackRangeSensorTrigger;
    public PieceMover PieceMover;
    private float _queueTime;
    private float _iqTime;
    private IEnumerator _think;
    private int? _lookAtTwid;
    private Rigidbody _rb;
    private Stats _stats;
    public DamageTaker DamageTaker;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _piece = Piece.GetComponent<IPiece>();
        (_piece as FirstEnemyPiece).ParentId = Id;
        _stats = GetComponent<Stats>();
        _stats.Init();

        DamageTaker.ParentId = Id;

        VisualSensorTrigger.ShowIndicator(false);
        AttackRangeSensorTrigger.ShowIndicator(false);

        EnemyState = EnemyState.DoingStuff;

        PieceMover.Init(this, _piece, ReachedGoal, CloseToTarget);
        PieceMover.GoTo();

        _queueTime = Random.Range(0, 45) * 0.01f;
        // Debug.Log("_queueTime: " + _queueTime);
        Timer._.iWait(() =>
        {
            _iqTime = Random.Range(0, 25) * 0.01f;
            // Debug.Log("_iqTime: " + _iqTime);
            DoThink();
        }, _queueTime);
    }

    void DoThink()
    {
        if (_think != null)
        {
            StopCoroutine(_think);
        }
        _think = Think();
        StartCoroutine(_think);
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(_iqTime);

        Thinking();

        Avatar.LookAt(Camera.main.transform);

        DoThink();
    }

    private void Thinking()
    {
        if (PieceMover.IsMoving)
        {
            if (PieceMover.NavAgent.steeringTarget.x != PieceMover.SteeringTarget.x
                && PieceMover.NavAgent.steeringTarget.z != PieceMover.SteeringTarget.z)
            {
                PieceMover.SteeringTarget = PieceMover.NavAgent.steeringTarget;
                Look(GetLookAtTarget(PieceMover.SteeringTarget));
            }
        }

        if (EnemyState == EnemyState.Idle)
        {
            DoJob();
        }
        else if (EnemyState == EnemyState.FollowingPlayer)
        {
            if (PieceMover.IsMoving == false && PieceMover.FollowingPlayer == false)
            {
                DoJob();
            }
        }
        else if (EnemyState == EnemyState.DoingStuff)
        {
            if (PieceMover.IsMoving == false && PieceMover.FollowingPlayer == false)
            {
                DoJob();
            }
        }
    }

    private void DoJob()
    {
        EnemyState = EnemyState.DoingStuff;
        PieceMover.GoTo(Game._.Level<OnlyLevel>().GetRandomPoint());
    }

    public void CloseToTarget(TargetType targetType)
    {
        switch (targetType)
        {
            case TargetType.AttackTarget:
                AttackPlayer();
                break;
            default:
                break;
        }
    }

    private Vector3 GetLookAtTarget(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        return Quaternion.LookRotation(dir, Vector3.up).eulerAngles;
    }

    private void Look(Vector3 target)
    {
        if (_lookAtTwid.HasValue)
        {
            LeanTween.cancel(_lookAtTwid.Value);
            _lookAtTwid = null;
        }
        _lookAtTwid = LeanTween.rotateLocal(Piece.gameObject, target, 0.3f).id;
        LeanTween.descr(_lookAtTwid.Value).setEase(LeanTweenType.easeOutCirc);
    }

    public void AttackPlayer()
    {
        EnemyState = EnemyState.AttackingPlayer;
        PieceMover.Stop();

        Look(GetLookAtTarget(Game._.Player.transform.position));

        _piece.Attack(AfterAttack);

        _dirToPlayer = null;
    }

    private void AfterAttack()
    {
        bool isPlayerInAttackRange = IsPlayerInAttackRange();
        // Debug.Log("isPlayerInAttackRange: " + isPlayerInAttackRange);

        if (isPlayerInAttackRange)
        {
            AttackPlayer();
            return;
        }

        EnemyState = EnemyState.Idle;

        bool canWeSeePlayer = CanWeSeePlayer();
        if (canWeSeePlayer)
        {
            PieceMover.FollowPlayer();
            PieceMover.GoTo(Game._.Player.transform.position);
            return;
        }

        ReachedGoal();
    }

    private bool IsPlayerInAttackRange()
    {
        float distance = Vector2.Distance(Coord(), Game._.Player.Coord());
        // Debug.Log("distance: " + distance);
        return distance < 2.5f;
    }

    void LateUpdate()
    {
        if (PieceMover.FollowingPlayer == true)
        {
            bool canWeSeePlayer = CanWeSeePlayer();
            if (canWeSeePlayer)
            {
                PieceMover.GoTo(Game._.Player.transform.position);
            }
            else
            {
                _dirToPlayer = null;
            }
        }

        if (_dirToPlayer.HasValue)
        {
            Debug.DrawRay(transform.position, _dirToPlayer.Value, Color.cyan);
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
        // Debug.Log("ReachedGoal()");
        PieceMover.IsMoving = false;
        _piece.SetState(PieceState.Idle);

        if (_dirToPlayer.HasValue == false)
        {
            PieceMover.FollowPlayer(false);

        }
    }

    public Vector2 Coord()
    {
        return new Vector2(
            transform.position.x,
            transform.position.z
        );
    }
}

public enum TargetType
{
    AttackTarget
}

public enum EnemyState
{
    Idle, DoingStuff, FollowingPlayer, AttackingPlayer
}
