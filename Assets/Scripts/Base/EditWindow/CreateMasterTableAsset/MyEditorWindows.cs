using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Reflection;
using System.Threading.Tasks;

public partial class MyEditorWindows : EditorWindow
{
    [MenuItem("My Tools Group/Setting Asset Group")]
    static void ShowMyWindow()
    {
        MyEditorWindows myWindow = EditorWindow.GetWindow<MyEditorWindows>();
        myWindow.InitializationDefaultPath();
        myWindow.Show();
    }
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;
    GUIStyle style1 = new GUIStyle();




    public void InitializationDefaultPath()
    {
        pathSplit = $"{Path.DirectorySeparatorChar}";
        localPath = $"{Application.dataPath}{pathSplit}";
        masterDataPath = $"{localPath}Master Data";
        scriptPath = $"{localPath}Scripts{pathSplit}Assets{pathSplit}Script";
        assetPath = $"Scripts{pathSplit}Assets{pathSplit}Assets";
    }
    static string pathSplit = "";
    static string localPath = "";
    static string masterDataPath = "";
    static string scriptPath = "";
    static string assetPath = "";
    static string masterDataAssetName = $"MasterDataAsset";
    static string masterData = $"MasterData";


    static string assetMasterDataPath = "";
    static string assetScriptPath = "";
    static string assetAssetPath = "";
    static string assetMasterDataAssetName = "";
    static string assetMasterData = "";

    public static string getMasterDataPath
    {
        get
        {
            string value = assetMasterDataPath;
            if (Equals(assetMasterDataPath, string.Empty))
            {
                value = masterDataPath;
            }
            return value;
        }
    }
    public static string getScriptPath
    {
        get
        {
            string value = assetScriptPath;
            if (Equals(assetScriptPath, string.Empty))
            {
                value = scriptPath;
            }
            return value;
        }
    }
    public static string getAssetPath
    {
        get
        {
            string value = assetAssetPath;
            if (Equals(assetAssetPath, string.Empty))
            {
                value = assetPath;
            }
            return value;
        }
    }
    public static string getMasterDataAssetName
    {
        get
        {
            string value = assetMasterDataAssetName;
            if (Equals(assetMasterDataAssetName, string.Empty))
            {
                value = masterDataAssetName;
            }
            return value;
        }
    }
    public static string getMasterData
    {
        get
        {
            string value = assetMasterDataAssetName;
            if (Equals(assetMasterDataAssetName, string.Empty))
            {
                value = masterData;
            }
            return value;
        }
    }

    public static string GetAssetPath()
    {
        return "";
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        //EditorGUILayout.BeginVertical("Default Path Box");
        {
            EditorGUILayout.LabelField("pathSplit", pathSplit);
            EditorGUILayout.LabelField("localPath", localPath);
            EditorGUILayout.LabelField("masterDataPath", masterDataPath);
            EditorGUILayout.LabelField("scriptPath", scriptPath);
            EditorGUILayout.LabelField("assetPath", assetPath);
            EditorGUILayout.LabelField("masterDataAssetsName", masterDataAssetName);
            EditorGUILayout.LabelField("dataManager", masterData);
            if (GUILayout.Button("Initialization Default Path"))
            {
                InitializationDefaultPath();
            }

            if (GUILayout.Button("Open Console Window"))
            {
                EditorMessageWindow messageWindow = EditorWindow.GetWindow<EditorMessageWindow>();//创建自定义窗口
                messageWindow.Initialization();
                messageWindow.Show();//显示创建的自定义窗口
            }
        }
        //EditorGUILayout.EndVertical();

        #region Path Box

        //EditorGUILayout.BeginVertical("Path Box");
        //{
        //    EditorGUILayout.BeginHorizontal("Default");
        //    {
        //        assetMasterDataAssetName = EditorGUILayout.TextField("MasterDataAssetName", assetMasterDataAssetName);

        //        var defaultButton = GUILayout.Button("Defalt");
        //        if (defaultButton)
        //        {
        //            assetMasterDataAssetName = masterDataAssetName;
        //        }
        //    }
        //    EditorGUILayout.EndHorizontal();

        //    EditorGUILayout.BeginHorizontal("Default");
        //    {
        //        assetMasterData = EditorGUILayout.TextField("MasterData", assetMasterData);

        //        var defaultButton2 = GUILayout.Button("Defalt");
        //        if (defaultButton2)
        //        {
        //            assetMasterData = masterData;
        //        }
        //    }
        //    EditorGUILayout.EndHorizontal();

        //    EditorGUILayout.BeginHorizontal("Default1");
        //    {
        //        assetMasterDataPath = EditorGUILayout.TextField("MasterDataPath", assetMasterDataPath);
        //        var defaultButton = GUILayout.Button("Defalt");
        //        if (defaultButton)
        //        {
        //            assetMasterDataPath = masterDataPath;
        //        }
        //    }
        //    EditorGUILayout.EndHorizontal();

        //    EditorGUILayout.BeginHorizontal("Default2");
        //    {
        //        assetScriptPath = EditorGUILayout.TextField("ScriptPath", assetScriptPath);

        //        var defaultButton = GUILayout.Button("Defalt");
        //        if (defaultButton)
        //        {
        //            assetScriptPath = scriptPath;
        //        }
        //    }
        //    EditorGUILayout.EndHorizontal();

        //    EditorGUILayout.BeginHorizontal("Default3");
        //    {
        //        assetAssetPath = EditorGUILayout.TextField("AssetPath", assetAssetPath);
        //        var defaultButton = GUILayout.Button("Defalt");
        //        if (defaultButton)
        //        {
        //            assetAssetPath = assetPath;
        //        }
        //    }
        //    EditorGUILayout.EndHorizontal();
        //}
        //EditorGUILayout.EndVertical();
        #endregion

        //EditorGUILayout.BeginHorizontal("");
        //GUILayout.BeginHorizontal("");
        {
            if (GUILayout.Button("Create Floders And Parameter Script"))
            {
                CreateDataItem.CreateFlodersAndScript();
            }
            if (GUILayout.Button("Create Asset Item And MasterData Script"))
            {
                CreateDataItem.CeateAssetAndMasterData();
            }
        }
        //EditorGUILayout.EndHorizontal();

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        if (groupEnabled)
        {
            myBool = EditorGUILayout.Toggle("Toggle", myBool);
            myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        }
        EditorGUILayout.EndToggleGroup();
    }
}

