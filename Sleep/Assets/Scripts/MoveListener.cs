using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveListener : MonoBehaviour
{
    private int _layerMask;

    void Start()
    {
        _layerMask = CreateLayerMask(aExclude: false, LayerMask.NameToLayer("Navigation"));
    }

    public void OnRightClick(BaseEventData eventData)
    {
        PointerEventData pointerData = (eventData as PointerEventData);

        if (pointerData.button == PointerEventData.InputButton.Right)
        {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
            {
                Transform objectHit = hit.transform;
                // Debug.Log("objectHit.gameObject.name: " + objectHit.gameObject.name);

                if (objectHit.gameObject.tag == "Floor")
                {
                    Game._.Player.PieceMover.GoTo(hit.point);
                }
            }
        }
        else
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
            {
                Transform objectHit = hit.transform;
                // Debug.Log("objectHit.gameObject.name: " + objectHit.gameObject.name);

                if (objectHit.gameObject.tag == "Floor")
                {
                    Game._.Player.ShootProjectile(hit.point);
                }
            }
        }

        // Debug.Log("targetPos: " + targetPos);
        // Game._.Player.Goal.position = targetPos;
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
