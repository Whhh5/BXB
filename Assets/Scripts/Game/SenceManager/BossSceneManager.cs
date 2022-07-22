using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;

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



    //map-
    [SerializeField] MapWapController mapWapController => SceneDataManager.Instance.mapWapController;
    [SerializeField] int wapUnit => SceneDataManager.Instance.wapUnit;


    [SerializeField] Dictionary<Vector2, Wap> pointToWap => SceneDataManager.Instance.pointToWap;
    protected override void OnAwake()
    {
        base.OnAwake();
        SceneDataManager.Instance.pointToWap.Clear();
        mapWapController.CreateMapWap(wapUnit, mapWidthAndHeight, pointToWap, wapParent);
    }

    protected override void OnStart()
    {
        base.OnStart();
        var player = (CharacterController)SceneDataManager.Instance.data;
        mapWapController.PlaceArticle(player, new Vector2(5, 5), pointToWap, 0.0f, DG.Tweening.Ease.Linear);



    }
}