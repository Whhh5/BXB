using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using BXB.Core;

public class Common_Horizontal_Word : MiBaseMonoBeHaviourClass
{
    enum Mode
    {
        Left,
        Center,
        Right,
    }
    [SerializeField] Transform main;
    [SerializeField, ReadOnly] GameObject original;
    [SerializeField] Mode mode;
    [SerializeField] List<Transform> targets = new List<Transform>();
    [SerializeField] Vector2 interval_MoveTime;
    protected override void OnStart()
    {
        base.OnStart();
    }
    protected override void OnAwake()
    {
        base.OnAwake();
        Clear();
    }
    void UpdateGroup()
    {
        var activePoint = 1;
        switch (mode)
        {
            case Mode.Left:
                activePoint = -1;
                break;
            //case Mode.Center:
            //    activePoint = -1;
                //break;
            case Mode.Right:
                activePoint = 1;
                break;
            default:
                activePoint = -1;
                break;
        }
        var targetCount = targets.Count;
        var deuceDirection = (float)(interval_MoveTime.x * (((targetCount - 1) * 0.5))) * activePoint;

        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].DOLocalMoveX(deuceDirection + interval_MoveTime.x * i * (-activePoint), interval_MoveTime.y, false);
        }
    }
    public void Setting(GameObject original, List<Transform> targets = null, Vector2 interval_MoveTime = default)
    {
        if (targets != null)
        {
            this.targets = targets;
        }
        this.original = original;
        if (interval_MoveTime != default)
        {
            this.interval_MoveTime = interval_MoveTime;
        }
        UpdateGroup();
    }
    public void AddElement(Transform transform)
    {
        transform.Normalization(main);
        targets.Add(transform);
        UpdateGroup();
    }
    public void Clear()
    {
        if (original != null)
        {
            foreach (var para in targets)
            {
                para.Normalization(null);
                ObjPool.Repulace(original, para.gameObject).Wait();
            }
            original = null;
        }
        targets = new List<Transform>();
    }
}
