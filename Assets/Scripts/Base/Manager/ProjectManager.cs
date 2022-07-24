using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BXB.Core;
using UnityEditor;

public class ProjectManager : MiSingleton<ProjectManager>
{
    static string assResSetting = "Assets/Resources/SettingAsset/";
    public enum AssetTypes
    {
        SystemStringAsset,
        ComputerSettingAsset,
    }
    Dictionary<AssetTypes, string> assetPath = new Dictionary<AssetTypes, string>
    {
        {AssetTypes.SystemStringAsset, $"{assResSetting}"},
        {AssetTypes.ComputerSettingAsset, $"{assResSetting}"},
    };
    public bool TryGetSettingAssets<T>(AssetTypes type, out T asset) where T : ScriptableObject
    {
        asset = null;
        bool isGet = false;
        try
        {
            var path = assetPath[type] + $"{type}.asset";
            asset = AssetDatabase.LoadAssetAtPath<T>(path);
        }
        catch (Exception exp)
        {
#if PC_GAME

#elif MOBILE_GAME

#endif


        }
        return isGet;
    }
}
