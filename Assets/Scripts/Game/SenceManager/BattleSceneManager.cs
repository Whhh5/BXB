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
    public float playerMoveInterval = 0.0f;
    public List<WapObjBase> legion = new List<WapObjBase>();
    public List<Wap> lastDetectionWaps = new List<Wap>();
    public LayerMask playerNoDetectionLayer;
    public LayerMask playerDetectionLayer;
    public LayerMask attackLayer;
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
        legion.Clear();
        var charPath = CommonManager.Instance.filePath.ResArticle;
        var charObj = ResourceManager.Instance.GetWorldObject<CharacterController>(charPath, "Player", new Vector3(0, 0, 0), null, f_id: 110000001);
        Addleglon(charObj);
        mainPlayer = charObj;
        mapWapController.PlaceArticle(charObj, new Vector2(0, 0), pointToWap, 0, Ease.Linear);


        var enemyPath = ResourceManager.Instance.GetWorldObject<WapObjBase>(charPath, "TestEnemy1", new Vector3(0, 0, 0), null, f_id: 110020001);
        mapWapController.PlaceArticle(enemyPath, new Vector2(3, 3), pointToWap, 0, Ease.Linear);
    }

    public void SetWap(WapObjBase obj, MoveMode mode)
    {
        if (mode == MoveMode.None || mode == MoveMode.EnumCount) return;
        var isMove = dirctionIsMove[mode];
        switch (isMove && (sceneMode & SceneMode.Play) != 0)
        {
            case true:
                break;
            case false:
                return;
        }
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
        catch (Exception)
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
            if ((wap.GetLayerMask() & attackLayer) != 0 && wap.TryGetObject(out WapObjBase wapObj))
            {
                targets.Add(wapObj);
            }
        }
        mainConsole.ShowEnemyList(targets).Wait();
    }

    private void DetectionWap()
    {
        var waplist = lastDetectionWaps;
        //init
        foreach (var item in waplist)
        {
            item.SetMouseWap(0, 1);
        }
        waplist.Clear();
        foreach (var player in legion)
        {
            //init
            var point = player.GetPoint();

            for (int i = 1; i < (int)MoveMode.EnumCount; i++)
            {
                if (dirctionPoint.TryGetValue((MoveMode)i, out Vector2 points))
                {
                    points += point;
                    if (TryGetWap(points, out Wap wap))
                    {
                        var l = wap.GetLayerMask();
                        if ((l & playerNoDetectionLayer) == 0)
                        {
                            waplist.Add(wap);
                        }
                        bool dirctionMove = false;
                        if ((l & playerDetectionLayer) == 0)
                        {
                            dirctionMove = true;
                        }
                        dirctionIsMove[(MoveMode)i] = dirctionMove;
                    }
                }
            }
        }
        foreach (var item in waplist)
        {
            Vector2 endActive = new Vector2(0.5f, 0.5f);
            var l = item.GetLayerMask();
            if ((l & playerDetectionLayer) != 0)
            {
                endActive = new Vector2(0, 1);
            }
            item.SetMouseWap(endActive.x, endActive.y);
        }
    }

    public void Addleglon(WapObjBase obj)
    {
        legion.Add(obj);
    }


    public void RemoveEnemyObj(WapObjBase enemy)
    {
        var point = enemy.GetPoint();
        enemy.Destroy();
        pointToWap[point].SetArticle(null);
    }

    public void ActiveBattle(WapObjBase obj1, WapObjBase obj2)
    {
        SetSceneMode(SceneMode.Acttack);

        //obj1.Active(obj2, new Action(() => { SetSceneMode(SceneMode.Play); }));

        StartCoroutine(Battle1(obj1, obj2));
        StartCoroutine(Battle2(obj2, obj1));
    }

    public IEnumerator Battle1(WapObjBase obj1, WapObjBase obj2)
    {
        var enemy = obj2;

        while (!(BattleSceneManager.Instance.sceneMode != BattleSceneManager.SceneMode.Acttack))
        {
            try
            {
                var nowEnemyBlood = enemy.nowBlood;
                var data = BattleSceneManager.Instance.Date(obj1, enemy);
                nowEnemyBlood -= data;
                nowEnemyBlood = nowEnemyBlood < 0 ? 0 : nowEnemyBlood;
                enemy.nowBlood = nowEnemyBlood;


                //attack number hint
                var targetPos = BattleSceneManager.Instance.sceneMainCamera.WorldToScreenPoint(enemy.transform.position);
                var hintPath = CommonManager.Instance.filePath.PreUIDialogSystemPath;
                var hintObj = ResourceManager.Instance.GetUIElementAsync<UIElement_NumberHint>(hintPath, "UIElement_NumberHint", BattleSceneManager.Instance.mainConsole.GetComponent<RectTransform>(), targetPos, -data);
                //

                if (nowEnemyBlood <= 0)
                {
                    enemy.Die();
                    SetSceneMode(SceneMode.Play);
                }
            }
            catch (Exception)
            {

                throw;
            }
            yield return new WaitForSeconds(obj1.property.attackInterval);
        }
    }
    public IEnumerator Battle2(WapObjBase obj1, WapObjBase obj2)
    {
        var enemy = obj2;

        while (!(BattleSceneManager.Instance.sceneMode != BattleSceneManager.SceneMode.Acttack))
        {
            try
            {
                var nowEnemyBlood = enemy.nowBlood;
                var data = BattleSceneManager.Instance.Date(obj1, enemy);
                nowEnemyBlood -= data;
                nowEnemyBlood = nowEnemyBlood < 0 ? 0 : nowEnemyBlood;
                enemy.nowBlood = nowEnemyBlood;


                //attack number hint
                var targetPos = BattleSceneManager.Instance.sceneMainCamera.WorldToScreenPoint(enemy.transform.position);
                var hintPath = CommonManager.Instance.filePath.PreUIDialogSystemPath;
                var hintObj = ResourceManager.Instance.GetUIElementAsync<UIElement_NumberHint>(hintPath, "UIElement_NumberHint", BattleSceneManager.Instance.mainConsole.GetComponent<RectTransform>(), targetPos, -data);
                //

                if (nowEnemyBlood <= 0)
                {
                    enemy.Die();
                    SetSceneMode(SceneMode.Play);
                }
            }
            catch (Exception)
            {

                throw;
            }
            yield return new WaitForSeconds(obj1.property.attackInterval);
        }
    }

    public float Date(WapObjBase obj1, WapObjBase obj2)
    {
        float ret = obj1.property.attack - obj2.property.defence;
        ret = ret < 0 ? 0 : ret;
        return ret;
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

    public void AddSystemItems(ItemsType itemsType, int number)
    {
        mainConsole.AddSystemItems(itemsType, number);
    }

    public void GameFinish()
    {
        Log(Color.red, "Finish");
    }

    public void RecruitLegion(WapObjBase obj1, WapObjBase obj2)
    {
        var pro = obj2.Recruit();
        //var pros = pro.Split
        foreach (var item in pro.Split(';'))
        {
            var para = item.Split(':');
            var number = float.Parse(para[1]);
            switch (int.Parse(para[0]))
            {
                case 1:
                    obj1.nowBlood += number;
                    break;
                case 2:
                    obj1.property.attack += number;
                    break;
                case 3:
                    obj1.property.defence += number;
                    break;
                case 4:
                    obj1.property.attackInterval += number;
                    break;
                default:
                    break;
            }
        }
    }

}
