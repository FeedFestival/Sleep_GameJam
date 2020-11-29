using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public IAm IAm;
    public int Health;
    private int currentHealth;
    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;

            if (healthBar)
                if (currentHealth < 0)
                {
                    healthBar.gameObject.SetActive(false);
                    UIController._.DialogController.ShowDialog(true, GameplayState.Failed);
                }
                else
                {
                    healthBar.CalculateHealthBar();
                }
        }
    }
    public Transform HealthBarTarget;
    [HideInInspector]
    public HealthBar healthBar;

    public void Init()
    {
        CurrentHealth = Health;
        healthBar = CreateHPBar();
    }

    public HealthBar CreateHPBar()
    {
        GameObject go = Instantiate(
            PrefabBank._.HealthBar,
            Vector3.zero, Quaternion.identity, UIController._.HealthBarsPanel.transform
            ) as GameObject;
        var rect = go.GetComponent<RectTransform>();
        rect.localPosition = Vector3.zero;
        rect.localEulerAngles = Vector3.zero;

        var hp = go.GetComponent<HealthBar>();
        var hpColor = (IAm == IAm.Ally) ? HiddenSettings._.PlayerHpBars : HiddenSettings._.EnemyHpBars;
        hp.HealthImage.color = hpColor;
        hp.Init(this, HealthBarTarget, UIController._.Canvas);
        return hp;
    }
}

public enum IAm
{
    Enemy,
    Ally
}
