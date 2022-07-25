using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;

public class Wap : WordPoolBase
{
    [SerializeField] float showTime = 0.1f;
    [SerializeField] float hideTime = 1f;
    [SerializeField] float maxTransparency = 0.5f;
    Tween setColorTween = null;
    [SerializeField] SpriteRenderer mainSprite;

    [SerializeField, ReadOnly] GameObject article;
    [SerializeField, ReadOnly] Vector2 point;

    [SerializeField] UIDialog_Battle_MainConsole_InformationFrame infor = null;

    [SerializeField] List<Wap> scopeList = new List<Wap>();
    [SerializeField] float defentAlphaA = 0.2f;
    public override void OnInit()
    {
        if (mainSprite == null)
        {
            mainSprite.GetComponent<SpriteRenderer>();
        }
        var color = mainSprite.color;
        color.a = 0;
        mainSprite.color = color;
    }

    public override void OnSetInit(object[] value)
    {
        var scale = (Vector3)value[0];
        transform.localScale = scale;
    }

    private void OnMouseEnter()
    {
        Debug.Log("ON Mouse Enter!");
        SceneDataManager.Instance.SetMouseWap(this);
        SetMouseWap(maxTransparency, showTime, Color.green);
        ShowObjInformation().Wait();
        if (infor != null)
        {
            if (article != null)
            {
                var obj = article.GetComponent<WapObjBase>();
                scopeList = obj.GetAttackScope();
                foreach (var item in scopeList)
                {
                    item.SetMouseWap(0.2f, 0.2f, Color.red);
                }
            }
            infor.SetTween(UIDialog_Battle_MainConsole_InformationFrame.Active.Show, 0.5f, () => { });

        }
    }

    private void OnMouseExit()
    {
        Debug.Log("ON Mouse Exit!");
        SetMouseWap(0.0f, hideTime, Color.green);
        if (infor != null)
        {
            var temp = infor;
            infor.SetTween(UIDialog_Battle_MainConsole_InformationFrame.Active.Hide, 0.5f, () =>
            {
                temp.Destroy();
            });
            foreach (var item in scopeList)
            {
                item.SetMouseWap(0.0f, 0.2f, Color.red);
            }
            scopeList = new List<Wap>();
        }
    }

    public void SetMouseWap(float tweenEndValue, float tweenTime, Color color)
    {
        try
        {
            setColorTween?.Kill();
            color.a = defentAlphaA;
            mainSprite.color = color;
            setColorTween = DOTween.To(() => mainSprite.color.a, value =>
            {
                var color = mainSprite.color;
                color.a = value;
                mainSprite.color = color;
            }, tweenEndValue, tweenTime);
            setColorTween.Play();
        }
        catch (Exception)
        {
        }

    }

    public bool TryGetObject<T>(out T com) where T : class
    {
        bool ret;
        try
        {
            com = article.GetComponent<T>();
            ret = true;
        }
        catch (Exception)
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

    public int GetLayerMask()
    {
        int layer = 0;
        if (article != null)
        {
            layer = article.layer;
        }
        return (int)Mathf.Pow(2, layer);
    }

    public async Task ShowObjInformation()
    {
        await AsyncDefaule();
        if (article != null)
        {
            var obj = article.GetComponent<WapObjBase>();
            infor = await BattleSceneManager.Instance.mainConsole.AddObjectInformation(obj.GetId(), obj);
        }
    }
}
