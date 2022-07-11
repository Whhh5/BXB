using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;

public abstract class WapObjBase : MiObjPoolPublicParameter, ICommon_GameObject
{
    [SerializeField] protected GameObject main;
    [SerializeField] protected Vector2 point = new Vector2(-1, -1);
    [SerializeField] protected Vector2 scope = new Vector2(0, 0);

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
    public abstract void OnInit();
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
}
