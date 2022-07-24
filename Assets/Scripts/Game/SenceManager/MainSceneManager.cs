using BXB.Core;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using LitJson;

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
    }

    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override async Task OnStartAsync()
    {
        await base.OnStartAsync();
        //DontDestroyOnLoad(gameObject);
        ResourceManager.Instance.LoadSceneAsync( ResourceManager.SceneMode.UI, mode: LoadSceneMode.Additive);
    }

    public async Task GameStart()
    {
        await AsyncDefaule();

    }
}
