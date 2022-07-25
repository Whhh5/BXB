using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using System;
using DG.Tweening;
using UnityEngine.SceneManagement;
//using UnityEditor;

public class SceneDataManager : MiSingleton<SceneDataManager>
{
    public object data;

    public CharacterController mainPlayer;

    public float playerMoveInterval = 0.3f;

    public List<Wap> lastDetectionWaps = new List<Wap>();

    public MapWapController mapWapController = new MapWapController();

    public int wapUnit = 1;

    public Dictionary<Vector2, Wap> pointToWap = new Dictionary<Vector2, Wap>();

    public Wap mouseDownWap;

    public UIDialog_Battle_MainConsole mainConsole;

    public SceneMode sceneMode = SceneMode.Play;

    public List<WapObjBase> enemys = new List<WapObjBase>();

    public Camera sceneMainCamera;

    public Action gameFinish = () => { };

    public float schedule = 0.0f;

    public float nextSceneSchedule = 0.0f;

    public float allSchedule = 0.0f;

    public Asset_SceneLevelData levelData;

    public float upLevelExp;

    public float nowExp;

    public enum MoveMode
    {
        None,
        Left,
        Right,
        Top,
        Down,
        EnumCount,
    }
    public enum SceneMode
    {
        None = 1,
        Play = 1 << 1,
        Acttack = 1 << 2,
        EnumCount = 1 << 3,
    }
    public enum ItemsType
    {
        None,
        Gold,
        EnumCount,
    }
    Dictionary<MoveMode, bool> dirctionIsMove = new Dictionary<MoveMode, bool>
    {
        { MoveMode.None, false},
        { MoveMode.Left, false},
        { MoveMode.Right, false},
        { MoveMode.Top, false},
        { MoveMode.Down, false},
    };
    Dictionary<MoveMode, Vector2> dirctionPoint = new Dictionary<MoveMode, Vector2>
    {
        { MoveMode.Left, new Vector2(0,-1)},
        { MoveMode.Right, new Vector2(0,1)},
        { MoveMode.Top, new Vector2(-1,0)},
        { MoveMode.Down, new Vector2(1,0)},
    };
    public void CreateLevelSceneData(short level)
    {
        var assetPath = "DataAsset/Asset_Scene_Level_" + level;
        Asset_SceneLevelData asset = Resources.Load<Asset_SceneLevelData>(assetPath);
        levelData = asset;
    }
    public void AddExp(float exp)
    {
        nowExp += exp;
        if (nowExp >= upLevelExp)
        {
            mainPlayer.GetSetLevel(1);
            upLevelExp = MasterData.Instance.GetTableData<LocalRolesLevelData>( (ulong)mainPlayer.GetSetLevel()).level;
            nowExp -= upLevelExp;
            mainPlayer.UpDateLevelData();
        }
    }
    public Asset_SceneLevelData GetLevelSceneData()
    {
        Asset_SceneLevelData ret = null;
        ret = levelData;
        return ret;
    }
    public void Removeenemys()
    {
        foreach (var item in enemys)
        {
            item?.gameObject.SetActive(false);
        }
        enemys.Clear();
    }
    public void RemoveAllGameObject()
    {
        Removeenemys();
        if (mainPlayer!=null)
        {
            foreach (var item in mainPlayer.GetSetLegion())
            {
                item?.gameObject.SetActive(false);
            }
            mainPlayer?.gameObject.SetActive(false);

        }
    }
    public void InitLevelData(float nextSchedule)
    {
        schedule = 0.0f;
        this.nextSceneSchedule = nextSchedule;
        allSchedule = schedule / nextSceneSchedule;
    }
    public float GetSetLevelSchedule(float increment)
    {
        schedule += increment;
        schedule = schedule > nextSceneSchedule ? nextSceneSchedule : schedule;
        allSchedule = schedule / nextSceneSchedule;
        if (allSchedule >= 1)
        {
            GameFinish();
        }
        Log(Color.green, allSchedule);
        return allSchedule;
    }

