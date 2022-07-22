using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using System;

public class CameraController : MiBaseMonoBeHaviourClass
{
    enum Driction
    {
        Left,
        Right,
        Top,
        Down,
    }
    [SerializeField] LayerMask mouse0RayMask;
    [SerializeField] Camera thisCamera;
    [SerializeField] Vector3 targetOffset = new Vector3();
    [SerializeField] SpriteRenderer mapSprote;
    [SerializeField] Vector3 maxDriction = new Vector3(50, -50);

    protected override void OnAwake()
    {
        base.OnAwake();
        thisCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        try
        {
            var cameraSize = thisCamera.orthographicSize;

            var pos = thisCamera.transform.position;
            //maxDriction = mapSprote.bounds.size;

            var endPos = new Vector3();
            var player = SceneDataManager.Instance.mainPlayer;
            endPos += player.transform.position + targetOffset;


            var leftTop = new Vector3(cameraSize * 2, -cameraSize, pos.z);
            var rightDown = new Vector3(maxDriction.x - cameraSize * 2, maxDriction.y + cameraSize, pos.z);
            //mapPos += pos;





            endPos.x = endPos.x < leftTop.x ? leftTop.x : endPos.x;
            endPos.x = endPos.x > rightDown.x ? rightDown.x : endPos.x;

            endPos.y = endPos.y > leftTop.y ? leftTop.y : endPos.y;
            endPos.y = endPos.y < rightDown.y ? rightDown.y : endPos.y;

            transform.position = Vector3.Lerp(transform.position, endPos, 1f);
        }
        catch (Exception)
        {
        }
    }
}
