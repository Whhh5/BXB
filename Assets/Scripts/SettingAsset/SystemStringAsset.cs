using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;

[CreateAssetMenu(fileName = "new System String Asset", menuName = "System/SystemStringAsset")]
public class SystemStringAsset : ScriptableObject
{
    [SerializeField, ReadOnly] private string _unityLocalPath;







    public string unityLocalPath { get => _unityLocalPath;}

    private void OnEnable()
    {
        _unityLocalPath = Application.dataPath;

    }
}