    public T GetGameObject<T>(ulong id, Vector2 startWap, WapObjBase.StatusMode mode, bool isEnemy = true) where T : WapObjBase
    {
        T obj = null;
        try
        {
            var table = MasterData.Instance.GetTableData<LocalRolesData>(id);

            var downImageIndex = table.downImage;
            //var downPath = CommonManager.Instance.filePath.ResImSpDownImage;
            //var downImageSprite = ResourceManager.Instance.Load<Sprite>(downPath, downImageIndex.ToString());
            var path = CommonManager.Instance.filePath.ResRolesPrePath;
            obj = ResourceManager.Instance.GetWorldObject<T>(path, id.ToString(), Vector3.zero, null, id);
            obj.SetMoveMode(mode);
            mapWapController.PlaceArticle(obj, startWap, pointToWap, 0, Ease.Linear);
            if (isEnemy)
            {
                enemys.Add(obj);
            }
        }
        catch (Exception exp)
        {
            Log(Color.red, exp);
        }
        return obj;
    }

    public Asset_SceneLevelData LoadSceneAsset(string name)
    {
        var assetPath = $"DataAsset/Asset_Scene_{name}.asset"; ;
        var asset = Resources.Load<Asset_SceneLevelData>(assetPath);
        levelData = asset;
        return levelData;
    }
    public void AddGameFinishAction(Action action)
    {
        gameFinish += action;
    }

    public void SetMouseWap(Wap wap)
    {
        mouseDownWap = wap;
    }
    public bool TryGetWap(Vector2 point, out Wap wap)
    {
        bool extend;
        try
        {
            wap = pointToWap[point];
            extend = true;
        }
        catch (Exception)
        {
            wap = null;
            extend = false;
        }
        return extend;
    }
    public async void ShowBattleMainConsole()
    {
        mainConsole?.Destroy();
        var path = CommonManager.Instance.filePath.PreUIDialogSystemPath;
        mainConsole = await ResourceManager.Instance.ShowDialogAsync<UIDialog_Battle_MainConsole>(path, "UIDialog_Battle_MainConsole", CanvasLayer.System);
    }

    public void MoveToDirection(WapObjBase obj, MoveMode mode)
    {
        if (mode == MoveMode.None || mode == MoveMode.EnumCount || (sceneMode & SceneMode.Play) == 0) return;

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

        List<WapObjBase> objs = new List<WapObjBase>(obj.GetSetLegion())
        {
            obj
        };
        foreach (var item in objs)
        {
            var listPoint = item.GetAllPoint();
            foreach (var parameter in listPoint)
            {
                var pos = parameter + new Vector2(x, y);
                if (!SceneDataManager.Instance.TryGetWap(pos, out Wap wap2) || (wap2.GetLayerMask() & mainPlayer.playerDetectionLayer) != 0)
                {
                    return;
                }
            }
        }
        obj.SetStatus(WapObjBase.Status.Walk);

        //foreach (var item in obj.GetSetLegion())
        //{
        //    pointToWap[item.GetPoint()] = null;
        //}
        //移动玩家自己
        var oldPoint = obj.GetPoint();
        var newPoint = oldPoint + new Vector2(x, y);
        //pointToWap[oldPoint].SetArticle(null);
        MoveToVector2(obj, newPoint, playerMoveInterval);

        //移动军团
        foreach (var item in obj.GetSetLegion())
        {
            var point = obj.GetPoint();
            point += item.GetOffset();
            MoveToVector2(item, point, playerMoveInterval);
        }

        DetectionWap();

        DetectionEnemy();
    }
    public void MoveToVector2(WapObjBase obj, Vector2 point, float moveTime = 1.0f, Ease ease = Ease.Linear)
    {
        mapWapController.PlaceArticle(obj, point, pointToWap, moveTime, ease);
    }


