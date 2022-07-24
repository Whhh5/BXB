using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using BXB.Core;
using TMPro;
public class RecruitConfirm : MiUIDialog
{
    [SerializeField]
    private Button confirmBtn, closeBtn;
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text demandText;
    [SerializeField, ReadOnly] WapObjBase enemy;
    // Start is called before the first frame update

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
        var demandItems = enemy.GetSet(WapObjBase.PropertyListString.recruitDemandArticle);
        string name = string.Format("{0}愿意加入您,",enemy.name);
        nameText.text = name;

        string hintStr = "您只需要消耗";
        foreach (var item in demandItems)
        {
            var itemAndNumber = item.Split(':');// id:number 1：10
            var id = ulong.Parse(itemAndNumber[0]);
            var itemName = MasterData.Instance.GetTableData<LocalItemData>(id).name;
            var number = int.Parse(itemAndNumber[1]);
            hintStr = hintStr + $"{number}{itemName}";
        }
        Debug.Log(hintStr);
        demandText.text = hintStr;
    }
    protected override async Task OnStartAsync()
    {
        await base.OnStartAsync();
    }

    public override void OnInit()
    {
        confirmBtn.onClick.RemoveAllListeners();
        confirmBtn.onClick.AddListener(async () =>
        {
            await AsyncDefaule();
            SceneDataManager.Instance.RecruitLegion(BattleSceneManager.Instance.mainPlayer, enemy);
            Destroy();
        });

        closeBtn.onClick.AddListener(OnCloseBtnClick);
        ShowAsync().Wait();
    }

    public override void OnSetInit(object[] value)
    {
        var obj = (WapObjBase)value[0];
        var name = obj.GetName();
        obj.GetSet(WapObjBase.PropertyFloat.attack);
        obj.GetSet(WapObjBase.PropertyFloat.defend);
        enemy = obj;

    }
    public override async Task OnSetInitAsync<T>(params object[] value)
    {
        await AsyncDefaule();

        transform.GetChild(0);
    }

    private void OnCloseBtnClick()
    {
        Destroy();
    }
}
