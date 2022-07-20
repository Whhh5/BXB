using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using DG.Tweening;

public abstract class WapObjBase : MiObjPoolPublicParameter, ICommon_Weapon
{
    public enum PropertyFloat
    {
        None,
        maxBlood,
        attack,
        defend,
        attackInterval,
        EnumCount,
    }
    public enum PropertyListString
    {
        recruitGetArticle,
        recruitDemandArticle,
        EnumCount,
    }
    public enum StatusMode
    {
        None,
        Manual,
        Trusteeship, //托管
        AutoPath,
        EnumCount,
    }
    public enum Status
    {
        None,
        Idle,
        Walk,
        Attack,
        Skill,
        Die,
        EnumCound,
    }
    [SerializeField] protected GameObject main;
    [SerializeField] string playerName = "";
    [SerializeField] private float nowBlood;

    [SerializeField, ReadOnly] float lastMoveTime;
    [SerializeField] WapObjBase target_Lord;
    [SerializeField] Vector2 target_Lord_Offset;
    [SerializeField] protected Vector2 nowPoint = new Vector2(-1, -1);
    [SerializeField] protected List<Vector2> extendPoint = new List<Vector2>();
    [SerializeField] protected Vector2 attack_Scope = new Vector2(0, 0);
    [SerializeField] List<WapObjBase> legionPoint = new List<WapObjBase>();
    [SerializeField] protected LayerMask layer_attack;

    [SerializeField] protected StatusMode moveMode;
    [SerializeField] int level;
    [SerializeField] protected Vector2 attackRange;
    [SerializeField] Animator anima;
    [SerializeField, ReadOnly] List<WapObjBase> attack_Target = null;

    [SerializeField] Dictionary<PropertyFloat, float> levelPropertyDic = new Dictionary<PropertyFloat, float>();
    [SerializeField] Dictionary<PropertyFloat, float> externalPropertyDic = new Dictionary<PropertyFloat, float>();
    [SerializeField] Dictionary<PropertyListString, List<string>> dieAndRecruittDic = new Dictionary<PropertyListString, List<string>>();
    [SerializeField] Dictionary<ulong, int> consumableDic = new Dictionary<ulong, int>();
    [SerializeField] Dictionary<ulong, int> articleDic = new Dictionary<ulong, int>();
    [SerializeField]
    Dictionary<PropertyFloat, short> additionDic = new Dictionary<PropertyFloat, short>
    {
        { PropertyFloat.attack, 0},

    };

    public Dictionary<ulong, int> articleGet { get => articleDic; }
    public Dictionary<ulong, int> consumableGet { get => consumableDic; }
    public Dictionary<PropertyFloat, float> levelPropertyGet { get => levelPropertyDic; }
    public Dictionary<PropertyFloat, float> externalPropertyGet { get => externalPropertyDic; }
    public Dictionary<PropertyListString, List<string>> dieAndRecruittGet { get => dieAndRecruittDic; }


