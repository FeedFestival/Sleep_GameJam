using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int ParentId;
    public GameObject ParentGo;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("other.gameObject.name: " + other.gameObject.name);

        var damageTaker = other.gameObject.GetComponent<DamageTaker>();
        if (ParentId != damageTaker.ParentId)
        {
            damageTaker.Stats.CurrentHealth = damageTaker.Stats.CurrentHealth - 20;
            Destroy(ParentGo);
        }
    }
}
