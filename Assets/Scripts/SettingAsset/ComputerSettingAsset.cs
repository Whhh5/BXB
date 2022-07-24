using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Computer Asset", menuName = "System/ComputerSettingAsset")]
public class ComputerSettingAsset : ScriptableObject
{
    public string ip;
    public int port;
}