    public bool TryGetMianCom<T>(out T component) where T : class
    {
        bool ret = false;
        if (main.TryGetComponent<T>(out component))
        {
            ret = true;
        }
        return ret;
    }
    public GameObject GetMain()
    {
        return main;
    }
    public string GetName()
    {
        return playerName;
    }
    public int GetLevel()
    {
        return level;
    }
    public void SetAttackTarget(List<WapObjBase> targets)
    {
        attack_Target = targets;
    }
    public void SetLayer(LayerMask layer)
    {
        gameObject.layer = layer;
    }
    public float GetSetBlood(float increment = 0)
    {
        float ret;
        ret = nowBlood;
        ret += increment;
        ret = ret < 0 ? 0 : ret;
        nowBlood = ret;
        return ret;
    }
    public virtual void OnInit()
    {
        //init dictionary
        legionPoint.Clear();
        attack_Target.Clear();
        SetStatus(Status.Idle);
        levelPropertyDic = new Dictionary<PropertyFloat, float>();
        externalPropertyDic = new Dictionary<PropertyFloat, float>();
        dieAndRecruittDic = new Dictionary<PropertyListString, List<string>>();
        for (int i = 0; i < (int)PropertyFloat.EnumCount; i++)
        {
            levelPropertyDic.Add((PropertyFloat)i, 0.0f);
        }
        for (int i = 0; i < (int)PropertyFloat.EnumCount; i++)
        {
            externalPropertyDic.Add((PropertyFloat)i, 0.0f);
        }
        for (int i = 0; i < (int)PropertyListString.EnumCount; i++)
        {
            dieAndRecruittDic.Add((PropertyListString)i, new List<string>());
        }
        var rolestableData = MasterData.Instance.GetTableData<LocalRolesData>(GetId());
        level = rolestableData.level;
        for (int i = 0; i < (int)PropertyFloat.EnumCount; i++)
        {
            try
            {
                var levelDataTable = MasterData.Instance.GetTableData<LocalRolesLevelData>((ulong)level);
                var fileName = ((PropertyFloat)i).ToString();
                var levelFiles = levelDataTable.GetType().GetField(fileName);
                var value = levelFiles.GetValue(levelDataTable);
                levelPropertyDic[(PropertyFloat)i] = (float)value;
            }
            catch (Exception)
            {
                Log(Color.red, $"Absent property   {((PropertyFloat)i).ToString()}");
            }
        }
        for (int i = 0; i < (int)PropertyListString.EnumCount; i++)
        {
            try
            {
                var data = rolestableData.GetType().GetField(((PropertyListString)i).ToString());
                var dataValue = data.GetValue(rolestableData);
                dieAndRecruittDic[(PropertyListString)i] = new List<string>((string[])dataValue);
            }
            catch (Exception)
            {
            }
        }
        //--
        nowBlood = MasterData.Instance.GetTableData<LocalRolesLevelData>((ulong)level).maxBlood;
        //
        playerName = rolestableData.name;
    }
    public float GetSet(PropertyFloat mode, float increment = 0)
    {
        float ret = 0.0f;
        try
        {
            if (mode == PropertyFloat.maxBlood)
            {
                nowBlood += increment;
                nowBlood = nowBlood < 0 ? 0 : nowBlood;
                ret = nowBlood;
                return ret;
            }
            var levelProperty = levelPropertyDic[mode];
            var externalProperty = externalPropertyDic[mode];
            externalProperty += increment;
            externalProperty = externalProperty < 0 ? 0 : externalProperty;
            externalPropertyDic[mode] = externalProperty;
            ret = levelProperty + externalProperty;
        }
        catch (Exception)
        {
        }
        return ret;
    }
    public List<string> GetSet(PropertyListString mode)
    {
        List<string> ret = new List<string>();
        try
        {
            ret = dieAndRecruittDic[mode];
        }
        catch (Exception)
        {
        }
        return ret;
    }
    public bool TryArticleCount(ulong id, int count)
    {
        bool ret = false;
        if (articleDic.ContainsKey(id) && articleDic[id] >= count)
        {
            ret = true;
        }
        return ret;
    }
    public int GetSetArticle(ulong id, int increment)
    {
        int ret = increment;
        if (articleDic.ContainsKey(id))
        {
            var num = articleDic[id];
            num += increment;
            num = num < 0 ? 0 : num;
            articleDic[id] = num;
            ret = num;
        }
        else
        {
            articleDic.Add(id, increment);
        }
        return ret;
    }

    public int GetSetConsumable(ulong id, int increment = 0)
    {
        int ret = increment;
        if (consumableDic.ContainsKey(id))
        {
            var num = consumableDic[id];
            num += increment;
            num = num < 0 ? 0 : num;
            consumableDic[id] = num;
            ret = num;
        }
        else
        {
            consumableDic.Add(id, increment);
        }
        return ret;
    }

    public abstract void OnSetInit(object[] value);
    public object Clone()
    {
        return MemberwiseClone();
    }

    public void UpLevel(int up = 1)
    {
        level += up;
        level = level < 0 ? 0 : level;
    }