public class EditorMessageWindow : EditorWindow
{
    string messages = "";
    GUIStyle style = new GUIStyle();
    public void AddMessage(string message)
    {
        messages += $"{message}\n";
    }
    public void CloseMessage()
    {
        messages = "";
    }
    private void OnGUI()
    {
        GUILayout.TextArea(messages, style);
    }
    public void Initialization()
    {
        //style.size
    }
}


public class CreateDataItem
{
    static string pathSplit = $"{Path.DirectorySeparatorChar}";
    static string localPath = $"{Application.dataPath}{pathSplit}";
    static string masterDataPath = $"{localPath}MasterTables";
    static string scriptPath = $"{localPath}Scripts{pathSplit}Assets{pathSplit}Script";
    static string assetPath = $"Scripts{pathSplit}Assets{pathSplit}Assets";
    static string masterDataAssetName = $"MasterDataAsset";
    static string masterData = $"MasterData";

    //[MenuItem("Create/Create Floder And Script", priority = 1)]
    public static void CreateFlodersAndScript()
    {
        CreateDirectory();
        CreateItemScript();
        CreateAssetScript();
    }
    //[MenuItem("Create/Create Asset", priority = 2)] // 生成Asset文件
    public static void CeateAssetAndMasterData()
    {
        CreateAsset();
        CreateDataManagerScrite();
    }
    //[MenuItem("Create/Path", priority = 1)]
    public static void CreateDirectory()
    {
        if (Directory.Exists(scriptPath))
        {
            Directory.Delete(scriptPath, true);
        }
        if (Directory.Exists($"{localPath}{pathSplit}{assetPath}"))
        {
            Directory.Delete($"{localPath}{pathSplit}{assetPath}", true);
        }

        Directory.CreateDirectory(scriptPath);
        Directory.CreateDirectory(scriptPath);
        Directory.CreateDirectory($"{localPath}{pathSplit}{assetPath}");
    }
    //[MenuItem("Create/Asset ItemScript", priority = 2)] // 生成属性脚本
    public static void CreateItemScript()
    {
        string[] allFilesPath = Directory.GetFileSystemEntries(masterDataPath, "*.txt", SearchOption.TopDirectoryOnly);

        foreach (var filePath in allFilesPath)
        {
            var file = File.ReadAllText(filePath);
            var lines = file.Split('\n');

            string itemName = "";
            List<string> itemType = new List<string>();
            List<string> itemParameter = new List<string>();

            foreach (var line in lines)
            {
                switch (line[0])
                {
                    case 'C':
                        TypeC(ref itemName, line);
                        break;
                    case 'T':
                        TypeT(itemType, line);
                        break;
                    case 'H':
                        TypeH(itemParameter, line);
                        break;
                    case 'I':
                        TypeI(line);
                        break;
                    default:
                        break;
                }
            }
            if (itemName == string.Empty)
            {
                Debug.Log($"<color=#FF0000>Class Is Nont A Null</color>");
                return;
            }
            int i = 0, j = 0;
            while (i < itemType.Count || j < itemParameter.Count)
            {
                string key = "", value = "";
                if (i < itemType.Count)
                {
                    key = itemType[i++];
                }
                if (j < itemParameter.Count)
                {
                    value = itemParameter[j++];
                }
                if (key == string.Empty || value == string.Empty)
                {
                    Debug.Log($"<color=#FF0000>Parameter Or Type Is Not Null: {itemName} {i} {j}  key: {key} Value: {value}</color>");
                    return;
                }
            }
            if (itemType.Count != itemParameter.Count || itemParameter.Count == 0)
            {
                Debug.Log($"<color=#FF0000>None Parameter And Type</color>");
            }

            string parameters = "";
            for (i = 0; i < itemType.Count; i++)
            {
                parameters += string.Format(TemplateClass.parameter, itemType[i], itemParameter[i]) + "\n";

                //TemplateAssetClass.CreateParameterAndScript(TemplateAssetClass.TemplateFlags.parameter, itemType[i], itemParameter[i]);
            }
            parameters = parameters.Substring(0, parameters.Length - 1);
            var cs = TemplateClass.CreateAssetScript(itemName, parameters);
            //var cs = TemplateAssetClass.CreateParameterAndScript( TemplateAssetClass.TemplateFlags.commonClass, itemName, parameters);
            //生成 *.cs 文件
            var filesPath = scriptPath + $"/{itemName}.cs";
            File.WriteAllText(filesPath, cs);
        }

        #region localization Method
        void TypeC(ref string itemName, string type)
        {
            string[] name = type.Split('\t');
            if (1 < name.Length && name[1] != string.Empty)
            {
                itemName = name[1];
            }
        }
        void TypeT(List<string> itemType, string type)
        {
            string[] types = type.Split('\t');
            for (int i = 1; i < types.Length; i++)
            {
                types[i] = types[i].Replace(" ", "");
                itemType.Add(types[i]);
            }
        }
        void TypeH(List<string> itemParameter, string names)
        {
            string[] types = names.Split('\t');
            for (int i = 1; i < types.Length; i++)
            {
                types[i] = types[i].Replace(" ", "");
                itemParameter.Add(types[i]);
            }
        }
        void TypeI(string type)
        {

        }
        #endregion
    }
    //[MenuItem("Create/Assset Script", priority = 3)] // 生成Asset脚本
    public static async void CreateAssetScript()
    {
        string paraTemplate = "public List<{0}> {0} = new List<{0}>();";
        string assetParameter = "";

        DirectoryInfo directoryInfo = new DirectoryInfo(scriptPath);
        var files = directoryInfo.GetFiles("*.cs", SearchOption.TopDirectoryOnly);
        foreach (var file in files)
        {
            var name = file.Name.Substring(0, file.Name.LastIndexOf('.'));

            assetParameter += string.Format(paraTemplate, name) + '\n';
        }
        var cs = TemplateClass.CreateAsset($"{masterDataAssetName}", assetParameter);
        //var cs = TemplateAssetClass.CreateParameterAndScript( TemplateAssetClass.TemplateFlags.asset, $"{masterDataAssetsName}", assetParameter);
        Directory.CreateDirectory(scriptPath);
        await Task.Run(() => File.WriteAllText($"{scriptPath}" + $"{pathSplit}{masterDataAssetName}.cs", cs));

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    public static void CreateAsset()
    {
        Type type = Type.GetType(masterDataAssetName);
        object itemTypeObj = Activator.CreateInstance(type);
        AssetDatabase.CreateAsset((UnityEngine.Object)itemTypeObj, $"Assets{pathSplit}{assetPath}{pathSplit}{masterDataAssetName}.asset");
        GiveAssetsAssignments();
    }
    public static void GiveAssetsAssignments()
    {
        var filesPath = Directory.EnumerateFiles(masterDataPath, "*.txt", SearchOption.TopDirectoryOnly);
        Type assetType = Type.GetType(masterDataAssetName);
        var mainAsset = AssetDatabase.LoadAssetAtPath($"Assets{pathSplit}{assetPath}{pathSplit}{masterDataAssetName}.asset", assetType);

        foreach (var filePath in filesPath)
        {
            var file = File.ReadAllText(filePath);
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
                continue;
            }
            List<Task> tasks = new List<Task>();
            foreach (var line in lines)
            {
                //object[] objs =
                //{
                //    line,
                //    chilTableName,
                //    paraName.ToArray(),
                //    paraType.ToArray(),
                //};
                //tasks.Add(new Task((objs) =>
                //{
                //    var obj = (object[])objs;
                //    var line = (string)obj[0];
                //    var chilTableName = (string)obj[1];
                //    var arr2 = (object[])obj[2];
                //    var arr3 = (object[])obj[2];
                //    List<string> paraName = new List<string>((string[])arr2);
                //    List<string> paraType = new List<string>((string[])arr3);

                string[] parameter = line.Split('\t');
                if (!Equals(parameter?[0], string.Empty))
                {
                    continue;
                }
                Dictionary<string, object> name_Value = new Dictionary<string, object>();
                for (int i = 1; i < parameter.Length; i++)
                {
                    object paras;
                    if (parameter[i].Contains(";") || string.Equals( paraType[i], "string[]") || string.Equals(paraType[i], "List<*>"))
                    {
                        var list = parameter[i].Split(';');
                        List<string> temporlateList = new List<string>();
                        foreach (var item in list)
                        {
                            if (!string.Equals(item,string.Empty))
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
                var assetParameter = assetType.GetField(chilTableName).GetValue(mainAsset); // list<cs>
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

                string str = "";
                var Fields = data.GetType().GetFields();
                foreach (var para in Fields)
                {
                    str += para.GetValue(data).ToString() + '\t';
                }
                //Debug.Log(str);

                var assetParaAddMethod = assetParameter.GetType().GetMethod("Add");
                assetParaAddMethod.Invoke(assetParameter, new object[] { data });
                //}, objs));
            }
            //foreach (var task in tasks)
            //{
            //    task.Start();
            //}
        }
    }

    //[MenuItem("Create/Create DataManager Script", priority = 3)]
    public static void CreateDataManagerScrite()
    {
        string primaryTemplate = TemplateClass.dictionary;
        string className = masterData;
        string dataManagerSubject = "";

        var asset = AssetDatabase.LoadAssetAtPath($"Assets{pathSplit}{assetPath}{pathSplit}{masterDataAssetName}.asset", Type.GetType(masterDataAssetName));
        var fields = asset.GetType().GetFields();

        foreach (var field in fields)
        {
            //var assetParameter = field.GetValue(asset);
            //var assetParameterType = Type.GetType(field.Name);

            var idType = "ulong";
            dataManagerSubject += string.Format(primaryTemplate, idType, field.Name) + '\n';
        }

        var csTemplate = TemplateClass.partialClass;

        csTemplate = string.Format(csTemplate, '{', '}', className, dataManagerSubject);
        File.WriteAllText($"{scriptPath}{pathSplit}{className}.cs", csTemplate);

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
    //[MenuItem("Create/Create DataManager", priority = 3)]
    [RuntimeInitializeOnLoadMethod]
    public static void AddDataManagerData()
    {
        CeateAssetAndMasterData();

        string className = masterData;

        var asset = AssetDatabase.LoadAssetAtPath($"Assets{pathSplit}{assetPath}{pathSplit}{masterDataAssetName}.asset", Type.GetType(masterDataAssetName));
        var fields = asset.GetType().GetFields();
        var dataManagerType = Type.GetType(className);

        var dataInstance = dataManagerType.GetField("Instance", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public).GetValue(dataManagerType);
        foreach (var field in fields)
        {
            var assetParameter = field.GetValue(asset);
            var assetParameterType = assetParameter.GetType();
            var assetParaList = assetParameterType.GetMethod("ToArray");
            var paraArray = assetParaList.Invoke(assetParameter, new object[] { });


            var dataParameter = dataManagerType.GetField(field.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance);
            //var fils = dataManagerType.GetFields();

            var dataParameterValue = dataParameter.GetValue(dataInstance);
            var dataParameterType = dataParameterValue.GetType();
            var addMethod = dataParameterType.GetMethod("Add");

            var list = (object[])paraArray;
            foreach (var parameter in list)
            {
                var type = parameter.GetType();
                var id = type.GetField("id").GetValue(parameter);
                try
                {
                    addMethod.Invoke(dataParameterValue, new object[] { id, parameter });
                }
                catch (Exception)
                {
                    Debug.Log($"Error: Already Dictionary:  {type}  {id}");
                }
            }
        }
    }
}