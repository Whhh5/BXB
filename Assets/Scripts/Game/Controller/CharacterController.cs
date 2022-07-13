using BXB.Core;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : WapObjBase
{
    [SerializeField,ReadOnly] float lastMoveTime = 0.0f;
    [SerializeField] public float attackInterval = 0.5f;

    public override void OnInit()
    {
        base.OnInit();
        property.attackInterval = attackInterval;
    }
    public override void OnSetInit(object[] value)
    {

    }

    private void Update()
    {
        Move();
        //Detection();
    }
    private void Detection()
    {
        var waplist = BattleSceneManager.Instance.lastDetectionWaps;
        //init
        foreach (var item in waplist)
        {
            item.SetMouseWap(0, 1);
        }
        waplist.Clear();

        //init
        var point = GetPoint();
        List<Vector2> detectionWaps = new List<Vector2>()
        {
            point + new Vector2(1, 0),
            point + new Vector2(-1, 0),
            point + new Vector2(0, 1),
            point + new Vector2(0, -1),
        };

        foreach (var item in detectionWaps)
        {
            if (BattleSceneManager.Instance.TryGetWap(item, out Wap wap))
            {
                waplist.Add(wap);
            }
        }

        foreach (var item in waplist)
        {
            item.SetMouseWap(1, 0.5f);
        }
    }

    private void Move()
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
                        switch (keyCode)
                        {
                            case KeyCode.W:
                                moveMode = BattleSceneManager.MoveMode.Top;
                                break;
                            case KeyCode.A:
                                moveMode = BattleSceneManager.MoveMode.Left;
                                break;
                            case KeyCode.S:
                                moveMode = BattleSceneManager.MoveMode.Down;
                                break;
                            case KeyCode.D:
                                moveMode = BattleSceneManager.MoveMode.Right;
                                break;
                            default:
                                break;
                        }
                        BattleSceneManager.Instance.SetWap(this, moveMode);
                    }
                }
            }
        }
    }

    public override void Active(params object[] value)
    {
        var enemy = (WapObjBase)value[0];
        var finish = (Action)value[1];
        DOTween.To(() => 2, value => { }, 0, attackInterval)
            .OnComplete(() => {
                if (BattleSceneManager.Instance.sceneMode != BattleSceneManager.SceneMode.Acttack)
                {
                    return;
                }
                var nowEnemyBlood = enemy.nowBlood;
                var data = BattleSceneManager.Instance.Date(this, enemy);
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
                    finish.Invoke();
                }
                Active(enemy, finish);
            });
    }

    public override void Die()
    {
        //ÓÎÏ·½áÊø
        BattleSceneManager.Instance.GameFinish();
        Destroy();
    }
}
