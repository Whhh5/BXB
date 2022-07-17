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
    //Scene
    public SceneMode sceneMode = SceneMode.Play;

    //plsyer
    public CharacterController mainPlayer;
    public float playerMoveInterval = 0.3f;
    //public List<WapObjBase> legionPoint = new List<WapObjBase>();
    public List<Wap> lastDetectionWaps = new List<Wap>();
    public LayerMask playerNoDetectionLayer;
    public LayerMask playerDetectionLayer;
    //player

    //Enemy
    public List<TestEnemy1> enemys = new List<TestEnemy1>();
    //Enemy

    //camera
    public Camera sceneMainCamera;
    //camera

    //mouse
    public Wap mouseDownWap;
    //mouse

    //Console
    public UIDialog_Battle_MainConsole mainConsole;
    //Console

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
            mainConsole = await ResourceManager.Instance.ShowDialogAsync<UIDialog_Battle_MainConsole>(path, "UIDialog_Battle_MainConsole", CanvasLayer.System);
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
        var legion1 = ResourceManager.Instance.GetWorldObject<CharacterController>(charPath, "Player", new Vector3(0, 0, 0), null, f_id: 110000001);
        legion1.SetMoveMode(WapObjBase.StatusMode.Trusteeship, charObj, new Vector2(1, 0));
        charObj.GetSetLegion(legion1);

        mainPlayer = charObj;
        mapWapController.PlaceArticle(charObj, new Vector2(0, 0), pointToWap, 0, Ease.Linear);


        var enemyPath = ResourceManager.Instance.GetWorldObject<WapObjBase>(charPath, "TestEnemy1", new Vector3(0, 0, 0), null, f_id: 110020001);
        mapWapController.PlaceArticle(enemyPath, new Vector2(3, 3), pointToWap, 0, Ease.Linear);

        var enemyPath2 = ResourceManager.Instance.GetWorldObject<WapObjBase>(charPath, "TestEnemy1", new Vector3(0, 0, 0), null, f_id: 110020001);
        mapWapController.PlaceArticle(enemyPath2, new Vector2(4, 5), pointToWap, 0, Ease.Linear);

        mainPlayer.GetSetArticle(120050001, 66);
        mainConsole.UpdatePlayerProperty().Wait();

        mainConsole.UpdatePlayerProperty().Wait();
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
            var pos = item.GetPoint() + new Vector2(x, y);
            if (TryGetWap(pos, out Wap wap2))
            {
                if ((wap2.GetLayerMask() & playerDetectionLayer) != 0)
                {
                    return;
                }
            }
        }


        //foreach (var item in obj.GetSetLegion())
        //{
        //    pointToWap[item.GetPoint()] = null;
        //}
        //�ƶ�����Լ�
        var oldPoint = obj.GetPoint();
        var newPoint = oldPoint + new Vector2(x, y);
        pointToWap[oldPoint].SetArticle(null);
        MoveToVector2(obj, newPoint, playerMoveInterval);

        //�ƶ�����
        foreach (var item in obj.GetSetLegion())
        {
            var point = obj.GetPoint();
            point += item.GetOffset();
            MoveToVector2(item, point, playerMoveInterval);
        }
    }
    public void MoveToVector2(WapObjBase obj, Vector2 point, float moveTime = 1.0f, Ease ease = Ease.Linear)
    {
        mapWapController.PlaceArticle(obj, point, pointToWap, moveTime, ease);
    }

    public void SetMouseWap(Wap wap)
    {
        this.mouseDownWap = wap;
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

    private void Update()
    {
        DetectionWap();
        DetectionEnemy();
    }

    private void DetectionEnemy()
    {
        List<WapObjBase> targets = new List<WapObjBase>();
        foreach (var wap in lastDetectionWaps)
        {
            if ((wap.GetLayerMask() & mainPlayer.GetSetAttackLayer()) != 0 && wap.TryGetObject(out WapObjBase wapObj))
            {
                targets.Add(wapObj);
            }
        }
        mainConsole.ShowEnemyList(targets).Wait();
    }

    private void DetectionWap()
    {
        //������Χ���
        var waplist = lastDetectionWaps;
        //init
        foreach (var item in waplist)
        {
            item.SetMouseWap(0, 1);
        }
        waplist.Clear();

        TryGetRound(mainPlayer, playerNoDetectionLayer, ref waplist);
        foreach (var player in mainPlayer.GetSetLegion())
        {
            //init
            TryGetRound(player, playerNoDetectionLayer, ref waplist);
        }
        //--

        //�����ɫ
        foreach (var item in waplist)
        {
            Vector2 endActive = new Vector2(0.2f, 0.5f);
            var l = item.GetLayerMask();
            if ((l & playerDetectionLayer) != 0)
            {
                endActive = new Vector2(0, 1);
            }
            item.SetMouseWap(endActive.x, endActive.y);
        }
    }
    //���ݲ㼶����ķ� ����
    public void TryGetRound(WapObjBase obj, LayerMask noCastlayer, ref List<Wap> wap)
    {
        var point = obj.GetPoint();
        for (int i = 1; i < (int)MoveMode.EnumCount; i++)
        {
            if (dirctionPoint.TryGetValue((MoveMode)i, out Vector2 points))
            {
                points += point;
                if (TryGetWap(points, out Wap wap2))
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
        //������Ʒ
        mainPlayer.GetSetArticle(id, number);
    }
    public void AddPlayerProperty(ulong id, float number)
    {
        //��������ֵ
        mainPlayer.GetSet((WapObjBase.PropertyFloat)id, number);
    }
    public void AddConsumable(ulong id, int number)
    {
        //��������Ʒ
        mainPlayer.GetSetConsumable(id, number);
    }

    public void GameFinish()
    {
        Log(Color.red, "Finish");
    }

    public void RecruitLegion(WapObjBase obj1, WapObjBase obj2)
    {
        var demandItems = obj2.GetSet(WapObjBase.PropertyListString.recruitDemandArticle);
        List<string> items = new List<string>();
        bool isRecuit = true;
        string hintStr;
        foreach (var item in demandItems)
        {
            var itemAndNumber = item.Split(':');
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
            //������Ʒ
            AddPlayerArticle(item.Key, item.Value);
        }
        foreach (var item in consumablesList)
        {
            //��������Ʒ
            AddConsumable(item.Key, item.Value);
        }
        foreach (var item in propertyList)
        {
            //��������ֵ
            AddPlayerProperty(item.Key, item.Value);
        }
        mainConsole.UpdatePlayerProperty().Wait();
    }































































































    /// <summary>
    /// һ���ƶ�һ��
    /// </summary>
    public List<Vector2> MoveToTarget(WapObjBase obj, Vector2 target)
    {
        List<Vector2> pointPath = new List<Vector2>();
        if (pointToWap[target].TryGetObject(out Transform downObj))
        {
            mainConsole.HintInformation($"None move, because target point have game object ...  -{downObj.name}-").Wait();
        }
        else
        {
            var dic = dirctionPoint;
            var obj2 = obj;
            List<Wap> wapList = new List<Wap>();
            foreach (var item in dic)
            {
                var point = item.Value + obj2.GetPoint();
                if (TryGetWap(point, out Wap wap))
                {
                    if (!wap.TryGetObject(out Transform tr))
                    {
                        wapList.Add(wap);
                    }
                }
            }
            TreeFour node = new TreeFour(pointToWap[obj2.GetPoint()], wapList, null);


        }

        void Loop(Vector2 target, TreeFour parent)
        {
            var dic = dirctionPoint;
            var obj = parent.GetNode();
            List<Wap> wapList = new List<Wap>();
            foreach (var item in dic)
            {
                var point = item.Value + obj.GetPoint();

                if (TryGetWap(point, out Wap wap))
                {
                    if (!wap.TryGetObject(out Transform tr))
                    {
                        wapList.Add(wap);
                    }
                }
                //if (target == point)
                //{
                //    return;
                //}
            }
            TreeFour node = new TreeFour(pointToWap[obj.GetPoint()], wapList, parent);






            Loop(target, node);
        }

        return pointPath;
    }
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