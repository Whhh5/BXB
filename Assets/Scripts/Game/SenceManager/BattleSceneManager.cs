using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;
using System.Threading.Tasks;

public class BattleSceneManager : MiSingletonMonoBeHaviour<BattleSceneManager>
{
    public enum MoveMode
    {
        None,
        Left,
        Right,
        Top,
        Down,
        EnumCount,
    }
    //plsyer
    public CharacterController mainPlayer;
    public float playerMoveInterval = 0.0f;
    public List<WapObjBase> leglon = new List<WapObjBase>();
    //player

    //camera
    public Camera sceneMainCamera;
    //camera

    //mouse
    public Wap mouseDownWap;
    //mouse

    //map-
    [SerializeField] MapWapController mapWapController = new MapWapController();
    [SerializeField] int wapUnit = 1;
    [SerializeField] Vector2 mapWidthAndHeight = new Vector2(10, 10);
    [SerializeField] Transform wapParent;

    [SerializeField] Dictionary<Vector2, Wap> pointToWap = new Dictionary<Vector2, Wap>();
    //map-




    protected override async Task OnAwakeAsync()
    {
        try
        {
            var path = CommonManager.Instance.filePath.PreUIDialogSystemPath;
            var mainConsole = await ResourceManager.Instance.ShowDialogAsync<UIDialog_Battle_MainConsole>(path, "UIDialog_Battle_MainConsole", CanvasLayer.System);
            var menu = await ResourceManager.Instance.ShowDialogAsync<UIDialog_BattleSettingMenu>(path, "UIDialog_BattleSettingMenu", CanvasLayer.System);
        }
        catch (Exception exp)
        {
            //Log(Color.red, exp);
        }
    }
    protected override void OnAwake()
    {
        base.OnAwake();
        mapWapController.CreateMapWap(wapUnit, mapWidthAndHeight, pointToWap, wapParent);
    }

    protected override async Task OnStartAsync()
    {
        await base.OnStartAsync();
    }

    protected override void OnStart()
    {
        base.OnStart();
        var charPath = CommonManager.Instance.filePath.ResArticle;
        var charObj = ResourceManager.Instance.GetWorldObject<CharacterController>(charPath, "Player", new Vector3(0, 0, 0), null, f_id:110000001);        
        mainPlayer = charObj;
        mapWapController.PlaceArticle(charObj, new Vector2(0, 0), pointToWap, 0, Ease.Linear);
    }

    public void SetWap(WapObjBase obj, MoveMode mode)
    {
        if (mode == MoveMode.None || mode == MoveMode.EnumCount) return;

        var oldPoint = obj.GetPoint();
        int x = 0, y = 0;
        switch (mode)
        {
            case MoveMode.None:
                break;
            case MoveMode.Left:
                y = -1;
                break;
            case MoveMode.Right:
                y = 1;
                break;
            case MoveMode.Top:
                x = -1;
                break;
            case MoveMode.Down:
                x = 1;
                break;
            case MoveMode.EnumCount:
                break;
            default:
                break;
        }
        var newPoint = oldPoint + new Vector2(x, y);

        try
        {
            pointToWap[newPoint].TryGetObject<WapObjBase>(out WapObjBase testWap);
        }
        catch (Exception exp)
        {
            return;
        }
        //if (newPoint.x < 0 || newPoint.y < 0 || newPoint.x >= mapWidthAndHeight.y || newPoint.y >= mapWidthAndHeight.x || pointToWap[newPoint].TryGetObject<WapObjBase>(out WapObjBase wap))
        //{
        //    return;
        //}
        mapWapController.PlaceArticle(obj, newPoint, pointToWap, playerMoveInterval, Ease.Linear);
    }

    public void SetMouseWap(Wap wap)
    {
        this.mouseDownWap = wap;
    }
}
