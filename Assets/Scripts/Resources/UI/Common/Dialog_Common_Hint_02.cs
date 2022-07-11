using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using DG.Tweening;
using System;
using BXB.Core;

public class Dialog_Common_Hint_02 : MiUIDialog
{
    [SerializeField] CanvasGroup mainGroup;
    [SerializeField] RectTransform rect;
    [SerializeField, Range(0, 20)] float hintMoveSpeed;
    [SerializeField] bool isEnabled = true;
    [SerializeField] Vector2 showAndHideTime;
    #region Old
    protected override async Task OnStartAsync()
    {
        await base.OnStartAsync();

        mainGroup.alpha = 0;
    }
    private void Update()
    {
        if (isEnabled)
        {
            Move();
        }
    }

    private async void Move()
    {
        await AsyncDefaule();
        rect.anchoredPosition3D = Vector3.Lerp(rect.anchoredPosition3D, Input.mousePosition, hintMoveSpeed * Time.deltaTime);
    }

    public async Task SetUpShowAsync(GameObject primary,List<RectTransform> rects,bool active)
    {
        await base.OnShowAsync();
        Vector2 endAlphaAndTime;
        if (active)
        {
            endAlphaAndTime.x = 1;
            endAlphaAndTime.y = showAndHideTime.x;
        }
        else
        {
            endAlphaAndTime.x = 0;
            endAlphaAndTime.y = showAndHideTime.y;
        }
        gameObject.SetActive(active);
        DOTween.To(() => mainGroup.alpha, value => { mainGroup.alpha = value; }, endAlphaAndTime.x, showAndHideTime.y)
            .OnStart(() =>
            {
                mainGroup.alpha = 0;
            })
            .OnComplete(() => { }).SetUpdate(false);
    }
    #endregion

    #region New

    [SerializeField] RectTransform listParent;
    [SerializeField] GameObject article;
    [SerializeField] List<UIElementPoolBase> articles = new List<UIElementPoolBase>();
    public async Task Active(Func<Task<Sprite>> F_icon, Func<Task<string>> f_text)
    {
        var icon = await F_icon.Invoke();
        var text = await f_text.Invoke();
        var obj = await ObjPool.GetObjectAsync(article);
        var cs = obj.GetComponent<Common_Article_3>();
        var rect = obj.GetComponent<RectTransform>();
        await cs.SetUp(icon, text);
        rect.Normalization(listParent);
        articles.Add(cs);
    }
    public async Task RemoveAllActicle()
    {
        foreach (var parameter in articles)
        {
            parameter.Destroy();
        }
        articles.Clear();
        await AsyncDefaule();
    }

    public override void OnInit()
    {
        
    }

    public override void OnSetInit(object[] value)
    {
    }

    public override async Task OnSetInitAsync<T>(params object[] value)
    {
        await RemoveAllActicle();
        var f_articles = (T)value[0];
        var type = typeof(T);
        var fields = type.GetFields();
        foreach (var para in fields)
        {
            string paraValue = "";
            var nowParavalue = para.GetValue(f_articles);
            if (string.Equals(nowParavalue.GetType().ToString(), "System.String[]") || string.Equals(para.GetType(), "List<*>"))
            {
                var listValue = (string[])para.GetValue(f_articles);
                foreach (var list in listValue)
                {
                    paraValue += list + '\n';
                }
                paraValue = paraValue.Substring(0, paraValue.Length - 1);
            }
            else
            {
                paraValue = nowParavalue.ToString();
            }
            var text = $"{para.Name}: {paraValue}";
            Sprite icon = null;
            //10104020003
            var path = CommonManager.Instance.filePath.PreUIElementPath;
            //var cs = await TableManager.Instance.tableData.GetUIElement<LocalizeUIDialogData, UIElementPoolBase>(path, 10104020003, listParent, Vector3.zero, icon, text);

            //
            //var obj = await ObjPool.GetObjectAsync(article);
            //var cs = obj.GetComponent<Common_Article_3>();
            //var rect = obj.GetComponent<RectTransform>();
            //await cs.SetUp(icon, text);
            //rect.Normalization(listParent);
            //obj.SetActive(true);
            //
            //articles.Add(cs);
        }
        await ShowAsync();
        await PlayTween(1, async () => { await AsyncDefaule(); }, async () => { await AsyncDefaule(); });

    }

    Tween temporaryTween = null;
    public override async void Destroy()
    {
        await PlayTween(0, async () => { await AsyncDefaule(); }, async () =>
         {
             base.Destroy();
             await RemoveAllActicle();
         });
    }

    private async Task PlayTween(float endValue, Func<Task> startFunc, Func<Task> endFunc)
    {
        if (temporaryTween != null)
        {
            temporaryTween.Kill();
        }
        temporaryTween = DOTween.To(() => mainGroup.alpha, value => { mainGroup.alpha = value; }, endValue, 1)
            .OnStart(async () =>
            {
                await startFunc.Invoke();
            })
            .OnComplete(async () =>
            {
                await endFunc.Invoke();
            });
        temporaryTween.Play();
        await AsyncDefaule();
    }













    #endregion
}
