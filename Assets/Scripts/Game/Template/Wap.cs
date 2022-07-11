using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using DG.Tweening;
using System;

public class Wap : WordPoolBase
{
    [SerializeField] float showTime = 0.1f;
    [SerializeField] float hideTime = 1f;
    [SerializeField] float maxTransparency = 0.5f;
    Tween setColorTween = null;
    [SerializeField] float tweenTime = 0.0f;
    [SerializeField] float tweenEndValue = 0.0f;
    [SerializeField] SpriteRenderer mainColor;

    [SerializeField, ReadOnly] GameObject article;
    [SerializeField, ReadOnly] Vector2 point;

    public override void OnInit()
    {
        var color = mainColor.color;
        color.a = 0;
        mainColor.color = color;

        if (mainColor == null)
        {
            mainColor.GetComponent<SpriteRenderer>();
        }
    }

    public override void OnSetInit(object[] value)
    {
        var scale = (Vector3)value[0];
        transform.localScale = scale;
    }

    private void OnMouseEnter()
    {
        BattleSceneManager.Instance.SetMouseWap(this);
        tweenTime = showTime;
        tweenEndValue = maxTransparency;
        SetMouseWap();
    }

    private void OnMouseExit()
    {
        tweenTime = hideTime;
        tweenEndValue = 0.0f;
        SetMouseWap();
    }

    private void SetMouseWap()
    {
        setColorTween.Kill();
        setColorTween = DOTween.To(() => mainColor.color.a, value =>
        {
            var color = mainColor.color;
            color.a = value;
            mainColor.color = color;
        }, tweenEndValue, tweenTime);
        setColorTween.Play();
    }

    public bool TryGetObject<T>(out T com) where T : class
    {
        bool ret;
        try
        {
            com = article.GetComponent<T>();
            ret = true;
        }
        catch (Exception exp)
        {
            com = null;
            ret = false;
        }
        return ret;
    }

    public void SetArticle(GameObject target)
    {
        article = target;
    }

    public Vector2 GetPoint()
    {
        return point;
    }
    public void SetPoint(Vector2 point)
    {
        this.point = point;
    }
}
