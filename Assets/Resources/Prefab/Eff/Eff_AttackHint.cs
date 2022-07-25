using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Eff_AttackHint : CommonEffectsBase
{
    public override void Action(params object[] objs)
    {
        
    }

    public override void OnInit()
    {
        gameObject.SetActive(true);
    }

    public override void OnSetInit(params object[] value)
    {
        var startPos = (Vector3)value[0];
        var endPos = (Vector3)value[1];
        var time = (float)value[2];

        Play();
        transform.position = startPos;
        transform.DOMove(endPos, time, false).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                Stop();
                Destroy();
            });

    }
}
