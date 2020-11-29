using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public int Id;
    public Transform Goal;
    private NavMeshAgent NavAgent;
    public Vector3 CamFollowOffset;
    public bool IsMoving;

    void Awake()
    {
        NavAgent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // SetNewPosition();
    }

    internal void SetNewPosition(Vector3? point = null)
    {
        if (point.HasValue)
        {
            Goal.position = point.Value;
        }
        MoveTo(Goal.position);
    }

    internal void MoveTo(Vector3 pos)
    {
        IsMoving = true;
        NavAgent.destination = pos;
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (h != 0 || v != 0)
        {
            Vector3 movement = new Vector3(h, 0f, v);
            SetNewPosition(transform.position + movement);
        }
    }

    void LateUpdate()
    {
        if (IsMoving)
        {
            Vector3 camPos = new Vector3(
                        transform.position.x + CamFollowOffset.x,
                        transform.position.y + CamFollowOffset.y,
                        transform.position.z + CamFollowOffset.z
                        );
            // Debug.Log(camPos);
            Camera.main.transform.position = new Vector3(
                camPos.x,
                Camera.main.transform.position.y,
                camPos.z
            );

            DidWeReachDestionation();
        }
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
                    IsMoving = false;
                }
            }
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
