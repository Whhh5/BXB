using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;
using System.Threading.Tasks;
using UnityEditor;

public class BattleSceneManager : MiSingletonMonoBeHaviour<BattleSceneManager>
{
    //plsyer
    public CharacterController mainPlayer => SceneDataManager.Instance.mainPlayer;
    public float playerMoveInterval => SceneDataManager.Instance.playerMoveInterval;
    //public List<WapObjBase> legionPoint = new List<WapObjBase>();
    public List<Wap> lastDetectionWaps => SceneDataManager.Instance.lastDetectionWaps;
    public LayerMask playerNoDetectionLayer;
    public LayerMask playerDetectionLayer;
    public GameObject level_200000001;
    public GameObject level_200000002;
    public GameObject level_200000003;
    //player

    //camera
    public Camera sceneMainCamera;
    //camera

    //mouse
    public Wap mouseDownWap => SceneDataManager.Instance.mouseDownWap;
    //mouse

    //Console
    public UIDialog_Battle_MainConsole mainConsole => SceneDataManager.Instance.mainConsole;
    //Console

    //map-
    [SerializeField] MapWapController mapWapController => SceneDataManager.Instance.mapWapController;
    [SerializeField] int wapUnit => SceneDataManager.Instance.wapUnit;

    [SerializeField] Vector2 mapWidthAndHeight = new Vector2(10, 10);
    [SerializeField] Transform wapParent;
    [SerializeField] Dictionary<Vector2, Wap> pointToWap => SceneDataManager.Instance.pointToWap;
    //map-
    protected override async Task OnAwakeAsync()
    {
        try
        {

            var path = CommonManager.Instance.filePath.PreUIDialogSystemPath;
            SceneDataManager.Instance.ShowBattleMainConsole();
            var menu = await ResourceManager.Instance.ShowDialogAsync<UIDialog_BattleSettingMenu>(path, "UIDialog_BattleSettingMenu", CanvasLayer.System);
            SceneDataManager.Instance.AddGameFinishAction(() =>
            {
                Finish();
            });
        }
        catch (Exception)
        {
            //Log(Color.red, exp);
        }
    }
    protected override void OnAwake()
    {
        base.OnAwake();
        pointToWap.Clear();
        //var 
        level_200000001?.SetActive(false);
        level_200000002?.SetActive(false);
        level_200000003?.SetActive(false);
        mapWapController.CreateMapWap(wapUnit, mapWidthAndHeight, pointToWap, wapParent);
        SceneDataManager.Instance.sceneMainCamera = sceneMainCamera;
    }

    protected override async Task OnStartAsync()
    {
        await base.OnStartAsync();
    }

    protected override void OnStart()
    {
        base.OnStart();


        var levelData = SceneDataManager.Instance.GetLevelSceneData();
        //var path = CommonManager.Instance.filePath.ResPreMap;
        //var spriteMap = Resources.Load<GameObject>(path + levelData.mapId_Battle);
        //var o = GameObject.Instantiate(spriteMap, transform);
        //o.transform.position = Vector3.zero;
        SceneDataManager.Instance.InitLevelData(levelData.overAllProgram);

        level_200000001?.SetActive(false);
        level_200000002?.SetActive(false);
        level_200000003?.SetActive(false);
        var fiekd = $"level_{levelData.mapId_Battle}";
        var type = GetType().GetField(fiekd).GetValue(Instance);
        type.GetType().GetMethod("SetActive").Invoke(type, new object[] { true });

        foreach (var item in levelData.data_Scene_Battle)
        {
            for (int i = 0; i < item.number; i++)
            {
                var posX = (int)UnityEngine.Random.Range(item.scope_xxYY.x, item.scope_xxYY.y);
                var posY = (int)UnityEngine.Random.Range(item.scope_xxYY.z, item.scope_xxYY.w);
                var enemy = SceneDataManager.Instance.GetGameObject<TestEnemy1>(item.id, new Vector2(posX, posY), WapObjBase.StatusMode.Trusteeship);
            }
        }


        var charObj = SceneDataManager.Instance.GetGameObject<CharacterController>(110000001, new Vector2(2, 2), WapObjBase.StatusMode.Manual, false);

        SceneDataManager.Instance.mainPlayer = charObj;

        //SceneDataManager.Instance.GetGameObject<TestEnemy1>(110020001, new Vector2(3, 3), WapObjBase.StatusMode.Trusteeship);

        //SceneDataManager.Instance.GetGameObject<TestEnemy1>(110020001, new Vector2(5, 5), WapObjBase.StatusMode.Trusteeship);

        SceneDataManager.Instance.RangArticle(100, 150);


    }

    public void Finish()
    {
        Log(Color.red, "Finish");

        var legion = mainPlayer.GetSetLegion();
        foreach (var item in legion)
        {
            item.SetPoint(new Vector2(2, 2));
        }

        SceneDataManager.Instance.Removeenemys();
        ResourceManager.Instance.RemoveSceneAsync( ResourceManager.SceneMode.Battle, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        ResourceManager.Instance.LoadSceneAsync( ResourceManager.SceneMode.Boss, LoadSceneMode.Additive);
    }
}
