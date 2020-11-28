using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIndicator : MonoBehaviour
{
    public SpriteRenderer Sprite;
    public SpriteRenderer WindupSprite;
    public Transform WindupMaskSlider;
    public float MaskSliderRestPoint;
    public float MaskSliderActivePoint;
    private int? _maskSliderTweenId;
    private int? _overtimeFadeTweenId;
    public float _windupTime;
    public delegate void AfterWindup();
    private AfterWindup _afterWindup;

    // Start is called before the first frame update
    void Start()
    {
        Show(false);
    }

    internal void ShowSprite()
    {
        Show();
    }

    public void Windup(float windupTime, AfterWindup afterWindup)
    {
        _windupTime = windupTime;
        _afterWindup = afterWindup;

        ResetWindup();

        if (_maskSliderTweenId.HasValue)
        {
            LeanTween.cancel(_maskSliderTweenId.Value);
        }
        _maskSliderTweenId = LeanTween.scaleZ(WindupMaskSlider.gameObject, MaskSliderActivePoint, _windupTime).id;
        LeanTween.descr(_maskSliderTweenId.Value).setEase(LeanTweenType.linear);
        LeanTween.descr(_maskSliderTweenId.Value).setOnComplete(() =>
        {
            _maskSliderTweenId = null;
            ResetWindup();
            Show(false, overtime: true);

            _afterWindup();
        });
    }

    private void ResetWindup()
    {
        WindupMaskSlider.localScale = new Vector3(WindupMaskSlider.localScale.x, WindupMaskSlider.localScale.y, MaskSliderRestPoint);
    }

    private void Show(bool show = true, bool overtime = false)
    {
        if (overtime == false)
        {
            Sprite.gameObject.SetActive(show);
        }
        else
        {
            if (_overtimeFadeTweenId.HasValue)
            {
                LeanTween.cancel(_overtimeFadeTweenId.Value);
            }
            _overtimeFadeTweenId = LeanTween.alpha(Sprite.gameObject, 0f, 0.5f).id;
            LeanTween.descr(_overtimeFadeTweenId.Value).setEase(LeanTweenType.linear);
            LeanTween.descr(_overtimeFadeTweenId.Value).setOnComplete(() =>
            {
                _overtimeFadeTweenId = null;
                Sprite.gameObject.SetActive(show);
            });
        }
        if (WindupSprite != null)
        {
            WindupSprite.gameObject.SetActive(show);
        }
    }
}
