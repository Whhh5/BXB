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
    [SerializeField] protected GameObject main;
    [SerializeField] string playerName = "";
    [SerializeField] private float nowBlood;

    [SerializeField, ReadOnly] float lastMoveTime;
    [SerializeField] WapObjBase target_Lord;
    [SerializeField] Vector2 target_Lord_Offset;
    [SerializeField] protected Vector2 point = new Vector2(-1, -1);
    [SerializeField] protected Vector2 attack_Scope = new Vector2(0, 0);
    [SerializeField] List<WapObjBase> legionPoint = new List<WapObjBase>();
    [SerializeField] protected LayerMask layer_attack;

    [SerializeField] protected StatusMode moveMode;
    [SerializeField] int level;
    [SerializeField] protected Vector2 attackRange;
    [SerializeField, ReadOnly] WapObjBase attack_Target = null;

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
    public void SetAttackTarget(WapObjBase target)
    {
        attack_Target = target;
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
        this.point = point;
    }
    public Vector2 GetPoint()
    {
        return point;
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

        BattleSceneManager.Instance.SetMapPoint(point, null);
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


    //使用物品
    Tween tween_usageArticle = DOTween.To(() => 2, value =>
       {

       }, 0, 1);
    public void UsageArticle(ulong id, int number)
    {
        if (tween_usageArticle.IsPlaying())
        {

        }
    }
}
