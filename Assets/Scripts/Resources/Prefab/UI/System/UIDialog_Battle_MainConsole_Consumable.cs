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
    [SerializeField] int number;
    [SerializeField] MiCommonCollider mainController;

    [TextArea] [SerializeField] string information;
    [SerializeField] MiUIButton miUIButton;
    [SerializeField] CharacterController characterController;
    [SerializeField] UIDialog_Battle_MainConsole_InformationFrame uIDialog_Battle_MainConsole_InformationFrame;


    public ulong GetItemId()
    {
        return consumableId;
    }
    public override void OnInit()
    {
        
    }

    public async Task UseItem()
    {
        UIDialog_Battle_MainConsole uIDialog_Battle_MainConsole = BattleSceneManager.Instance.mainConsole;
        
        number=number-1;
        Log(Color.red, "UseItem"+number);
        if (number > 0)
        {
           Dictionary<ulong,int> dic= BattleSceneManager.Instance.mainPlayer.consumableGet;
            dic.Remove(consumableId);
            dic.Add(consumableId,number);
        }
        else
        {
            BattleSceneManager.Instance.mainPlayer.consumableGet.Remove(consumableId);
            Destroy();
            Log(Color.red, "number==0");
        }

        SetMainPlayer();
        SetLegion();
        uIDialog_Battle_MainConsole.UpdatePlayerProperty();
        //await AsyncDefaule();
    }

    private void SetMainPlayer()//使用物品修改自身属性
    {
        characterController = SceneDataManager.Instance.mainPlayer;
        LocalItemData localItem = null;
        MasterData.Instance.LocalItemData.TryGetValue(GetItemId(), out localItem);
        string[] effects = localItem.effect;
        for (int i = 0; i < effects.Length; i++)
        {
            string[] effect = effects[i].Split(':');
            int mode = int.Parse(effect[0]);
            switch (mode)
            {
                case 1:
                    SetBlood(characterController, mode, float.Parse(effect[1]));//加血
                    break;
                case 2:
                    SetAttack(characterController, mode, float.Parse(effect[1]));//加攻击
                    break;
                case 3:
                    SetDefend(characterController, mode, float.Parse(effect[1]));//加防御
                    break;
                case 4:
                    SetAttackInterval(characterController, mode, float.Parse(effect[1]));//加攻击间隔
                    break;
                default:
                    break;
            }
        }
        UIDialog_Battle_MainConsole uIDialog_Battle_MainConsole = BattleSceneManager.Instance.mainConsole;
        //uIDialog_Battle_MainConsole.UpdatePlayerProperty();
    }
    private void SetLegion()//使用物品修改队友属性
    {
        characterController = SceneDataManager.Instance.mainPlayer;
        LocalItemData localItem = null;
        MasterData.Instance.LocalItemData.TryGetValue(GetItemId(), out localItem);
        string[] effects = localItem.effect;
        List<WapObjBase> legionPoint = characterController.GetSetLegion();
        for (int j = 0; j < legionPoint.Count; j++)
        {

            WapObjBase wapObj = legionPoint[j];
            for (int i = 0; i < effects.Length; i++)
            {
                string[] effect = effects[i].Split(':');
                int mode = int.Parse(effect[0]);
                switch (mode)
                {
                    case 1:
                        SetBlood(wapObj, mode, float.Parse(effect[1]));//加血

                        break;
                    case 2:
                        SetAttack(wapObj, mode, float.Parse(effect[1]));//加攻击
                        break;
                    case 3:
                        SetDefend(wapObj, mode, float.Parse(effect[1]));//加防御
                        break;
                    case 4:
                        SetAttackInterval(wapObj, mode, float.Parse(effect[1]));//加攻击间隔
                        break;
                    default:
                        break;
                }
            }
        }
        UIDialog_Battle_MainConsole uIDialog_Battle_MainConsole = BattleSceneManager.Instance.mainConsole;
        //uIDialog_Battle_MainConsole.UpdatePlayerProperty();
    }
    private void SetBlood(WapObjBase wapObj,int mode,float effect)//修改属性
    {
        wapObj.GetSetBlood(effect);     
    }
    private void SetAttack(WapObjBase wapObj, int mode, float effect)//修改属性
    {
        wapObj.GetSet(WapObjBase.PropertyFloat.attack,effect);
    }

    private void SetDefend(WapObjBase wapObj, int mode, float effect)//修改属性
    {
        wapObj.GetSet(WapObjBase.PropertyFloat.defend,effect);
    }
    private void SetAttackInterval(WapObjBase wapObj, int mode, float effect)//修改属性
    {
        wapObj.GetSet(WapObjBase.PropertyFloat.attackInterval,effect);
    }

    public override void OnSetInit(object[] value)
    {
        var id = (ulong)value[0];
        number = (int)value[1];

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
        miUIButton = transform.GetComponentInChildren<MiUIButton>();
        if (miUIButton != null)
        {
            miUIButton.onClick.RemoveAllListeners();
            miUIButton.AddOnPointerClick(UseItem);
        }
        else
        {
            Log(Color.black,"miUIButton为null");
        }
    }

    public override async Task OnSetInitAsync<T>(params object[] value)
    {
        await AsyncDefaule();
    }
}
