using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Threading.Tasks;
using BXB.Core;

public class UICameraTools : Editor
{
    [MenuItem("GameObject/Create UI/Slider",false,0)]
    public static async Task CreateSlider()
    {
        await ResourceManager.Instance.LoadAsync<GameObject>("Prefab/Editor/UI", "Slider", true);
    }
}
