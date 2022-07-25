using UnityEngine;
using BXB.Core;
using System;
using System.Reflection;

public class StatusPattern : MiSingleton<StatusPattern>
{
    public ClassificationStatus GetStatus<T>(ClassificationStatus classification) where T : IStatusPattern
    {
        Type type;
        type = Type.GetType($"StatusPattern_{classification}");
        var getMethod = type.GetMethod("GetState", BindingFlags.Public | BindingFlags.NonPublic);

        var typeState = GetType();
        var parameter = typeState.GetField($"Instance.{classification}");
        if (parameter == null)
        {
            Log(color: Color.black, $"Instance Is Null");
        }
        if (getMethod == null)
        {
            Log(color: Color.black, $"GetState Is Null");
        }
        var ret = getMethod.Invoke(parameter,new object[] { });

        return (ClassificationStatus)ret;
    }

    public void SetStatus(ClassificationStatus classification, uint state)
    {
        Type type = Type.GetType($"StatusPattern_{classification}");
        var getMethod = type.GetMethod("SetState", BindingFlags.Instance | BindingFlags.Public);

        var typeState = GetType();
        var parClass = typeState.GetField($"{classification}", BindingFlags.NonPublic | BindingFlags.Instance);
        getMethod.Invoke(parClass.GetValue(this), new object[] { state });

    }
}
