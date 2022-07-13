using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;

public abstract class WapObjBase : MiObjPoolPublicParameter, ICommon_Weapon
{
    enum StatusMode
    {
        None,
        Trusteeship, //ÍÐ¹Ü
        EnumCount,
    }
    [SerializeField] protected GameObject main;
    public float nowBlood;
    [SerializeField] protected Vector2 point = new Vector2(-1, -1);
    [SerializeField] protected Vector2 scope = new Vector2(0, 0);

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

    //ÍÐ¹ÜÒÆ¶¯Âß¼­
    private void TrusteeshipMove()
    {

    }

    public abstract void Die();
    public virtual string Recruit()
    {
        BattleSceneManager.Instance.RemoveEnemyObj(this);
        return "1:10;2:20;3:30";
    }
}
