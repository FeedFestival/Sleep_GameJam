using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveListener : MonoBehaviour
{
    public void OnRightClick()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
            Debug.Log("objectHit.gameObject.name: " + objectHit.gameObject.name);

            if (objectHit.gameObject.tag == "Floor")
            {
                Game._.Player.SetNewPosition(hit.point);
            }
        }

        // Debug.Log("targetPos: " + targetPos);
        // Game._.Player.Goal.position = targetPos;
    }
}
