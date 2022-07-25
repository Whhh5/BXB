using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using System.Threading.Tasks;
using DG.Tweening;

public class UIElement_NumberHint : UIElementPoolBase
{
    [SerializeField] MiUIText number;
    [SerializeField] RectTransform mainRect;

    
    public override void OnInit()
    {
        ShowAsync().Wait();
        DOTween.To(() => 2, value => { }, 0, showClip.length)
            .OnComplete(() =>
            {
                Destroy();
            });
    }

    public override void OnSetInit(object[] value)
    {
        var text = (float)value[0];
        number.SetRawText(text).Wait();

        Color color;
        switch (text > 0)
        {
            case true:
                color = Color.green;
                break;
            case false:
                color = Color.red;
                break;
        }
        number.SetColor(color).Wait();
    }

    public override async Task OnSetInitAsync<T>(params object[] value)
    {
        await AsyncDefaule();
    }
}
