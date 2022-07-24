using UnityEngine;
using BXB.Core;
using System;
using DG.Tweening;

public abstract class BaseGameObject_Game : MiObjPoolPublicParameter, ICommon_Object
{
    [SerializeField, ReadOnly] string objName;
    [SerializeField] protected GameObject main;
    [SerializeField] Animation mainAnima;
    [SerializeField] ObjectType objectType;
    [SerializeField] protected CommonGameObjectInfo objInfo = new CommonGameObjectInfo(0);
    [SerializeField] private float bloodMax;
    [SerializeField] private float _blood;
    [SerializeField] private Action<CommonGameObjectInfo> setBloodClick = (x) => { };
    private float blood
    {
        get
        {
            return _blood;
        }
        set
        {
            value = value > bloodMax ? _blood : value;
            value = value < 0 ? 0 : value;
            _blood = value;
        }
    }
    [SerializeField, ReadOnly] protected float bloodProportion;
    protected override void OnAwake()
    {
        base.OnAwake();
        setBloodClick += (objInfo) =>
        {
            if (objInfo.proportionBlood <= 0)
            {
                Destroy();
            } 
        };
        objInfo.MaxBlood = bloodMax;
        objInfo.lastBlood = blood;
        SetBlood(bloodMax);
    }
    protected override void OnStart()
    {
        base.OnStart();
    }
    public ObjectType GetObjectType()
    {
        return objectType;
    }
    public GameObject GetMain()
    {
        return main;
    }

    public CommonGameObjectInfo GetInfo()
    {
        return objInfo;
    }
    public abstract void OnInit();
    public abstract void OnSetInit(params object[] value);
    public void AddBloodValue(float value)
    {
        blood += value;
        SetProportion();
    }
    public virtual void SetBlood(float value)
    {
        blood = value;
        SetProportion();
    }
    void SetProportion()
    {
        bloodProportion = blood / bloodMax;
        objInfo.SetBlood(blood);
        setBloodClick.Invoke(objInfo);
    }

    public void AddBSetBloodClick(Action<CommonGameObjectInfo> action)
    {
        setBloodClick += action;
    }

    public object Clone()
    {
        return MemberwiseClone();
    }

    public override void Destroy()
    {
        var delayTime = 0.0f;
        if (mainAnima != null)
        {
            var hideClip = mainAnima.GetClip($"{original.name}_Hide");
            if (hideClip != null)
            {
                mainAnima.Play(hideClip.name);
                delayTime = hideClip.length;
            }
        }
        DOTween.To(() => 2, value => { }, 0, delayTime).OnComplete(() =>
               {
                   base.Destroy();
                   main.transform.Normalization(transform);
               });
    }


    #region Temporary

    private void OnMouseUpAsButton()
    {
        //if (TryGetComponent<Rigidbody2D>( out Rigidbody2D rigi2D))
        //{

        //}
        return;
    }

    #endregion
}
