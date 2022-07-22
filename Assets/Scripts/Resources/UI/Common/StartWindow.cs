using UnityEngine.SceneManagement;
using UnityEngine;
using System.Threading.Tasks;
using TMPro;
using BXB.Core;

public class StartWindow : MiUIDialog
{
    [SerializeField] MiUIButton StartButton;
    [SerializeField] MiUIButton LeaveButton;
    protected override async Task OnAwakeAsync()
    {
        await base.OnAwakeAsync();

        StartButton.onClick.SubscribeEventAsync(async () =>
        {
            await MainSceneManager.Instance.GameStart();
            Destroy();
            ResourceManager.Instance.LoadSceneAsync(ResourceManager.SceneMode.LevelSelect, LoadSceneMode.Additive);
        });
        //退出响应时间
        LeaveButton.onClick.SubscribeEventAsync(async () =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
        });

    }
    public override void OnInit()
    {
        gameObject.SetActive(true);
    }
    public override void OnSetInit(object[] value)
    {

    }
    public override async Task OnSetInitAsync<T>(params object[] value)
    {
        await AsyncDefaule();
    }
}