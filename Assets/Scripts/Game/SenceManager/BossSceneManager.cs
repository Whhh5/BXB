using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using System;
using UnityEngine.SceneManagement;

public class BossSceneManager : MiSingletonMonoBeHaviour<BossSceneManager>
{
    [SerializeField] Vector2 mapWidthAndHeight;
    [SerializeField] Transform wapParent;
    public GameObject level_200100001;
    public GameObject level_200100002;
    public GameObject level_200100003;

    //plsyer
    public CharacterController mainPlayer => SceneDataManager.Instance.mainPlayer;
    public float playerMoveInterval => SceneDataManager.Instance.playerMoveInterval;
    //public List<WapObjBase> legionPoint = new List<WapObjBase>();
    public List<Wap> lastDetectionWaps => SceneDataManager.Instance.lastDetectionWaps;
    //player


    public Camera sceneMainCamera;


    //map-
    [SerializeField] MapWapController mapWapController => SceneDataManager.Instance.mapWapController;
    [SerializeField] int wapUnit => SceneDataManager.Instance.wapUnit;


    [SerializeField] Dictionary<Vector2, Wap> pointToWap => SceneDataManager.Instance.pointToWap;
    protected override void OnAwake()
    {
        base.OnAwake();
        SceneDataManager.Instance.pointToWap.Clear();
        mapWapController.CreateMapWap(wapUnit, mapWidthAndHeight, pointToWap, wapParent);
        SceneDataManager.Instance.sceneMainCamera = sceneMainCamera;
        SceneDataManager.Instance.AddGameFinishAction(() => { Finish(); });
    }

    protected override void OnStart()
    {
        base.OnStart();
        var player = (CharacterController)SceneDataManager.Instance.data;
        mapWapController.PlaceArticle(player, new Vector2(5, 5), pointToWap, 0.0f, DG.Tweening.Ease.Linear);


        var levelData = SceneDataManager.Instance.GetLevelSceneData();


        //var path = CommonManager.Instance.filePath.ResPreMap;
        //var spriteMap = Resources.Load<GameObject>(path + levelData.data_Scene_Boss);
        //var o = GameObject.Instantiate(spriteMap,transform);
        //o.transform.position = Vector3.zero;
        level_200100001?.SetActive(false);
        level_200100002?.SetActive(false);
        level_200100003?.SetActive(false);
        var fiekd = $"level_{levelData.mapId_Boss}";
        var type = GetType().GetField(fiekd).GetValue(BossSceneManager.Instance);
        type.GetType().GetMethod("SetActive").Invoke(type, new object[] { true});


        SceneDataManager.Instance.InitLevelData(levelData.overAllProgram);
        foreach (var item in levelData.data_Scene_Boss)
        {
            for (int i = 0; i < item.number; i++)
            {
                var posX = (int)UnityEngine.Random.Range(item.scope_xxYY.x, item.scope_xxYY.y);
                var posY = (int)UnityEngine.Random.Range(item.scope_xxYY.z, item.scope_xxYY.w);
                var enemy = SceneDataManager.Instance.GetGameObject<TestEnemy1>(item.id, new Vector2(posX, posY), WapObjBase.StatusMode.Trusteeship);
            }
        }
        SoundManager.instance.EntryBossEnviroment();
    }
    async void Finish()
    {
        Action action = () =>
        {
            Log(Color.red, "Finish");
            SceneDataManager.Instance.Removeenemys();
            ResourceManager.Instance.RemoveSceneAsync(ResourceManager.SceneMode.Boss, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            SoundManager.instance.StopBossBGM();
            ResourceManager.Instance.LoadSceneAsync(ResourceManager.SceneMode.LevelSelect, LoadSceneMode.Additive);
        };
        var path = CommonManager.Instance.filePath.PreUIDialogSystemPath;
        await ResourceManager.Instance.ShowDialogAsync<MiUIDialog>(path, "UIDialog_TextPopup", CanvasLayer.System, SceneDataManager.Instance.levelData.endStory, action);
    }
}