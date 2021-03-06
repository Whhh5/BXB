using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Level Data", menuName = "Create Level Data")]
public class Asset_SceneLevelData : ScriptableObject
{
    public enum Controllers
    {
        None,

        CharacterController,

        TestEnemy1,

        EnumCount,
    }
    [Serializable]
    public struct LevelDataStruct
    {
        public ulong id;
        public int number;
        public int downImageId;
        public Vector4 scope_xxYY;
        public Controllers controller;
    }
    public List<LevelDataStruct> data_Scene_Battle;
    public List<LevelDataStruct> data_Scene_Boss;

    public ulong mapId_Battle;
    public ulong mapId_Boss;
    public float overAllProgram;
    [TextArea]
    public string startStory;
    [TextArea]
    public string endStory;
}
