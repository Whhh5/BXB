using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;

public abstract class WordPoolBase : MiObjPoolPublicParameter, IWordGameObject
{
    [SerializeField] GameObject main;
    public object Clone()
    {
        return MemberwiseClone();
    }

    public GameObject GetMain()
    {
        return main;
    }

    public abstract void OnInit();

    public abstract void OnSetInit(object[] value);
}
