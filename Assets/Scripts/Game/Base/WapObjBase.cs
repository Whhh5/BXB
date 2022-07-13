using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;

public abstract class WapObjBase : MiObjPoolPublicParameter, ICommon_Weapon
{
    enum StatusMode
    {
        None,
        Trusteeship, //托管
        EnumCount,
    }
    [SerializeField] protected GameObject main;
    public float nowBlood;
    [SerializeField] protected Vector2 point = new Vector2(-1, -1);
    [SerializeField] protected Vector2 scope = new Vector2(0, 0);
    [SerializeField] protected string recruitSetArticle = "1:10;2:20;3:30";
    [SerializeField] protected string recruitDemandArticle = "120050001:1";//120050001:金币

    [SerializeField] StatusMode mode;

    public ObjBaseproperty property;

    public bool TryGetMianCom<T>(out T component) where T : class
    {
        bool ret = false;
        if (main.TryGetComponent<T>(out component))
        {
            ret = true;
        }
        return ret;
    }
    public GameObject GetMain()
    {
        return main;
    }
    public virtual void OnInit()
    {
        var tableData = MasterData.Instance.GetTableData<LocalRolesData>(GetId());
        nowBlood = property.maxBlood = tableData.maxBlood;
        property.name = tableData.name;
        property.attack = tableData.attack;
        property.defence = tableData.armor;
        property.attackInterval = 2.0f;
    }
    public abstract void OnSetInit(object[] value);
    public object Clone()
    {
        return MemberwiseClone();
    }


    public void SetPont(Vector2 point)
    {
        this.point = point;
    }
    public Vector2 GetPoint()
    {
        return point;
    }

    public abstract void Active(params object[] value);

    private void Update()
    {
        switch (mode)
        {
            case StatusMode.None:
                break;
            case StatusMode.Trusteeship:
                TrusteeshipMove();
                break;
            default:
                break;
        }
    }

    //托管移动逻辑
    private void TrusteeshipMove()
    {

    }

    public abstract void Die();
    public virtual bool TryRecruit(WapObjBase obj,out string article)
    {
        bool ret = false;
        article = null;
        var glod = BattleSceneManager.Instance.mainConsole.GetGlod();

        foreach (var item in recruitDemandArticle.Split(';'))
        {
            var para = item.Split(':');
            var number = int.Parse(para[1]);
            switch (int.Parse(para[0]))
            {
                case 120050001:
                    if (glod >= number)
                    {
                        BattleSceneManager.Instance.mainConsole.AddSystemItems(BattleSceneManager.ItemsType.Gold, -number);
                        ret = true;
                        article = recruitSetArticle;
                    }
                    break;
                default:
                    break;
            }
        }

        return ret;
        
    }
}
