using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Reflection;
using System;
using BXB.Core;

public partial class MasterData
{
	public static MasterData Instance = new MasterData();
	/// <summary>
	/// T is table type
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="key"></param>
	/// <returns></returns>
	public T GetTableData<T>(ulong key) where T:class
	{
		var table = typeof(T).Name;
		var type = GetType().GetField(table, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        if (type == null)
        {
			Debug.Log($"<color=#FF0000> Absent, Please Check Script Exists,  Return Null </color>");
            return null;
        }
		var typeValue = type.GetValue(this); 
        if (typeValue == null)
        {
			Debug.Log($"<color=#FF0000>Parameter : {table}, Absent, Please Check MasterData Exists Parameter,  Return Null</color>");
            return null;
        }
		var getValueMethod = typeValue.GetType().GetMethod("TryGetValue");
		var getContenMethod = typeValue.GetType().GetMethod("ContainsKey");

		if (!(bool)getContenMethod.Invoke(typeValue, new object[] { key }))
        {
			Debug.Log($"<color=#FF0000> Table : {table}  Absent Key: {key} </color>");
            return null;
        }
		var obj = Activator.CreateInstance(Type.GetType(table.ToString()));
		var parameters = new object[] { key, obj };
		getValueMethod.Invoke(typeValue, parameters);
		obj = parameters[1];

		return (T)obj;
	}

}