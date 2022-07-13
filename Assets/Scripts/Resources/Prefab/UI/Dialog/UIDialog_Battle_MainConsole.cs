using BXB.Core;
using DG.Tweening;
using System;
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

    [SerializeField] Vector2 itemLoopBtnShowAndHideTime = new Vector2(0.5f, 1.0f);
    [SerializeField] bool handleActive = true;

    [SerializeField] MiUIText goldText;
    [SerializeField] RectTransform enemyInformationListParent;
    [SerializeField, ReadOnly] List<MiUIBase> enemyList = new List<MiUIBase>();

    [SerializeField] Image playerIcon;
    [SerializeField] MiUIText tex_PlayerName;
    [SerializeField] MiUIText tex_Blood;
    [SerializeField] MiUIText tex_Attack;
    [SerializeField] MiUIText tex_Defent;
    [SerializeField] MiUIText tex_AttackInterval;

    public override void OnInit()
    {
        ShowAsync().Wait();
        Tween activeTween = DOTween.To(() => 2, value => { }, 0, 1);
        btn_LeftLoop.onClickPersist.RemoveAllListeners();
        btn_RightLoop.onClickPersist.RemoveAllListeners();
        btn_ShowAndHideHandle.onClick.RemoveAllListeners();
        LoopBtnActiveTween(0, itemLoopBtnShowAndHideTime.y);

        btn_LeftLoop.AddOnPointerLongDownClick(async () =>
        {
            await AsyncDefaule();
            Btn_LeftLoopOnPointerLongDownClick();
        });
        btn_RightLoop.AddOnPointerLongDownClick(async () =>
        {
            await AsyncDefaule();
            Btn_RightLoopOnPointerLongDownClick();
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
            activeTween?.Kill();
            var oldAlpha = loopBtnObjs.alpha;
            activeTween = DOTween.To(() => oldAlpha, value => { loopBtnObjs.alpha = value; }, endAlpha, time);
            activeTween.Play();
        }


        btn_ShowAndHideHandle.AddOnPointerClick(async () =>
        {
            await AsyncDefaule();
            btn_ShowAndHideHandleClick();
        });

        handleActive = true;
    }
    private void Btn_RightLoopOnPointerLongDownClick()
    {
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
    }
    private void Btn_LeftLoopOnPointerLongDownClick()
    {
        var anch = itemMainList.anchoredPosition3D;
        if (anch.x > 0)
        {
            anch.x = 0;
        }
        anch.x += itemListMoveunit;
        itemMainList.anchoredPosition3D = Vector3.Lerp(itemMainList.anchoredPosition3D, anch, itemListMoveSpeed * Time.deltaTime);
    }

    void btn_ShowAndHideHandleClick()
    {
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
        anima.GetClip(animaName).events = null;
        anima.Play(animaName);
        handleActive = !handleActive;
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

    public void AddSystemItems(BattleSceneManager.ItemsType itemsType, int number)
    {
        switch (itemsType)
        {
            case BattleSceneManager.ItemsType.None:
                break;
            case BattleSceneManager.ItemsType.Gold:
                int oldNumber = int.Parse(goldText.GetRawText());
                oldNumber += number;
                goldText.SetRawText(oldNumber).Wait();
                break;
            case BattleSceneManager.ItemsType.EnumCount:
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(keyCode))
                {
                    switch (keyCode)
                    {
                        case KeyCode.Tab:
                            btn_ShowAndHideHandleClick();
                            break;
                        case KeyCode.Alpha1:

                            break;
                        case KeyCode.Alpha2:

                            break;
                        case KeyCode.Alpha3:

                            break;
                        case KeyCode.Alpha4:

                            break;
                        case KeyCode.Alpha5:

                            break;
                        default:
                            break;
                    }
                }
            }
        }
        if (Input.anyKey)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(keyCode))
                {
                    switch (keyCode)
                    {
                        case KeyCode.Q:
                            Btn_LeftLoopOnPointerLongDownClick();
                            break;
                        case KeyCode.E:
                            Btn_RightLoopOnPointerLongDownClick();
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    public async Task ShowEnemyList(List<WapObjBase> wapObjs)
    {
        await AsyncDefaule();
        foreach (var item in enemyList)
        {
            item.Destroy();
        }
        enemyList.Clear();
        foreach (var item in wapObjs)
        {
            var path = CommonManager.Instance.filePath.PreUIDialogSystemPath;
            var obj = await ResourceManager.Instance.GetUIElementAsync<UIDialog_Battle_MainConsole_EnemyInformation>(path, "UIDialog_Battle_MainConsole_EnemyInformation", enemyInformationListParent, Vector3.zero, item);
            enemyList.Add(obj);
        }
    }

    public async Task UpdatePlayerProperty()
    {
        await AsyncDefaule();
        var mainPlayer = BattleSceneManager.Instance.mainPlayer;
        await tex_PlayerName.SetRawText(mainPlayer.property.name);
        await tex_Blood.SetRawText(mainPlayer.nowBlood);
        await tex_Attack.SetRawText(mainPlayer.property.attack);
        await tex_Defent.SetRawText(mainPlayer.property.defence);
        await tex_AttackInterval.SetRawText(mainPlayer.property.attackInterval);
        playerIcon.sprite = ResourceManager.Instance.Load<Sprite>($"Images/Sprite/Icon", mainPlayer.GetId().ToString());
    }
    public int GetGlod()
    {
        return int.Parse(goldText.GetRawText());
    }
}
