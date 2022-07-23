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

        SceneDataManager.Instance.GetGameObject<TestEnemy1>(110020001, new Vector2(4, 16), WapObjBase.StatusMode.Trusteeship);

    }
    async void Finish()
    {
        Action action = () =>
        {
            Log(Color.red, "Finish");
            var enemys = SceneDataManager.Instance.enemys;
            foreach (var item in enemys)
            {
                item.Destroy();
            }
            enemys.Clear();
            ResourceManager.Instance.RemoveSceneAsync(ResourceManager.SceneMode.Boss, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            ResourceManager.Instance.LoadSceneAsync(ResourceManager.SceneMode.LevelSelect, LoadSceneMode.Additive);
        };
        var path = CommonManager.Instance.filePath.PreUIDialogSystemPath;
        await ResourceManager.Instance.ShowDialogAsync<MiUIDialog>(path, "UIDialog_TextPopup", CanvasLayer.System, "" +
            "ashiduhaosfapfh[asfoaishfpahspojhjapfas" +
            "ashf", action);
    }
}