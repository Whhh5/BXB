using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using System;

public class CommonGameObject : MiObjPoolPublicParameter, ICommon_Weapon
{
    [SerializeField] GameObject main;
    [SerializeField] SpriteRenderer downImage;
    [SerializeField] SpriteRenderer mainSprite;
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
        downImage.sprite = (Sprite)value[0];
        mainSprite.sprite = (Sprite)value[1];

        var animaControllerName = (string)value[2];
        var path = CommonManager.Instance.filePath.ResAnima;
        var animatorController = ResourceManager.Instance.Load<RuntimeAnimatorController>(path, animaControllerName);
        animator.runtimeAnimatorController = animatorController;
    }
}