    public void DetectionEnemy()
    {
        List<WapObjBase> targets = new List<WapObjBase>();
        foreach (var wap in lastDetectionWaps)
        {
            if ((wap.GetLayerMask() & mainPlayer.GetSetAttackLayer()) != 0 && wap.TryGetObject(out WapObjBase wapObj))
            {
                if (!targets.Contains(wapObj))
                {
                    targets.Add(wapObj);
                }
            }
        }
        if (targets.Count >=1)
        {
            var t = targets[0];
            var list = new List<WapObjBase>();
            list.Add(t);
            mainConsole.ShowEnemyList(list).Wait();
        }
        
    }

    private void DetectionWap()
    {
        //计算周围检测
        //init
        foreach (var item in lastDetectionWaps)
        {
            item?.SetMouseWap(0, 1, Color.green);
        }
        lastDetectionWaps.Clear();

        TryGetRound(mainPlayer, mainPlayer.playerNoDetectionLayer, ref SceneDataManager.Instance.lastDetectionWaps);
        foreach (var player in mainPlayer.GetSetLegion())
        {
            //init
            TryGetRound(player, mainPlayer.playerNoDetectionLayer, ref SceneDataManager.Instance.lastDetectionWaps);
        }
        //--

        //区域变色
        foreach (var item in lastDetectionWaps)
        {
            Vector2 endActive = new Vector2(0.2f, 0.5f);
            var l = item.GetLayerMask();
            //if ((l & playerDetectionLayer) != 0)
            //{
            //    endActive = new Vector2(0, 1);
            //}
            item.SetMouseWap(endActive.x, endActive.y, Color.green);
        }
    }
    //根据层级检测四方 方块
    public void TryGetRound(WapObjBase obj, LayerMask noCastlayer, ref List<Wap> wap)
    {
        var allPoints = obj.GetAllPoint();
        foreach (var point in allPoints)
        {
            for (int i = 1; i < (int)MoveMode.EnumCount; i++)
            {
                if (dirctionPoint.TryGetValue((MoveMode)i, out Vector2 points))
                {
                    points += point;
                    if (SceneDataManager.Instance.TryGetWap(points, out Wap wap2))
                    {
                        var layer = wap2.GetLayerMask();
                        if ((layer & noCastlayer) == 0)
                        {
                            if (!wap.Contains(wap2))
                            {
                                wap.Add(wap2);
                            }
                        }
                    }
                }
            }
        }
    }

    public void Addleglon(WapObjBase obj1, WapObjBase obj2)
    {
        obj1.GetSetLegion(obj2);
        var offset = mainPlayer.GetPoint() - obj1.GetPoint();
        obj1.SetMoveMode(WapObjBase.StatusMode.Trusteeship, mainPlayer, offset);
    }


    public void RemoveEnemyObj(WapObjBase enemy)
    {
        var point = enemy.GetPoint();
        enemy.Destroy();
        pointToWap[point].SetArticle(null);
    }
    public void SetMapPoint(Vector2 point, GameObject obj)
    {
        pointToWap[point].SetArticle(obj);
    }

    public void ActiveBattle(WapObjBase obj1, WapObjBase obj2)
    {
        SetSceneMode(SceneMode.Acttack);

        List<WapObjBase> obj1_legion = new List<WapObjBase>(obj1.GetSetLegion());
        obj1_legion.Add(obj1);
        List<WapObjBase> obj2_legion = new List<WapObjBase>(obj2.GetSetLegion());
        obj2_legion.Add(obj2);
        obj1.Action(obj2, obj2_legion);
        foreach (var item in obj1.GetSetLegion())
        {
            item.Action(obj2, obj2_legion);
        }
        obj2.Action(obj1, obj1_legion);
        foreach (var item in obj2.GetSetLegion())
        {
            item.Action(obj1, obj1_legion);
        }
    }

    public void SetSceneMode(SceneMode sceneMode)
    {
        this.sceneMode = sceneMode;
        switch (sceneMode)
        {
            case SceneMode.None:
                break;
            case SceneMode.Play:
                break;
            case SceneMode.Acttack:
                break;
            case SceneMode.EnumCount:
                break;
            default:
                break;
        }
    }

