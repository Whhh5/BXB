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
    [SerializeField] RectTransform informationParent;

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

    [SerializeField] RectTransform playerArticleListParent;
    [SerializeField] List<MiUIBase> playerArticleList = new List<MiUIBase>();
    [SerializeField] RectTransform playerConsumableListParent;
    [SerializeField] public List<MiUIBase> playerConsumableList = new List<MiUIBase>();
    public UIDialog_Battle_MainConsole_UpperCenter upper;
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

    public void AddSystemItems(SceneDataManager.ItemsType itemsType, int number)
    {
        switch (itemsType)
        {
            case SceneDataManager.ItemsType.None:
                break;
            case SceneDataManager.ItemsType.Gold:
                int oldNumber = int.Parse(goldText.GetRawText());
                oldNumber += number;
                goldText.SetRawText(oldNumber).Wait();
                break;
            case SceneDataManager.ItemsType.EnumCount:
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
                            //obj.GetSet(WapObjBase.PropertyFloat.attackInterval)
                            var obj = BattleSceneManager.Instance.mainPlayer;
                            obj.SetStatus(WapObjBase.Status.Attack, 0.1f, true);
                            break;
                        case KeyCode.Alpha2:
                            var path = CommonManager.Instance.filePath.PreUIDialogSystemPath;
                            ResourceManager.Instance.ShowDialogAsync<UIDialog_TextPopup>(path, "UIDialog_TextPopup", CanvasLayer.System, "dasdioagiodugaugfiaguiagidagiudgaioughoagfoahfoahfoahfoahf;oafsjfoiafafasfasfagiagdigaidfa").Wait();
                            break;
                        case KeyCode.Alpha3:
                            SceneDataManager.Instance.GameFinish();
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
        //更新属性
        var mainPlayer = BattleSceneManager.Instance.mainPlayer;
        await tex_PlayerName.SetRawText(mainPlayer.GetName());
        await tex_Blood.SetRawText(mainPlayer.GetSetBlood());
        await tex_Attack.SetRawText(mainPlayer.GetSet(WapObjBase.PropertyFloat.attack));
        await tex_Defent.SetRawText(mainPlayer.GetSet(WapObjBase.PropertyFloat.defend));
        await tex_AttackInterval.SetRawText(mainPlayer.GetSet(WapObjBase.PropertyFloat.attackInterval));
        playerIcon.sprite = ResourceManager.Instance.Load<Sprite>($"Images/Sprite/Icon", mainPlayer.GetId().ToString());

        //更新物品
        foreach (var item in playerArticleList)
        {
            item.Destroy();
        }
        playerArticleList.Clear();
        var articles = BattleSceneManager.Instance.mainPlayer.articleGet;
        foreach (var item in articles)
        {
            var iconPath = CommonManager.Instance.filePath.ResImSpIcon;
            var icon = ResourceManager.Instance.Load<Sprite>(iconPath, item.Key.ToString());
            var path = CommonManager.Instance.filePath.PreUIDialogSystemPath;
            var dialog = await ResourceManager.Instance.GetUIElementAsync<UIDialog_Battle_MainConsole_PlayerOriginal>(path, "UIDialog_Battle_MainConsole_PlayerOriginal", playerArticleListParent, Vector3.zero, icon, item.Value.ToString());
            playerArticleList.Add(dialog);
        }

        //更新消耗品
        foreach (var item in playerConsumableList)
        {
            item.Destroy();
        }
        playerConsumableList.Clear();
        var consumables = BattleSceneManager.Instance.mainPlayer.consumableGet;
        foreach (var item in consumables)
        {
            var iconPath = CommonManager.Instance.filePath.ResImSpIcon;
            var icon = ResourceManager.Instance.Load<Sprite>(iconPath, item.Key.ToString());
            var path = CommonManager.Instance.filePath.PreUIDialogSystemPath;
            var number = item.Value;
            var dialog = await ResourceManager.Instance.GetUIElementAsync<UIDialog_Battle_MainConsole_Consumable>(path, "UIDialog_Battle_MainConsole_Consumable", playerConsumableListParent, Vector3.zero, item.Key, number);
            playerConsumableList.Add(dialog);
        }
        upper.SetUpperCenterParam();
    }
    public int GetGlod()
    {
        return int.Parse(goldText.GetRawText());
    }
    public async Task HintInformation(string hint)
    {
        var path = CommonManager.Instance.filePath.PreUIDialogPath;
        await ResourceManager.Instance.ShowDialogAsync<Dialog_Common_Hint_01>(path, "Dialog_Common_Hint_01", CanvasLayer.System, hint);
    }

    public async Task<UIDialog_Battle_MainConsole_InformationFrame> AddObjectInformation(ulong id, WapObjBase obj)
    {
        var path = CommonManager.Instance.filePath.PreUIDialogSystemPath;
        var dialog = await ResourceManager.Instance.GetUIElementAsync<UIDialog_Battle_MainConsole_InformationFrame>(path, "UIDialog_Battle_MainConsole_InformationFrame", informationParent, Vector3.zero, id, obj);
        return dialog;
    }
}
