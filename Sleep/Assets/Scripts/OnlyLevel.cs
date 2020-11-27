using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.LevelService;
using UnityEngine;

public class OnlyLevel : MonoBehaviour, ILevel
{
    public void StartLevel()
    {
        Debug.Log("Started level..");
    }
}
