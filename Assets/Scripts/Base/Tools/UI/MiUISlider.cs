using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[ExecuteInEditMode]
public class MiUISlider : BXB.Core.MiBaseMonoBeHaviourClass
{
    [SerializeField] RectTransform main;
    [SerializeField] RectTransform targetGraphic;
    [SerializeField] RectTransform fillrect;
    [SerializeField] RectTransform degree;
    [SerializeField] Text number;
    [SerializeField, Range(0,10)] float moveTime;
    [SerializeField, Range(0, 1)] float testSc;

    protected override void OnStart()
    {
        base.OnStart();
        //SetValue(0);
    }
    private void Update()
    {
        if (!Application.isPlaying)
        {
            SetValueTest(testSc);
        }
    }
    private void SetValueTest(float value)
    {
        value = value > 1.0f ? 1.0f : value;
        value = value < 0.0f ? 0.0f : value;
        number.text = (value * 100).ToString("#0.00");
        targetGraphic.anchoredPosition3D = new Vector3(main.rect.width * value, 0, 0);
        fillrect.sizeDelta = new Vector2(main.rect.width * (value), fillrect.sizeDelta.y);
    }
    /// <summary>
    /// value is 0 to 1
    /// </summary>
    /// <param name="value"></param>
    public void SetValue(float value)
    {
        value = value > 1.0f ? 1.0f : value;
        value = value < 0.0f ? 0.0f : value;

        //lerp
        //DOTween.To(() => float.Parse(number.text), value => 
        //{
            
        //}, value, moveTime)
        //    .SetUpdate(false);

        //targetGraphic.DOAnchorPos3DX(main.rect.width * value, moveTime, false).SetUpdate(false);
        //fillrect.DOSizeDelta(new Vector2(-main.rect.width * (1 - value), fillrect.sizeDelta.y), moveTime, false).SetUpdate(false);

        //
        number.text = (value * 100).ToString("#0.00") + "%";
        var anchored = targetGraphic.anchoredPosition3D;
        anchored.x = main.rect.width * value;
        targetGraphic.anchoredPosition3D = anchored;

        var sizedelta = fillrect.sizeDelta;
        sizedelta.x = -main.rect.width * (1 - value);
        fillrect.sizeDelta = sizedelta;
    }

    public void OnInspectorGUI()
    {
        degree.sizeDelta = main.sizeDelta;
        main.anchoredPosition3D = Vector3.zero;
        main.localScale = Vector3.one;
    }
}

