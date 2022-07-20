using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using System.Threading.Tasks;
using DG.Tweening;

public class UIDialog_TextPopup : MiUIDialog
{
    public enum Mode
    {
        Show,
        Hide,
    }
    [SerializeField] List<UIElement_CommonText> list_texts = new List<UIElement_CommonText>();
    [SerializeField] int lineMaxCount = 10;
    [SerializeField, Range(0, 200)] float lineInterval = 50;
    [SerializeField, Range(0, 200)] float textInterval = 100;
    [SerializeField] RectTransform textParent;
    [SerializeField] Vector2 startLocalEndPosOffset = new Vector2(0, 10);
    [SerializeField, Range(0, 1)] float startAlpha = 0.0f;
    [SerializeField, Range(0, 1)] float endAlpha = 1.0f;
    [SerializeField, Range(0, 1)] float intervalTime = 0.1f;
    [SerializeField] CanvasGroup group;
    [SerializeField] bool isRunning = false;
    [SerializeField] MiUIButton btn_close;
    public override void OnInit()
    {
        ShowAsync().Wait();
        btn_close.onClick.RemoveAllListeners();
        btn_close.AddOnPointerClick(async () =>
        {
            if (!isRunning)
            {
                Destroy();
            }
            else
            {
                BattleSceneManager.Instance.mainConsole.HintInformation("Plase wait ... ").Wait();
            }
        });
    }

    public override void OnSetInit(object[] value)
    {
        var str = (string)value[0];
        StartCoroutine(CreateTextPrefab(str, Mode.Show));
        DOTween.To(() => startAlpha, value => { group.alpha = value; }, endAlpha, 1.0f);
    }

    public override async Task OnSetInitAsync<T>(params object[] value)
    {
        await AsyncDefaule();
    }

    IEnumerator CreateTextPrefab(string str, Mode mode)
    {
        isRunning = true;
        int lineTextCount;
        int line = 0;
        var path = CommonManager.Instance.filePath.PreUIElementPath;
        float startAlpha = 0.0f;
        float endAlpha = 0.0f;
        switch (mode)
        {
            case Mode.Show:
                startAlpha = this.startAlpha;
                endAlpha = this.endAlpha;
                break;
            case Mode.Hide:
                startAlpha = this.endAlpha;
                endAlpha = this.startAlpha;
                break;
            default:
                break;
        }
        for (int i = 0; i < str.Length; i++)
        {
            lineTextCount = i % lineMaxCount;
            if (i >= (line + 1) * 10)
            {
                line++;
            }
            var endPos = new Vector2(lineTextCount * textInterval, -lineInterval * line);
            var obj = ResourceManager.Instance.GetUIElement<UIElement_CommonText>(path, "UIElement_CommonText", textParent, Vector3.zero, "");
            var rect = obj.GetComponent<RectTransform>();
            obj.SetRawText(str[i].ToString());
            rect.anchoredPosition = endPos + startLocalEndPosOffset;
            var time = intervalTime;
            rect.DOAnchorPos3D(endPos, time);
            DOTween.To(() => startAlpha, value => { obj.GetGroup().alpha = value; }, endAlpha, time);
            list_texts.Add(obj);
            yield return new WaitForSeconds(intervalTime);
        }
        isRunning = false;
    }
    public override void Destroy()
    {
        if (!isRunning)
        {
            DOTween.To(() => endAlpha, value => { group.alpha = value; }, startAlpha, 1.0f)
                .OnComplete(()=> 
                {
                    base.Destroy();
                });
        }
    }
}
