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
    //public enum MoveMode
    //{
    //    None,
    //    Left,
    //    Right,
    //    Top,
    //    Down,
    //    EnumCount,
    //}
    //public enum SceneMode
    //{
    //    None = 1,
    //    Play = 1 << 1,
    //    Acttack = 1 << 2,
    //    EnumCount = 1 << 3,
    //}
    //public enum ItemsType
    //{
    //    None,
    //    Gold,
    //    EnumCount,
    //}
    //Dictionary<MoveMode, bool> dirctionIsMove = new Dictionary<MoveMode, bool>
    //{
    //    { MoveMode.None, false},
    //    { MoveMode.Left, false},
    //    { MoveMode.Right, false},
    //    { MoveMode.Top, false},
    //    { MoveMode.Down, false},
    //};
    //Dictionary<MoveMode, Vector2> dirctionPoint = new Dictionary<MoveMode, Vector2>
    //{
    //    { MoveMode.Left, new Vector2(0,-1)},
    //    { MoveMode.Right, new Vector2(0,1)},
    //    { MoveMode.Top, new Vector2(-1,0)},
    //    { MoveMode.Down, new Vector2(1,0)},
    //};
    ////Scene
    //public SceneMode sceneMode = SceneMode.Play;

    //plsyer
    public CharacterController mainPlayer => SceneDataManager.Instance.mainPlayer;
    public float playerMoveInterval => SceneDataManager.Instance.playerMoveInterval;
    //public List<WapObjBase> legionPoint = new List<WapObjBase>();
    public List<Wap> lastDetectionWaps => SceneDataManager.Instance.lastDetectionWaps;
    public LayerMask playerNoDetectionLayer;
    public LayerMask playerDetectionLayer;
    //player

    //Enemy
    public List<WapObjBase> enemys = new List<WapObjBase>();
    //Enemy

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
        var charObj = ResourceManager.Instance.GetWorldObject<CharacterController>(charPath, "Player", new Vector3(0, 0, 0), null, f_id: 110000001);
        charObj.SetMoveMode(WapObjBase.StatusMode.Manual);
        //charObj.name = "Player";
        //var legion1 = ResourceManager.Instance.GetWorldObject<CharacterController>(charPath, "Player", new Vector3(0, 0, 0), null, f_id: 110000001);
        //legion1.SetMoveMode(WapObjBase.StatusMode.Trusteeship, charObj, new Vector2(1, 0));
        //charObj.GetSetLegion(legion1);

        SceneDataManager.Instance.mainPlayer = charObj;
        mapWapController.PlaceArticle(charObj, new Vector2(2, 2), pointToWap, 0, Ease.Linear);


        var enemyPath = ResourceManager.Instance.GetWorldObject<WapObjBase>(charPath, "TestEnemy1", new Vector3(0, 0, 0), null, f_id: 110020001);
        mapWapController.PlaceArticle(enemyPath, new Vector2(3, 3), pointToWap, 0, Ease.Linear);

        var enemyPath2 = ResourceManager.Instance.GetWorldObject<WapObjBase>(charPath, "TestEnemy1", new Vector3(0, 0, 0), null, f_id: 110020001);
        mapWapController.PlaceArticle(enemyPath2, new Vector2(4, 5), pointToWap, 0, Ease.Linear);

        mainPlayer.GetSetArticle(120050001, 66);
        mainConsole.UpdatePlayerProperty().Wait();

        mainConsole.UpdatePlayerProperty().Wait();
        SceneDataManager.Instance.enemys.Add(enemyPath);
        SceneDataManager.Instance.enemys.Add(enemyPath2);

    }

    ////public void CreateEnemys(List<Vector4> )
    ////{

    ////}

    //public void MoveToDirection(WapObjBase obj, MoveMode mode)
    //{
    //    if (mode == MoveMode.None || mode == MoveMode.EnumCount || (sceneMode & SceneMode.Play) == 0) return;

    //    int x = 0, y = 0;
    //    switch (mode)
    //    {
    //        case MoveMode.None:
    //            break;
    //        case MoveMode.Left:
    //            y = -1;
    //            break;
    //        case MoveMode.Right:
    //            y = 1;
    //            break;
    //        case MoveMode.Top:
    //            x = -1;
    //            break;
    //        case MoveMode.Down:
    //            x = 1;
    //            break;
    //        case MoveMode.EnumCount:
    //            break;
    //        default:
    //            break;
    //    }

    //    List<WapObjBase> objs = new List<WapObjBase>(obj.GetSetLegion())
    //    {
    //        obj
    //    };
    //    foreach (var item in objs)
    //    {
    //        var listPoint = item.GetAllPoint();
    //        foreach (var parameter in listPoint)
    //        {
    //            var pos = parameter + new Vector2(x, y);
    //            if (!SceneDataManager.Instance.TryGetWap(pos, out Wap wap2) || (wap2.GetLayerMask() & playerDetectionLayer) != 0)
    //            {
    //                return;
    //            }
    //        }
    //    }
    //    obj.SetStatus(WapObjBase.Status.Walk);

    //    //foreach (var item in obj.GetSetLegion())
    //    //{
    //    //    pointToWap[item.GetPoint()] = null;
    //    //}
    //    //移动玩家自己
    //    var oldPoint = obj.GetPoint();
    //    var newPoint = oldPoint + new Vector2(x, y);
    //    //pointToWap[oldPoint].SetArticle(null);
    //    MoveToVector2(obj, newPoint, playerMoveInterval);

    //    //移动军团
    //    foreach (var item in obj.GetSetLegion())
    //    {
    //        var point = obj.GetPoint();
    //        point += item.GetOffset();
    //        MoveToVector2(item, point, playerMoveInterval);
    //    }

    //    DetectionWap();
    //}
    //public void MoveToVector2(WapObjBase obj, Vector2 point, float moveTime = 1.0f, Ease ease = Ease.Linear)
    //{
    //    mapWapController.PlaceArticle(obj, point, pointToWap, moveTime, ease);
    //}

    //private void Update()
    //{
    //    DetectionEnemy();
    //    return;
    //    DetectionWap();
    //}

    //private void DetectionEnemy()
    //{
    //    List<WapObjBase> targets = new List<WapObjBase>();
    //    foreach (var wap in lastDetectionWaps)
    //    {
    //        if ((wap.GetLayerMask() & mainPlayer.GetSetAttackLayer()) != 0 && wap.TryGetObject(out WapObjBase wapObj))
    //        {
    //            if (!targets.Contains(wapObj))
    //            {
    //                targets.Add(wapObj);
    //            }
    //        }
    //    }
    //    mainConsole.ShowEnemyList(targets).Wait();
    //}

    //private void DetectionWap()
    //{
    //    //计算周围检测
    //    //init
    //    foreach (var item in lastDetectionWaps)
    //    {
    //        item.SetMouseWap(0, 1, Color.green);
    //    }
    //    lastDetectionWaps.Clear();

    //    TryGetRound(mainPlayer, playerNoDetectionLayer, ref SceneDataManager.Instance.lastDetectionWaps);
    //    foreach (var player in mainPlayer.GetSetLegion())
    //    {
    //        //init
    //        TryGetRound(player, playerNoDetectionLayer, ref SceneDataManager.Instance.lastDetectionWaps);
    //    }
    //    //--

    //    //区域变色
    //    foreach (var item in lastDetectionWaps)
    //    {
    //        Vector2 endActive = new Vector2(0.2f, 0.5f);
    //        var l = item.GetLayerMask();
    //        //if ((l & playerDetectionLayer) != 0)
    //        //{
    //        //    endActive = new Vector2(0, 1);
    //        //}
    //        item.SetMouseWap(endActive.x, endActive.y, Color.green);
    //    }
    //}
    ////根据层级检测四方 方块
    //public void TryGetRound(WapObjBase obj, LayerMask noCastlayer, ref List<Wap> wap)
    //{
    //    var allPoints = obj.GetAllPoint();
    //    foreach (var point in allPoints)
    //    {
    //        for (int i = 1; i < (int)MoveMode.EnumCount; i++)
    //        {
    //            if (dirctionPoint.TryGetValue((MoveMode)i, out Vector2 points))
    //            {
    //                points += point;
    //                if (SceneDataManager.Instance.TryGetWap(points, out Wap wap2))
    //                {
    //                    var layer = wap2.GetLayerMask();
    //                    if ((layer & noCastlayer) == 0)
    //                    {
    //                        if (!wap.Contains(wap2))
    //                        {
    //                            wap.Add(wap2);
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    //public void Addleglon(WapObjBase obj1, WapObjBase obj2)
    //{
    //    obj1.GetSetLegion(obj2);
    //    var offset = mainPlayer.GetPoint() - obj1.GetPoint();
    //    obj1.SetMoveMode(WapObjBase.StatusMode.Trusteeship, mainPlayer, offset);
    //}


    //public void RemoveEnemyObj(WapObjBase enemy)
    //{
    //    var point = enemy.GetPoint();
    //    enemy.Destroy();
    //    pointToWap[point].SetArticle(null);
    //}
    //public void SetMapPoint(Vector2 point, GameObject obj)
    //{
    //    pointToWap[point].SetArticle(obj);
    //}

    //public void ActiveBattle(WapObjBase obj1, WapObjBase obj2)
    //{
    //    SetSceneMode(SceneMode.Acttack);

    //    List<WapObjBase> obj1_legion = new List<WapObjBase>(obj1.GetSetLegion());
    //    obj1_legion.Add(obj1);
    //    List<WapObjBase> obj2_legion = new List<WapObjBase>(obj2.GetSetLegion());
    //    obj2_legion.Add(obj2);
    //    obj1.Action(obj2, obj2_legion);
    //    foreach (var item in obj1.GetSetLegion())
    //    {
    //        item.Action(obj2, obj2_legion);
    //    }
    //    obj2.Action(obj1, obj1_legion);
    //    foreach (var item in obj2.GetSetLegion())
    //    {
    //        item.Action(obj1, obj1_legion);
    //    }
    //}

    //public void SetSceneMode(SceneMode sceneMode)
    //{
    //    this.sceneMode = sceneMode;
    //    switch (sceneMode)
    //    {
    //        case SceneMode.None:
    //            break;
    //        case SceneMode.Play:
    //            break;
    //        case SceneMode.Acttack:
    //            break;
    //        case SceneMode.EnumCount:
    //            break;
    //        default:
    //            break;
    //    }
    //}

    //public void AddPlayerArticle(ulong id, int number)
    //{
    //    //增加物品
    //    mainPlayer.GetSetArticle(id, number);
    //}
    //public void AddPlayerProperty(ulong id, float number)
    //{
    //    //增加属性值
    //    mainPlayer.GetSet((WapObjBase.PropertyFloat)id, number);
    //}
    //public void AddConsumable(ulong id, int number)
    //{
    //    //增加消耗品
    //    mainPlayer.GetSetConsumable(id, number);
    //}

    //public void GameFinish()
    //{
    //    Log(Color.red, "Finish");
    //    SceneDataManager.Instance.data = mainPlayer;
    //    //ResourceManager.Instance.RemoveSceneAsync(ResourceManager.SceneMode.Battle, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
    //    //ResourceManager.Instance.LoadSceneAsync(ResourceManager.SceneMode.Boss, LoadSceneMode.Additive);
    //}

    //public void RecruitLegion(WapObjBase obj1, WapObjBase obj2)
    //{
    //    var demandItems = obj2.GetSet(WapObjBase.PropertyListString.recruitDemandArticle);
    //    List<string> items = new List<string>();
    //    bool isRecuit = true;
    //    string hintStr;
    //    foreach (var item in demandItems)
    //    {
    //        var itemAndNumber = item.Split(':');
    //        var id = ulong.Parse(itemAndNumber[0]);
    //        var number = int.Parse(itemAndNumber[1]);
    //        items.Add($"{id}:{-number}");
    //        if (!obj1.TryArticleCount(id, number))
    //        {
    //            isRecuit = false;
    //        }
    //    }
    //    if (isRecuit)
    //    {
    //        var list = obj2.GetSet(WapObjBase.PropertyListString.recruitGetArticle);
    //        Dictionary<ulong, float> testDic = new Dictionary<ulong, float>();
    //        foreach (var item in items)
    //        {
    //            var str = item.Split(':');
    //            var id = ulong.Parse(str[0]);
    //            var number = float.Parse(str[1]);
    //            testDic.Add(id, number);
    //        }
    //        foreach (var item in list)
    //        {
    //            var str = item.Split(':');
    //            var id = ulong.Parse(str[0]);
    //            var number = float.Parse(str[1]);
    //            if (testDic.ContainsKey(id))
    //            {
    //                testDic[id] += number;
    //            }
    //            else
    //            {
    //                testDic.Add(id, number);
    //            }
    //        }
    //        list = new List<string>();
    //        foreach (var item in testDic)
    //        {
    //            list.Add($"{item.Key}:{item.Value}");
    //        }
    //        AddProperty(list);
    //        var offset = obj2.GetPoint() - obj1.GetPoint();
    //        obj2.SetMoveMode(WapObjBase.StatusMode.Trusteeship, obj1, offset);
    //        obj1.GetSetLegion(obj2);
    //        obj2.SetLayer(obj1.gameObject.layer);
    //        obj2.GetSetAttackLayer(obj1.GetSetAttackLayer());
    //        hintStr = $"Recuit -{obj2.GetName()}- finish ...";
    //    }
    //    else
    //    {
    //        hintStr = $"Recuit -{obj2.GetName()}- Scant supply of material ! ";
    //    }
    //    mainConsole.HintInformation(hintStr).Wait();
    //}

    //public void AddProperty(List<string> property)
    //{
    //    Dictionary<ulong, int> articleList = new Dictionary<ulong, int>();
    //    Dictionary<ulong, int> consumablesList = new Dictionary<ulong, int>();
    //    Dictionary<ulong, float> propertyList = new Dictionary<ulong, float>();
    //    foreach (var item in property)
    //    {
    //        var article = item.Split(':');
    //        var id = ulong.Parse(article[0]);
    //        var number = float.Parse(article[1]);
    //        switch (MasterData.Instance.GetTableData<LocalItemData>(id).type)
    //        {
    //            case 1:
    //                propertyList.Add(id, number);
    //                break;
    //            case 2:
    //                articleList.Add(id, (int)number);
    //                break;
    //            case 3:
    //                consumablesList.Add(id, (int)number);
    //                break;
    //            default:
    //                break;
    //        }
    //    }
    //    foreach (var item in articleList)
    //    {
    //        //增加物品
    //        AddPlayerArticle(item.Key, item.Value);
    //    }
    //    foreach (var item in consumablesList)
    //    {
    //        //增加消耗品
    //        AddConsumable(item.Key, item.Value);
    //    }
    //    foreach (var item in propertyList)
    //    {
    //        //增加属性值
    //        AddPlayerProperty(item.Key, item.Value);
    //    }
    //    mainConsole.UpdatePlayerProperty().Wait();
    //}
































































































    ///// <summary>
    ///// 一次移动一格
    ///// </summary>
    //public List<Vector2> MoveToTarget(WapObjBase obj, Vector2 target)
    //{
    //    List<Vector2> pointPath = new List<Vector2>();
    //    if (pointToWap[target].TryGetObject(out Transform downObj))
    //    {
    //        mainConsole.HintInformation($"None move, because target point have game object ...  -{downObj.name}-").Wait();
    //    }
    //    else
    //    {
    //        var dic = dirctionPoint;
    //        var obj2 = obj;
    //        List<Wap> wapList = new List<Wap>();
    //        foreach (var item in dic)
    //        {
    //            var point = item.Value + obj2.GetPoint();
    //            if (SceneDataManager.Instance.TryGetWap(point, out Wap wap))
    //            {
    //                if (!wap.TryGetObject(out Transform tr))
    //                {
    //                    wapList.Add(wap);
    //                }
    //            }
    //        }
    //        TreeFour node = new TreeFour(pointToWap[obj2.GetPoint()], wapList, null);


    //    }

    //    void Loop(Vector2 target, TreeFour parent)
    //    {
    //        var dic = dirctionPoint;
    //        var obj = parent.GetNode();
    //        List<Wap> wapList = new List<Wap>();
    //        foreach (var item in dic)
    //        {
    //            var point = item.Value + obj.GetPoint();

    //            if (SceneDataManager.Instance.TryGetWap(point, out Wap wap))
    //            {
    //                if (!wap.TryGetObject(out Transform tr))
    //                {
    //                    wapList.Add(wap);
    //                }
    //            }
    //            //if (target == point)
    //            //{
    //            //    return;
    //            //}
    //        }
    //        TreeFour node = new TreeFour(pointToWap[obj.GetPoint()], wapList, parent);






    //        Loop(target, node);
    //    }

    //    return pointPath;
    //}
}

public class TreeFour
{
    private Wap node = null;
    private int index = 0;
    private List<Wap> childs = new List<Wap>();
    private TreeFour parent = null;
    public TreeFour(Wap f_node, List<Wap> f_childs, TreeFour f_parent)
    {
        node = f_node;
        childs = f_childs;
        parent = f_parent;
        index = 0;
    }
    public Wap GetNode()
    {
        return node;
    }
    public TreeFour GetSetParent()
    {
        return parent;
    }
    public bool TryGetNextWap(out Wap wap)
    {
        bool ret = false;
        wap = null;
        if (index < childs.Count)
        {
            wap = childs[index];
            index++;
            ret = true;
        }
        return ret;
    }

}