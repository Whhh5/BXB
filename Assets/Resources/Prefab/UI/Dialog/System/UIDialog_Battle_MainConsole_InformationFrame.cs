using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using BXB.Core;
using DG.Tweening;
using System;

public class UIDialog_Battle_MainConsole_InformationFrame : UIElementPoolBase
{
    public enum Active
    {
        Show,
        Hide,
    }
    [SerializeField] ulong objId;
    [SerializeField] MiUIText objName;
    [SerializeField] MiUIText blood;
    [SerializeField] MiUIText attack;
    [SerializeField] MiUIText defend;
    [SerializeField] MiUIText attackInterval;

    [SerializeField] Image icon;
    [SerializeField] CanvasGroup group;

    [SerializeField] Vector2 showPos;
    [SerializeField] Vector2 hidePos;

    Tween tween_Active = null;
    Tween tween_Move = null;

    public override void OnInit()
    {

    }

    public override void OnSetInit(object[] value)
    {
        objId = (ulong)value[0];
        WapObjBase obj = (WapObjBase)value[1];

        var data = MasterData.Instance.GetTableData<LocalRolesData>(objId);
        var name = data.name;
        var level = obj.GetSetLevel();
        var propertys = MasterData.Instance.GetTableData<LocalRolesLevelData>((ulong)level);
        var attack = propertys.attack;
        var defend = propertys.defend;
        var attackInterval = propertys.attackInterval;
        var nowBlood = obj.GetSetBlood();

        objName.SetRawText(name).Wait();
        blood.SetRawText(" HP "+nowBlood).Wait();
        this.attack.SetRawText("¹¥»÷ " + attack).Wait();
        this.defend.SetRawText("·ÀÓù "+defend).Wait();
        this.attackInterval.SetRawText("ËÙ¶È "+attackInterval).Wait();

        string path = CommonManager.Instance.filePath.ResImSpIcon;
        icon.sprite = ResourceManager.Instance.Load<Sprite>(path, objId.ToString());

        main.GetComponent<RectTransform>().anchoredPosition = hidePos;
    }

    public override async Task OnSetInitAsync<T>(params object[] value)
    {
        await AsyncDefaule();
    }

    public void SetTween(Active mode, float time, Action endFunc)
    {
        float toAlpha = 0.0f;
        Vector2 toPos = new Vector3();
        switch (mode)
        {
            case Active.Show:
                toAlpha = 1;
                toPos = showPos;
                break;
            case Active.Hide:
                toAlpha = 0;
                toPos = hidePos;
                break;
        }

        tween_Active?.Kill();
        tween_Move?.Kill();
        tween_Active = DOTween.To(() => group.alpha, value => { group.alpha = value; }, toAlpha, time);
        var mainRect = main.GetComponent<RectTransform>();
        tween_Move = DOTween.To(() => mainRect.anchoredPosition.x, value => 
        {
            var anch = mainRect.anchoredPosition;
            anch.x = value;
            var d3 = mainRect.anchoredPosition3D;
            var local = mainRect.localPosition;
            mainRect.anchoredPosition = anch;
        }, toPos.x, time)
            .OnComplete(() => 
            {
                endFunc.Invoke();
            });
        tween_Active.Play();
        tween_Move.Play();

    }

}
