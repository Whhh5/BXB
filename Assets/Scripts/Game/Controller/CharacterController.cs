using BXB.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : WapObjBase
{
    [SerializeField] float lastMoveTime = 0.0f;
    public override void OnInit()
    {
        
    }
    public override void OnSetInit(object[] value)
    {

    }

    private void Update()
    {
        Move();
        SetBarrier();
    }
    private void SetBarrier()
    {

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
}
