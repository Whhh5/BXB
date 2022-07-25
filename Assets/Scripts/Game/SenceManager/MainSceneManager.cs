using BXB.Core;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using TMPro;

public class MainSceneManager : MiSingletonMonoBeHaviour<MainSceneManager>
{
    public Camera mainCamera;
    protected override void OnAwake()
    {
        base.OnAwake();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    protected override void Start()
    {
        base.Start();

        JsonData jsonData = new JsonData()
        {
            ["qwe"] = 1,
            ["s"] = "qwe",
        };
        foreach (var item in jsonData)
        {
            //var data = (JsonData)item;
            //Log(Color.green, data.ToJson(), item, data);

            //Log(Color.green, item);
        }

        //var tablePath = Application.dataPath + "/Resources/MasterTables";
        //var filesPath = Directory.EnumerateFiles(tablePath, "*.txt", SearchOption.TopDirectoryOnly);
        List<string> filesPath = new List<string>
        {
            "MasterTables/LocalItemData",
            "MasterTables/LocalPropertyData",
            "MasterTables/LocalRolesData",
            "MasterTables/LocalRolesLevelData",
        };

        foreach (var filePath in filesPath)
        {
            //var file = File.ReadAllText(filePath);
            var file = Resources.Load<TextAsset>(filePath).ToString();
            file = file.Replace("\r", "");
            var lines = file.Split('\n');
            string chilTableName = "";
            List<string> paraType = new List<string>();
            List<string> paraName = new List<string>();
            foreach (var line in lines)
            {
                if (Equals(line[0], 'C'))
                {
                    chilTableName = line.Split('\t')[1];
                    continue;
                }
                if (Equals(line[0], 'T'))
                {
                    paraType = new List<string>(line.Split('\t'));
                }
                if (Equals(line[0], 'H'))
                {
                    paraName = new List<string>(line.Split('\t'));
                }
            }
            if (paraType.Count != paraName.Count && paraType.Count == 0 && chilTableName == string.Empty)
            {
                Debug.Log($"<color=#FF0000>Error: paraType and paraName number not empty  or  csName is null</color>");
                //continue;
            }
            List<Task> tasks = new List<Task>();
            foreach (var line in lines)
            {
                string[] parameter = line.Split('\t');
                if (!Equals(parameter?[0], string.Empty))
                {
                    continue;
                }
                Dictionary<string, object> name_Value = new Dictionary<string, object>();
                for (int i = 1; i < parameter.Length; i++)
                {
                    object paras;
                    if (parameter[i].Contains(";") || string.Equals(paraType[i], "string[]") || string.Equals(paraType[i], "List<*>"))
                    {
                        var list = parameter[i].Split(';');
                        List<string> temporlateList = new List<string>();
                        foreach (var item in list)
                        {
                            if (!string.Equals(item, string.Empty))
                            {
                                temporlateList.Add(item);
                            }
                        }
                        paras = temporlateList.ToArray() as object;
                    }
                    else
                    {
                        paras = parameter[i] as object;
                    }
                    //object paras = parameter[i].Split(';').Length <= 1 ? parameter[i] as object : parameter[i].Split(';') as object;
                    name_Value.Add(paraName[i], paras);
                }
                var dataType = Type.GetType(chilTableName);
                var data = Activator.CreateInstance(dataType);

                foreach (var assetPara in name_Value)
                {
                    var field = dataType.GetField(assetPara.Key);
                    Type fieldType = null;
                    object changeType = assetPara.Value;
                    if (field.GetValue(data) != null)
                    {
                        fieldType = field.GetValue(data).GetType();
                    }
                    if (fieldType != null)
                    {
                        changeType = Convert.ChangeType(assetPara.Value, fieldType);
                    }
                    else
                    {
                        //Debug.Log($"{assetPara.Key}   {assetPara.Value}");
                    }
                    field.SetValue(data, changeType);
                }

                //string str = "";
                //var Fields = data.GetType().GetFields();
                //foreach (var para in Fields)
                //{
                //    str += para.GetValue(data).ToString() + '\t';
                //}
                var field_ = MasterData.Instance.GetType().GetField(chilTableName);
                var mathod = field_.GetValue(MasterData.Instance).GetType().GetMethod("Add");
                var data2 = field_.GetValue(MasterData.Instance);
                var id = data.GetType().GetField("id").GetValue(data);
                mathod.Invoke(data2, new object[] { id, data });
            }
        }
      




        ResourceManager.Instance.LoadSceneAsync( ResourceManager.SceneMode.UI, mode: LoadSceneMode.Additive);
    }

    protected override void OnStart()
    {
        base.OnStart();


    }

    protected override async Task OnStartAsync()
    {
        await base.OnStartAsync();
        //DontDestroyOnLoad(gameObject);

    }

    public async Task GameStart()
    {
        await AsyncDefaule();

    }
}
