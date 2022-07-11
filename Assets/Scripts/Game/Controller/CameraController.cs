using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using System;

public class CameraController : MiBaseMonoBeHaviourClass
{
    [SerializeField] LayerMask mouse0RayMask;
    protected override void OnAwake()
    {
        base.OnAwake();
        //var table = MasterData.Instance.GetTableData<LocalItemData>(120010001);
        //Log(Color.green, table.name);
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    var hit2d = Physics2D.Raycast(BattleSceneManager1.Instance.sceneMainCamera.transform.position, Input.mousePosition);
        //    if (hit2d.collider)
        //    {
        //        if (hit2d.collider.TryGetComponent(out Wap wap))
        //        {
        //            Log(Color.green, wap.GetPoint());
        //            if (wap.TryGetObject(out CharacterController charController))
        //            {
        //                Log(Color.green, charController.name);
        //            }
        //        }
        //    }
        //}
    }
}