    public void SetPont(Vector2 point)
    {
        this.nowPoint = point;
    }
    public List<Vector2> GetExtendPoint()
    {
        return extendPoint;
    }
    public Vector2 GetPoint()
    {
        return nowPoint;
    }
    public List<Vector2> GetAllPoint()
    {
        List<Vector2> list = new List<Vector2>() { GetPoint() };
        foreach (var item in extendPoint)
        {
            list.Add(GetPoint() + item);
        }
        return list;
    }


    private void Update()
    {
        switch (moveMode)
        {
            case StatusMode.None:
                break;
            case StatusMode.Manual:
                ManualMove();
                break;
            case StatusMode.Trusteeship:
                TrusteeshipMove();
                break;
            case StatusMode.EnumCount:
                break;
            default:
                break;
        }
    }


    public abstract List<string> Die();

    private void ManualMove()
    {
        if (Input.anyKey)
        {
            if (Time.time - lastMoveTime >= BattleSceneManager.Instance.playerMoveInterval)
            {
                lastMoveTime = Time.time;
                BattleSceneManager.MoveMode moveMode = BattleSceneManager.MoveMode.None;

                foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKey(keyCode))
                    {
                        bool isDown = false;
                        switch (keyCode)
                        {
                            case KeyCode.W:
                                moveMode = BattleSceneManager.MoveMode.Top;
                                isDown = true;
                                break;
                            case KeyCode.A:
                                moveMode = BattleSceneManager.MoveMode.Left;
                                isDown = true;
                                break;
                            case KeyCode.S:
                                moveMode = BattleSceneManager.MoveMode.Down;
                                isDown = true;
                                break;
                            case KeyCode.D:
                                moveMode = BattleSceneManager.MoveMode.Right;
                                isDown = true;
                                break;
                            default:
                                break;
                        }
                        BattleSceneManager.Instance.MoveToDirection(this, moveMode);
                        if (isDown)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }

    //托管移动逻辑
    public void TrusteeshipMove()
    {

    }
    public void SetTargetMove(List<Vector2> pointPath)
    {
        var nowMode = moveMode;
        moveMode = StatusMode.AutoPath;

        foreach (var point in pointPath)
        {
            BattleSceneManager.Instance.SetMapPoint(point, gameObject);
        }

        moveMode = nowMode;
    }
    public Vector2 GetOffset()
    {
        return target_Lord_Offset;
    }
    public void SetMoveMode(StatusMode mode, WapObjBase target = null, Vector2 offset = default)
    {
        moveMode = mode;
        this.target_Lord = target;
        this.target_Lord_Offset = offset;
    }
    public List<WapObjBase> GetSetLegion(WapObjBase legion = null)
    {
        if (legion != null && !legionPoint.Contains(legion))
        {
            legionPoint.Add(legion);
        }
        return legionPoint;
    }
    public List<WapObjBase> RemoveLegion(WapObjBase legion)
    {
        if (legion != null && legionPoint.Contains(legion))
        {
            legionPoint.Remove(legion);
            BattleSceneManager.Instance.RemoveEnemyObj(legion);
        }
        return legionPoint;
    }

    protected bool idExitCoroutine = true;
    public virtual void Action(params object[] value)
    {
        var lord = (WapObjBase)value[0];
        var enemys = (List<WapObjBase>)value[1];
        if (idExitCoroutine)
        {
            StartCoroutine(IE_Action(lord, enemys));
        }
    }
    public abstract IEnumerator IE_Action(WapObjBase enemy, List<WapObjBase> enemys);

    public override void Destroy()
    {
        base.Destroy();
        StopCoroutine(IE_Action(null, null));
        var point = GetPoint();
        target_Lord?.RemoveLegion(this);
        var listPoint = GetAllPoint();
        SetStatus(Status.Die);
        foreach (var item in listPoint)
        {
            BattleSceneManager.Instance.SetMapPoint(item, null);
        }
    }
    public LayerMask GetSetAttackLayer(LayerMask layerMask = default)
    {
        if (layerMask != default)
        {
            layer_attack = layerMask;
        }
        return layer_attack;
    }
    public int GetLayerMask()
    {
        return (int)Mathf.Pow(2, gameObject.layer);
    }


    public bool TryGetAnimaController(out Animator anima)
    {
        bool ret = false;
        anima = null;
        if (this.anima != null)
        {
            anima = this.anima;
            ret = true;
        }
        return ret;
    }

    public void SetStatus(Status toState, float speed = 1, bool isforcePlay = true)
    {
        try
        {
            var name = anima.name.Split(new string[] { "(Clone)" }, StringSplitOptions.RemoveEmptyEntries);
            var clickName = $"{name[0]}_{toState}";
            var stateId = Animator.StringToHash(clickName);
            var isAnimatorHash = anima.HasState(0, stateId);
            if (isAnimatorHash)
            {
                var isPlay = anima.GetCurrentAnimatorStateInfo(0);
                if (!isPlay.IsName(clickName) || isforcePlay)
                {
                    Log(Color.green, $"Play Animation  {clickName}");
                    anima.Play(clickName, 0, 0);
                    anima.speed = speed;
                }
            }
        }
        catch (Exception)
        {
        }

    }
    public void SetAnimatorSpeedNorm(float speed)
    {
        anima.speed = 1;
    }
    public List<Wap> GetAttackScope()
    {
        var allPointList = GetAllPoint();
        var retList = new List<Wap>();
        for (int i = -(int)attack_Scope.x; i <= (int)attack_Scope.x; i++)
        {
            for (int j = -(int)attack_Scope.y; j <= (int)attack_Scope.y; j++)
            {
                foreach (var item in allPointList)
                {
                    var point = item + new Vector2(i, j);
                    if (BattleSceneManager.Instance.TryGetWap(point, out Wap wap))
                    {
                        if (!wap.TryGetObject(out WapObjBase obj) || 
                            ((obj.GetLayerMask() & GetLayerMask()) == 0 && !retList.Contains(wap)))
                        {
                            retList.Add(wap);
                        }
                    }
                }
            }
        }
        return retList;
    }

    protected List<WapObjBase> GetAtactTargets(List<Vector2> allPointList)
    {
        List<WapObjBase> attack_targets = new List<WapObjBase>();
        //查找范围内 攻击目标
        for (int i = -(int)attack_Scope.x; i <= (int)attack_Scope.x; i++)
        {
            for (int j = -(int)attack_Scope.y; j <= (int)attack_Scope.y; j++)
            {
                foreach (var item in allPointList)
                {
                    var point = item + new Vector2(i, j);
                    if (BattleSceneManager.Instance.TryGetWap(point, out Wap wap))
                    {
                        if (wap.TryGetObject(out WapObjBase obj))
                        {
                            if (((int)Mathf.Pow(2, obj.gameObject.layer) & layer_attack) != 0 && !attack_targets.Contains(obj))
                            {
                                attack_targets.Add(obj);
                            }
                        }
                    }
                }
            }
        }
        this.SetAttackTarget(attack_targets);
        return attack_targets;
    }

    protected void AttactTarget(WapObjBase target, Action dieEvent)
    {
        if (!(BattleSceneManager.Instance.sceneMode != BattleSceneManager.SceneMode.Acttack) && target != null)
        {
            Debug.DrawLine(transform.position, target.transform.position, Color.red);
            try
            {
                var data = MiDataManager.Instance.dataProceccing.AttackData(this, target);
                var nowEnemyBlood = target.GetSetBlood(-data);

                //attack number hint
                var targetPos = BattleSceneManager.Instance.sceneMainCamera.WorldToScreenPoint(target.transform.position);
                var hintPath = CommonManager.Instance.filePath.PreUIDialogSystemPath;
                var hintObj = ResourceManager.Instance.GetUIElementAsync<UIElement_NumberHint>(hintPath, "UIElement_NumberHint", BattleSceneManager.Instance.mainConsole.GetComponent<RectTransform>(), targetPos, -data);

                if (nowEnemyBlood <= 0)
                {
                    var list = target.Die();
                    BattleSceneManager.Instance.AddProperty(list);
                    dieEvent.Invoke();
                }
                SetStatus(Status.Attack);
            }
            catch (Exception)
            {
            }
            BattleSceneManager.Instance.mainConsole.UpdatePlayerProperty().Wait();
        }
    }
}
