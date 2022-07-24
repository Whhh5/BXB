using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using System;
using System.Text;
using DG.Tweening;

public class MapWapController : MiBaseClass
{
    public void CreateMapWap(int wapUnit, Vector2 mapWidthAndHeight, Dictionary<Vector2, Wap> pointToWap, Transform parent)
    {
        for (int i = 0; i < mapWidthAndHeight.x; i++)
        {
            for (int j = 0; j < mapWidthAndHeight.y; j++)
            {
                var startPos = new Vector3(j * (wapUnit) + wapUnit * 0.5f, -i * (wapUnit) - wapUnit * 0.5f, 0);
                var path = CommonManager.Instance.filePath.ResArticle;
                var wap = ResourceManager.Instance.GetWorldObject<Wap>(path, "Wap", startPos, parent, 0, new Vector3(wapUnit, wapUnit, wapUnit), new Vector2(i, j));
                var point = new Vector2(i, j);
                wap.SetPoint(point);
                pointToWap.Add(point, wap);
            }
        }
        Log(Color.green, 1 << 80, (1 << 95), (1 << 31), 1 << 10);
    }

    public void PlaceArticle(WapObjBase obj, Vector2 toPoint, Dictionary<Vector2, Wap> pointToWap, float time, Ease ease)
    {
        try
        {
            var allPoint = obj.GetAllPoint();

            foreach (var item in allPoint)
            {
                if (!(item.x < 0 || item.y < 0))
                {
                    var oldWap = pointToWap[item];
                    if (oldWap.TryGetObject(out Transform oldObj))
                    {
                        if (oldObj.gameObject == obj.gameObject)
                        {
                            oldWap.SetArticle(null);
                        }
                    }
                }
            }
            var oWap = pointToWap[toPoint];
            obj.SetPont(toPoint);
            var newWwaps = new List<Wap>() { pointToWap[toPoint] };
            var exetend = obj.GetExtendPoint();
            foreach (var item in exetend)
            {
                newWwaps.Add(pointToWap[item + toPoint]);
            }
            foreach (var wap in newWwaps)
            {
                wap.SetArticle(obj.gameObject);
            }
            var endPostion = obj.transform.position;
            endPostion.x = oWap.transform.position.x;
            endPostion.y = oWap.transform.position.y;
            obj.transform.DOMove(endPostion, time, false).SetEase(ease: ease);
            //var wap = pointToWap[point];
            //var oldPoint = obj.GetPoint();
            //if (!(oldPoint.x < 0 || oldPoint.y < 0))
            //{
            //    var oldWap = pointToWap[oldPoint];
            //    if (oldWap.TryGetObject(out Transform oldObj))
            //    {
            //        if (oldObj.gameObject == obj.gameObject)
            //        {
            //            oldWap.SetArticle(null);
            //        }
            //    }
            //}
            //obj.SetPont(point);
            //wap.SetArticle(obj.gameObject);
            //var endPostion = obj.transform.position;
            //endPostion.x = wap.transform.position.x;
            //endPostion.y = wap.transform.position.y;
            //obj.transform.DOMove(endPostion, time, false).SetEase(ease: ease);
        }
        catch (Exception)
        {
        }
    }
}
