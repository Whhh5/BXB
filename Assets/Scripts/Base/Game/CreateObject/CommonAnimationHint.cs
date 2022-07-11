using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using BXB.Core;
using System;

public abstract class CommonAnimationHint : MiObjPoolPublicParameter, ICommonAnimationHint
{
    [SerializeField] protected GameObject main;
    [SerializeField] protected Animation anima;
    [SerializeField] protected List<GameObject> hideObj;
    protected override void OnAwake()
    {
        base.OnAwake();
        foreach (var para in hideObj)
        {
            para.SetActive(false);
        }
    }
    public void Show(Action endEvent)
    {
        gameObject.SetActive(true);
        var animaName = $"{gameObject.name.Split(new string[] { "(Clone)" }, StringSplitOptions.RemoveEmptyEntries)[0]}_Show";
        var clip = anima.GetClip(animaName);
        if (clip != null)
        {
            anima.Play(animaName);
            DOTween.To(() => 2, value => { }, 0, clip.length).OnComplete(() =>
            {
                endEvent.Invoke();
            });
        }
    }
    public override void Destroy()
    {
        base.Destroy();
    }

    public GameObject GetMain()
    {
        return main;
    }

    public abstract void OnInit();

    public abstract void OnSetInit(object[] value);

    public object Clone()
    {
        return MemberwiseClone();
    }

    public abstract void Active(params object[] value);

    public virtual void Hide(Action startEvent)
    {
        var animaName = $"{gameObject.name.Split(new string[] { "(Clone)" }, StringSplitOptions.RemoveEmptyEntries)[0]}_Hide";
        var click = anima.GetClip(animaName);
        float hideDelaty = 0;
        if (click != null)
        {
            anima.Play(animaName);
            hideDelaty = click.length;
        }
        DOTween.To(() => 2, value => { }, 0, hideDelaty)
            .OnStart(()=>
            {
                startEvent.Invoke();
            })
            .OnComplete(() =>
            {
                Destroy();
            });
    }
}
