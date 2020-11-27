using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public Transform Goal;
    private NavMeshAgent _agent;
    public Vector3 CamFollowOffset;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
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
        _agent.destination = pos;
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
    }
}
