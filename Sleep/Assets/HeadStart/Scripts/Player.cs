using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public int Id;
    public GameObject Piece;
    private IPiece _piece;
    public Vector3 CamFollowOffset;
    public PieceMover PieceMover;
    private int? _camCenterTwid;
    private float _iqTime;
    private IEnumerator _checkState;
    private Vector3 _camPos;
    private Stats _stats;

    void Awake()
    {
        _piece = Piece.GetComponent<IPiece>();
        _stats = GetComponent<Stats>();

        PieceMover.Init(null, _piece, ReachedGoal);
    }

    void Start()
    {
        _stats.Init();
        _iqTime = UnityEngine.Random.Range(0, 25) * 0.01f;

        _camPos = new Vector3(transform.position.x + CamFollowOffset.x,
                transform.position.y + CamFollowOffset.y, transform.position.z + CamFollowOffset.z);
        KeepCamCentered();

        DoCheckState();
    }

    void DoCheckState()
    {
        if (_checkState != null)
        {
            StopCoroutine(_checkState);
        }
        _checkState = CheckState();
        StartCoroutine(_checkState);
    }

    IEnumerator CheckState()
    {
        yield return new WaitForSeconds(_iqTime);

        if (PieceMover.IsMoving)
        {
            Vector3 newCamPos = new Vector3(transform.position.x + CamFollowOffset.x,
                transform.position.y + CamFollowOffset.y, transform.position.z + CamFollowOffset.z);
            if (newCamPos.x != _camPos.x || newCamPos.z != _camPos.z)
            {
                _camPos = newCamPos;
                KeepCamCentered();
            }
        }

        DoCheckState();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (h != 0 || v != 0)
        {
            Vector3 movement = new Vector3(h, 0f, v);
            PieceMover.GoTo(transform.position + movement);
        }
    }

    void KeepCamCentered()
    {
        if (_camCenterTwid.HasValue)
        {
            LeanTween.cancel(_camCenterTwid.Value);
            _camCenterTwid = null;
        }

        _camCenterTwid = LeanTween.move(Camera.main.gameObject, _camPos, 0.3f).id;
        LeanTween.descr(_camCenterTwid.Value).setEase(LeanTweenType.linear);
    }

    void LateUpdate()
    {
        if (PieceMover.IsMoving)
        {

        }
    }

    private void ReachedGoal()
    {
        PieceMover.IsMoving = false;
        _piece.SetState(PieceState.Idle);
    }

    public Vector2 Coord()
    {
        return new Vector2(
            transform.position.x,
            transform.position.z
        );
    }
}
