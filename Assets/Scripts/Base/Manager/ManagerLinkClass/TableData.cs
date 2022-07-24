using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using System;
using System.Threading.Tasks;

public class TableData : MiBaseClass
{
    Dictionary<ulong, GameObject> worldObjects = new Dictionary<ulong, GameObject>();
    Dictionary<ulong, MiUIDialog> uiDialogs = new Dictionary<ulong, MiUIDialog>();
    public T1 GetWorldObject<TTable, T1>(string f_path, ulong f_id, Vector3 f_startPosition, params object[] f_status)
        where TTable : class
        where T1 : MiObjPoolPublicParameter, ICommon_GameObject
    {
        var path = f_path;
        var id = f_id;
        var startPosition = f_startPosition;
        var status = f_status;
        #region new

        GetOriginal<TTable>(path, id, out GameObject original);
        if (original == null)
        {
            Log(color: Color.red, path + "/" + id);
            return null;
        }
        #endregion

        #region old

        //var data = DataManager.Master.GetTableData<TTable>(id);
        //if (data == null)
        //{
        //    Log(Color.red, $" Absent   Skills Id: {id} ");
        //    return null;
        //}
        //var prefabName = MiDataManager.Instance.master.LocalizeSkillsDataItem[id].prefabName;



        //var table = DataManager.Master.GetTableData<TTable>(id);
        //Type type = table.GetType();
        //var fieldInfo = type.GetField("prefabName");
        //if (fieldInfo == null)
        //{
        //    Log(Color.red, $"{type.Name} Absent 'prefabName' Parameter");
        //    return null;
        //}
        //string prefabName = (string)fieldInfo.GetValue(table);
        //GameObject original;
        //if (skills.ContainsKey(id))
        //{
        //    original = skills[id];
        //}
        //else
        //{
        //    original = MiResourcesManager.Instance.Load<GameObject>(path, $"{prefabName}");//CommonManager.Instance.filePath.PreComPath/Skills
        //    if (original != null)
        //    {
        //        skills.Add(id, original);
        //    }
        //    else
        //    {
        //        Log(Color.red, $" Absent   Prefab   Path   {path}/{prefabName}");
        //        return null;
        //    }
        //}
        #endregion
        var obj = ObjPool.GetObject(original);
        obj.transform.Normalization(null);
        obj.transform.position = startPosition;
        T1 result = obj.GetComponent<T1>();
        if (result != null)
        {
            obj.SetActive(true);
            result.GetMain().transform.Normalization(obj.transform);
            result.SettingId(id);
            result.OnInit();
            result.OnSetInit(status);
        }
        return result;
    }
    public async Task<T1> ShowUIDialog<TTable, T1>(string path, ulong id, CanvasLayer layerGroup, params object[] parameters)
        where TTable : class where T1 : MiUIDialog
    {
        T1 obj;
        if (!uiDialogs.ContainsKey(id))
        {
            //var prefabName = DataManager.Master.GetTableData<LocalizeUIDialogData>(id).prefabName;
            //var o = await ResourceManager.Instance.loadUIElementAsync<GameObject>(path, prefabName, layerGroup);
            //var dialog = o.GetComponent<MiUIDialog>();
            //uiDialogs.Add(id, dialog);
        }
        else if (uiDialogs[id] == null)
        {
            //var prefabName = DataManager.Master.GetTableData<LocalizeUIDialogData>(id).prefabName;
            //var o = await ResourceManager.Instance.loadUIElementAsync<GameObject>(path, prefabName, layerGroup);
            //var dialog = o.GetComponent<MiUIDialog>();
            //uiDialogs[id] = dialog;
        }
        obj = (T1)uiDialogs[id];
        obj.SettingId(id);
        obj.OnInit();
        obj.OnSetInit(parameters);
        await obj.OnSetInitAsync<TTable>(parameters);
        return obj;
    }

    public async Task<T1> GetUIElement<TTable, T1>(string f_path, ulong f_id, RectTransform f_parent, Vector3 f_anchor, params object[] f_parameter)
        where T1 : MiObjPoolPublicParameter, IUIElementPoolBase
        where TTable : class
    {
        var id = f_id;
        var path = f_path;
        var startPosition = f_anchor;
        var parameter = f_parameter;

        GetOriginal<TTable>(path, id, out GameObject original);
        if (original == null)
        {
            Log(color: Color.red, f_path + "/" + id);
            return null;
        }

        var obj = await ObjPool.GetObjectAsync(original);
        var rect = obj.GetComponent<RectTransform>();
        rect.Normalization(f_parent);
        rect.anchoredPosition3D = startPosition;
        T1 result = obj.GetComponent<T1>();
        if (result != null)
        {
            obj.SetActive(true);
            result.GetMain().GetComponent<RectTransform>().Normalization(rect);
            result.SettingId(id);
            result.OnInit();
            result.OnSetInit(parameter);
            await result.OnSetInitAsync<TTable>(parameter);
        }
        return result;
    }

    public void GetOriginal<TTable>(string path, ulong id, out GameObject original)
        where TTable : class
    {
        original = null;
        var data = DataManager.Master.GetTableData<TTable>(id);
        if (data == null)
        {
            Log(Color.red, $" Absent   Skills Id: {id} ");
            return;
        }
        var table = DataManager.Master.GetTableData<TTable>(id);
        Type type = table.GetType();
        var fieldInfo = type.GetField("prefabName");
        if (fieldInfo == null)
        {
            Log(Color.red, $"{type.Name} Absent 'prefabName' Parameter");
            return;
        }
        string prefabName = (string)fieldInfo.GetValue(table);

        if (worldObjects.ContainsKey(id))
            if (worldObjects[id] != null)
                original = worldObjects[id];
            else
            {
                original = ResourceManager.Instance.Load<GameObject>(path, $"{prefabName}");
                if (original != null)
                    worldObjects[id] = original;
                else
                    Log(Color.red, $" Absent   Prefab   Path   {path}/{prefabName}");
            }
        else
        {
            original = ResourceManager.Instance.Load<GameObject>(path, $"{prefabName}");
            if (original != null)
                worldObjects.Add(id, original);
            else
                Log(Color.red, $" Absent   Prefab   Path   {path}/{prefabName}");
        }
    }







}