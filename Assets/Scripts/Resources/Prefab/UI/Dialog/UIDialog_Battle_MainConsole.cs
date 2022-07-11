using BXB.Core;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UIDialog_Battle_MainConsole : MiUIDialog
{
    [SerializeField] MiUIButton btn_LeftLoop;
    [SerializeField] MiUIButton btn_RightLoop;
    [SerializeField] MiUIButton btn_Items;
    [SerializeField] MiUIButton btn_ShowAndHideHandle;

    [SerializeField] CanvasGroup loopBtnObjs;
    [SerializeField] RectTransform itemMainList;
    [SerializeField] float itemListMoveSpeed = 1.0f;
    [SerializeField] float itemListMoveunit = 30.0f;
    [SerializeField] Vector3 maskInterval;

    [SerializeField] Vector2 itemLoopBtnShowAndHideTime = new Vector2(0.5f,1.0f);

    [SerializeField] bool handleActive = true;

    public override void OnInit()
    {
        ShowAsync().Wait();
        Tween activeTween = DOTween.To(() => 2, value => { }, 0, 1);
        btn_LeftLoop.onClickPersist.RemoveAllListeners();
        btn_RightLoop.onClickPersist.RemoveAllListeners();
        btn_ShowAndHideHandle.onClick.RemoveAllListeners();
        LoopBtnActiveTween(0, itemLoopBtnShowAndHideTime.y);

        Log(color: Color.green, "Defisish");
        btn_LeftLoop.AddOnPointerLongDownClick(async () =>
        {
            await AsyncDefaule();

            var anch = itemMainList.anchoredPosition3D;
            if (anch.x > 0)
            {
                anch.x = 0;
            }
            anch.x += itemListMoveunit;
            itemMainList.anchoredPosition3D = Vector3.Lerp(itemMainList.anchoredPosition3D, anch, itemListMoveSpeed * Time.deltaTime);
        });
        btn_RightLoop.AddOnPointerLongDownClick(async () =>
        {
            await AsyncDefaule();
            var maxDiatance = maskInterval.x - maskInterval.y - maskInterval.z;
            if (itemMainList.childCount != 0)
            {
                var childCount = itemMainList.childCount;
                var hor = itemMainList.GetComponent<HorizontalLayoutGroup>();
                var childRect = itemMainList.GetChild(0).GetComponent<RectTransform>();
                maxDiatance -= childCount * childRect.sizeDelta.x + hor.spacing * (childCount - 1);
            }

            var anch = itemMainList.anchoredPosition3D;
            if (anch.x < maxDiatance)
            {
                anch.x = maxDiatance;
            }
            anch.x -= itemListMoveunit;
            itemMainList.anchoredPosition3D = Vector3.Lerp(itemMainList.anchoredPosition3D, anch, itemListMoveSpeed * Time.deltaTime);
        });

        btn_Items.AddOnPointerEnterClick(async () =>
        {
            await AsyncDefaule();
            LoopBtnActiveTween(1, itemLoopBtnShowAndHideTime.x);
        });
        btn_LeftLoop.AddOnPointerEnterClick(async () =>
        {
            await AsyncDefaule();
            LoopBtnActiveTween(1, itemLoopBtnShowAndHideTime.x);
        });
        btn_RightLoop.AddOnPointerEnterClick(async () =>
        {
            await AsyncDefaule();
            LoopBtnActiveTween(1, itemLoopBtnShowAndHideTime.x);
        });
        btn_RightLoop.AddOnPointerExitClick(async () =>
        {
            await AsyncDefaule();
            LoopBtnActiveTween(0, itemLoopBtnShowAndHideTime.x);
        });
        btn_Items.AddOnPointerExitClick(async () =>
        {
            await AsyncDefaule();
            LoopBtnActiveTween(0, itemLoopBtnShowAndHideTime.y);
        });
        btn_LeftLoop.AddOnPointerExitClick(async () =>
        {
            await AsyncDefaule();
            LoopBtnActiveTween(0, itemLoopBtnShowAndHideTime.y);
        });

        void LoopBtnActiveTween(float endAlpha, float time)
        {

            activeTween.Kill();
            var oldAlpha = loopBtnObjs.alpha;
            activeTween = DOTween.To(() => oldAlpha, value => { loopBtnObjs.alpha = value; }, endAlpha, time);
            activeTween.Play();
        }



        btn_ShowAndHideHandle.AddOnPointerClick(async () =>
        {
            await AsyncDefaule();
            string animaName;
            switch (handleActive)
            {
                case true:
                    animaName = hideClip.name;
                    break;
                case false:
                    animaName = showClip.name;
                    break;
            }
            anima.Play(animaName);
            handleActive = !handleActive;
        });
        handleActive = true;
    }

    public override void OnSetInit(object[] value)
    {

    }

    public override async Task OnSetInitAsync<T>(params object[] value)
    {
        await AsyncDefaule();
    }

    public async Task AddItems(RectTransform target)
    {
        await AsyncDefaule();
    }



}
