using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using BXB.Core;

public class UIDialog_Battle_MainConsole_EnemyInformation : UIElementPoolBase
{
    [SerializeField] MiUIText nameTxt;
    [SerializeField] MiUIText attackTxt;
    [SerializeField] MiUIText defenseTxt;

    [SerializeField] Image iconImage;
    [SerializeField] MiUIButton btn_Battle;
    [SerializeField] MiUIButton btn_recruit;

    [SerializeField, ReadOnly] WapObjBase enemy;
    protected override void OnAwake()
    {
        base.OnAwake();
    }
    protected override async Task OnAwakeAsync()
    {
        await base.OnAwakeAsync();
    }
    protected override void OnStart()
    {
        base.OnStart();
    }
    protected override async Task OnStartAsync()
    {
        await base.OnStartAsync();
    }


    public override void OnInit()
    {
        btn_Battle.onClick.RemoveAllListeners();
        btn_recruit.onClick.RemoveAllListeners();

        btn_Battle.AddOnPointerClick(async () =>
        {
            await AsyncDefaule();
            SceneDataManager.Instance.ActiveBattle(BattleSceneManager.Instance.mainPlayer, enemy);
        });
        btn_recruit.AddOnPointerClick(async () =>
        {

            var path = CommonManager.Instance.filePath.PreUIDialogSystemPath;
            await ResourceManager.Instance.ShowDialogAsync<MiUIDialog>(path, "Dialog_RecruitConfirmWidget", CanvasLayer.System, enemy);
        });

        ShowAsync().Wait();
    }

    public override void OnSetInit(object[] value)
    {
        var obj = (WapObjBase)value[0];

        var name = obj.GetName();
        var attack = obj.GetSet(WapObjBase.PropertyFloat.attack);
        var defenese = obj.GetSet(WapObjBase.PropertyFloat.defend);
        var icon = ResourceManager.Instance.Load<Sprite>($"Images/Sprite/Icon", obj.GetId().ToString());

        nameTxt.SetRawText(name).Wait();
        attackTxt.SetRawText(attack).Wait();
        defenseTxt.SetRawText(defenese).Wait();
        iconImage.sprite = icon;
        enemy = obj;
    }

    public override async Task OnSetInitAsync<T>(params object[] value)
    {
        await AsyncDefaule();

        transform.GetChild(0);
    }
}
