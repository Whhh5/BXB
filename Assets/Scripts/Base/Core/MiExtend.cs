using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using System.Reflection;

namespace BXB
{
    namespace Core
    {
        public static class MiExtend
        {
            #region  SubscribeEventAsync
            public static IDisposable SubscribeEventAsync(this UnityEvent events, Func<Task> func)
            {
                events.AddListener(() => func.Invoke());
                return events as IDisposable;
            }
            public static IDisposable SubscribeEventAsync<T0>(this UnityEvent<T0> events, Func<T0, Task> func)
            {
                events.AddListener((T0) => func.Invoke(T0));
                return events as IDisposable;
            }
            public static IDisposable SubscribeEventAsync<T0, T1>(this UnityEvent<T0, T1> events, Func<T0, T1, Task> func)
            {
                events.AddListener((t0, t1) => func.Invoke(t0, t1));
                return events as IDisposable;
            }
            public static IDisposable SubscribeEventAsync<T0, T1, T2>(this UnityEvent<T0, T1, T2> events, Func<T0, T1, T2, Task> func)
            {
                events.AddListener((t0, t1, t2) => func.Invoke(t0, t1, t2));
                return events as IDisposable;
            }
            #endregion
            public static void SubscribeGC(this IDisposable disposable, int value)
            {
                //Debug.Log($"{value.ToString()}");
            }

            public static RectTransform Normalization(this RectTransform rect, RectTransform parent)
            {
                rect.SetParent(parent);
                rect.anchoredPosition3D = Vector3.zero;
                rect.localRotation = Quaternion.Euler(Vector3.zero);
                rect.localScale = Vector3.one;
                return rect;
            }
            public static Transform Normalization(this Transform tr, Transform parent)
            {
                tr.SetParent(parent);
                tr.localPosition = Vector3.zero;
                tr.localRotation = Quaternion.Euler(Vector3.zero);
                tr.localScale = Vector3.one;
                return tr;
            }
            public static Dictionary<ulong, TDicValue> LoadData<TDicValue, TDataItem>(this Dictionary<ulong, TDicValue> dic, string assetName)
                where TDataItem : UnityEngine.Object, IDataItemMothodBase
            {
                string path = $"{CommonManager.Instance.filePath.ResLocalDataPath}/{assetName}.asset";
                var asset = AssetDatabase.LoadAssetAtPath<TDataItem>(path);
                Dictionary<ulong, TDicValue> tempDic = new Dictionary<ulong, TDicValue>();
                if (asset != null)
                {
                    Type type;
                    FieldInfo id;
                    var dataList = asset.GetData();
                    if (dataList != null)
                    {
                        foreach (var parameter in dataList)
                        {
                            type = parameter.GetType();
                            id = type.GetField("id");
                            tempDic.Add((ulong)id.GetValue(parameter), (TDicValue)parameter);
                        }
                    }
                }
                return tempDic;
            }
        }
    }
}
