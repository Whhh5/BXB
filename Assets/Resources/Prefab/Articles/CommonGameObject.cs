using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using System;

public class CommonGameObject : MiObjPoolPublicParameter, ICommon_Weapon
{
    [SerializeField] GameObject main;
    [SerializeField] Sprite downImage;
    [SerializeField] Sprite mainSprite;
    [SerializeField] Animator animator;

    public void Action(params object[] value)
    {
        
    }

    public object Clone()
    {
        return MemberwiseClone();
    }

    public GameObject GetMain()
    {
        return main;
    }

    public void OnInit()
    {
        
    }

    public void OnSetInit(object[] value)
    {
        var downImage = (Sprite)value[0];
        var mainSprite = (Sprite)value[1];
        //var animator = ()

    }
}
