using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using BXB.Core;
using System;

public class UIDialog_Battle_MainConsole_Consumable : UIElementPoolBase
{
    [SerializeField] string consumableName;
    [SerializeField] ulong consumableId;
    [SerializeField] List<string> effects = new List<string>();
    [SerializeField] Image icon;
    [SerializeField] MiUIText tex_Number;

    [SerializeField] MiCommonCollider mainController;

    [TextArea] [SerializeField] string information;

    public ulong GetItemId()
    {
        return consumableId;
    }
    public override void OnInit()
    {

    }

    public override void OnSetInit(object[] value)
    {
        var id = (ulong)value[0];
        var number = (int)value[1];

        var tableData = MasterData.Instance.GetTableData<LocalItemData>(id);

        var name = tableData.name;
        var effs = new List<string>(tableData.effect);
        var infor = "None";

        var spritePath = CommonManager.Instance.filePath.ResImSpIcon;
        try
        {
            var sprite = ResourceManager.Instance.Load<Sprite>(spritePath, id.ToString());
            icon.sprite = sprite;
        }
        catch (Exception)
        {
        }
        tex_Number.SetRawText(number.ToString()).Wait();
        consumableId = id;
        consumableName = name;
        effects = effs;
        information = infor;
    }

    public override async Task OnSetInitAsync<T>(params object[] value)
    {
        await AsyncDefaule();
    }
}
