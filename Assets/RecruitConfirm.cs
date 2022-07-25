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
    [SerializeField]
    private TMP_Text DescText;
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
            Time.timeScale = 1;
        });

        closeBtn.onClick.AddListener(OnCloseBtnClick);
        ShowAsync().Wait();
    }

    public override void OnSetInit(object[] value)
    {
        var obj = (WapObjBase)value[0];
        enemy = obj;

        var demandItems = enemy.GetSet(WapObjBase.PropertyListString.recruitDemandArticle);
        var name = obj.GetName();
        bool canRecruit = true;
        string text = string.Format("{0}愿意加入您,", name);
        string hintStr = "您只需要消耗";
        foreach (var item in demandItems)
        {
            var itemAndNumber = item.Split(':');// id:number 1：10
            var id = ulong.Parse(itemAndNumber[0]);
            var itemName = MasterData.Instance.GetTableData<LocalItemData>(id).name;
            var number = int.Parse(itemAndNumber[1]);
            if (number > 9999)
            {
                canRecruit = false;
            }
            hintStr = hintStr + $"{number}{itemName}";
        }
        if (canRecruit)
        {
            nameText.text = text;
            demandText.text = hintStr;
            DescText.enabled = true;
            confirmBtn.enabled = true;
            confirmBtn.gameObject.SetActive(true);
        }
        else
        {
            string failRecruitText = string.Format("{0}是怪物,", name);
            nameText.text = failRecruitText;
            demandText.text = "您无法招募";
            DescText.enabled = false;
            confirmBtn.enabled = false;
            confirmBtn.gameObject.SetActive(false);
        }

    }
    public override async Task OnSetInitAsync<T>(params object[] value)
    {
        await AsyncDefaule();

        transform.GetChild(0);
    }

    private void OnCloseBtnClick()
    {
        Time.timeScale = 1;
        Destroy();
    }
}
