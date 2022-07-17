using UnityEngine;
using BXB.Core;
using System;

public class MiDataProcessing : MiBaseClass
{
    public Action<BaseGameObject_Game> CharacterBloodEvent = (x) => { };
    public void BloodChange(float value, BaseGameObject_Game obj, Vector3 point = default, HarmType type = HarmType.Remove)
    {
        obj.AddBloodValue(value * ((int)type));
        Color color = Color.gray;
        if (point == default)
        {
            point = obj.transform.position;
        }
        switch (type)
        {
            case HarmType.Noen:
                ColorUtility.TryParseHtmlString("#FFFFFFCC", out color);
                break;
            case HarmType.Add:
                ColorUtility.TryParseHtmlString("#CCFF00CC", out color);
                break;
            case HarmType.Remove:
                ColorUtility.TryParseHtmlString("#FF7F00CC",out color);
                break;
            default:
                break;
        }
        CharacterBloodEvent.Invoke(obj);
        return;
        //var path = CommonManager.Instance.filePath.PreComEffectsPath;
        //var setBoolDialog = TableManager.Instance.tableData.GetWorldObject<LocalizeSkillsData, CommonAnimationHint>(
        //    f_path: path, f_id: 10202040002, point, value.ToString(), color);
        //setBoolDialog.Active();
    }

    public float AttackInterval(float number)
    {
        float ret;
        ret = 1.0f / number;
        ret = ret < 0 ? 0 : ret;
        return ret;
    }

    public float AttackData(WapObjBase obj1, WapObjBase obj2)
    {
        float ret = obj1.GetSet(WapObjBase.PropertyFloat.attack) - obj2.GetSet(WapObjBase.PropertyFloat.defend);
        ret = ret <= 0 ? 1 : ret;
        return ret;
    }
    public float GetStartAttackInterval(float interval)
    {
        var rang = UnityEngine.Random.Range(0.1f, 0.3f);
        return interval * rang;
    }
}