    public void AddPlayerArticle(ulong id, int number)
    {
        //增加物品
        mainPlayer.GetSetArticle(id, number);
    }
    public void AddPlayerProperty(ulong id, float number)
    {
        //增加属性值
        mainPlayer.GetSet((WapObjBase.PropertyFloat)id, number);
    }
    public void AddConsumable(ulong id, int number)
    {
        //增加消耗品
        mainPlayer.GetSetConsumable(id, number);
    }

    public void GameFinish()
    {
        gameFinish?.Invoke();
        gameFinish = null;
    }

    public void RecruitLegion(WapObjBase obj1, WapObjBase obj2)
    {
        var demandItems = obj2.GetSet(WapObjBase.PropertyListString.recruitDemandArticle);
        List<string> items = new List<string>();
        bool isRecuit = true;
        string hintStr;
        foreach (var item in demandItems)
        {
            var itemAndNumber = item.Split(':');// id:number 1：10
            var id = ulong.Parse(itemAndNumber[0]);
            var number = int.Parse(itemAndNumber[1]);
            items.Add($"{id}:{-number}");
            if (!obj1.TryArticleCount(id, number))
            {
                isRecuit = false;
            }
        }
        if (isRecuit)
        {
            var list = obj2.GetSet(WapObjBase.PropertyListString.recruitGetArticle);
            Dictionary<ulong, float> testDic = new Dictionary<ulong, float>();
            foreach (var item in items)
            {
                var str = item.Split(':');
                var id = ulong.Parse(str[0]);
                var number = float.Parse(str[1]);
                testDic.Add(id, number);
            }
            foreach (var item in list)
            {
                var str = item.Split(':');
                var id = ulong.Parse(str[0]);
                var number = float.Parse(str[1]);
                if (testDic.ContainsKey(id))
                {
                    testDic[id] += number;
                }
                else
                {
                    testDic.Add(id, number);
                }
            }
            list = new List<string>();
            foreach (var item in testDic)
            {
                list.Add($"{item.Key}:{item.Value}");
            }
            AddProperty(list);
            var offset = obj2.GetPoint() - obj1.GetPoint();
            obj2.SetMoveMode(WapObjBase.StatusMode.Trusteeship, obj1, offset);
            obj1.GetSetLegion(obj2);
            obj2.SetLayer(obj1.gameObject.layer);
            obj2.GetSetAttackLayer(obj1.GetSetAttackLayer());
            hintStr = $"Recuit -{obj2.GetName()}- finish ...";
            if (enemys.Contains(obj2))
            {
                enemys.Remove(obj2);
            }
            obj2.Change();





        }
        else
        {
            hintStr = $"Recuit -{obj2.GetName()}- Scant supply of material ! ";
        }
        mainConsole.HintInformation(hintStr).Wait();
    }

    public void AddProperty(List<string> property)
    {
        Dictionary<ulong, int> articleList = new Dictionary<ulong, int>();
        Dictionary<ulong, int> consumablesList = new Dictionary<ulong, int>();
        Dictionary<ulong, float> propertyList = new Dictionary<ulong, float>();
        foreach (var item in property)
        {
            var article = item.Split(':');
            var id = ulong.Parse(article[0]);
            var number = float.Parse(article[1]);
            switch (MasterData.Instance.GetTableData<LocalItemData>(id).type)
            {
                case 1:
                    propertyList.Add(id, number);
                    break;
                case 2:
                    articleList.Add(id, (int)number);
                    break;
                case 3:
                    consumablesList.Add(id, (int)number);
                    break;
                default:
                    break;
            }
        }
        foreach (var item in articleList)
        {
            //增加物品
            AddPlayerArticle(item.Key, item.Value);
        }
        foreach (var item in consumablesList)
        {
            //增加消耗品
            AddConsumable(item.Key, item.Value);
        }
        foreach (var item in propertyList)
        {
            //增加属性值
            AddPlayerProperty(item.Key, item.Value);
        }
        mainConsole.UpdatePlayerProperty().Wait();
    }
}
