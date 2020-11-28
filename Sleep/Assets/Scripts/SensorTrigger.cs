using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorTrigger : MonoBehaviour
{
    public Enemy ParentEnemy;
    public SensorType SensorType;
    public SpriteRenderer Indicator;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (SensorType == SensorType.Visual)
            {
                ParentEnemy.FollowPlayer();
            }
            else if (SensorType == SensorType.AtackRange)
            {
                ParentEnemy.AttackPlayer();
            }
        }
    }

    public void ShowIndicator(bool show = true)
    {
        Indicator.gameObject.SetActive(show);
    }
}

public enum SensorType
{
    Visual, Hearing, AtackRange
}