using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public int ParentId;
    private int? _moveTwid;
    public DamageDealer DamageDealer;

    public void GoTowards(Vector3 dir, int parentId)
    {
        ParentId = parentId;
        DamageDealer.ParentId = ParentId;

        if (_moveTwid.HasValue)
        {
            LeanTween.cancel(_moveTwid.Value);
            _moveTwid = null;
        }

        var newPos = transform.position + (dir * 50f);

        _moveTwid = LeanTween.move(gameObject, newPos, 3f).id;
        LeanTween.descr(_moveTwid.Value).setEase(LeanTweenType.linear);
        LeanTween.descr(_moveTwid.Value).setOnComplete(() =>
        {
            LeanTween.cancel(_moveTwid.Value);
            _moveTwid = null;
            Destroy(gameObject);
        });
    }
}
