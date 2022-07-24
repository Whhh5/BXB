using UnityEngine.SceneManagement;
using UnityEngine;
using System.Threading.Tasks;
using TMPro;
using BXB.Core;
using UnityEngine.UI;

public class StartWindow : MiUIDialog
{
    [SerializeField] MiUIButton StartButton;
    [SerializeField] MiUIButton LeaveButton;
    [SerializeField] MiUIButton SettingsButton;
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
            var path = CommonManager.Instance.filePath.PreUIDialogSystemPath;
            await ResourceManager.Instance.ShowDialogAsync<MiUIDialog>(path, "LeaveCheckWindow", CanvasLayer.System);

//#if UNITY_EDITOR
//            UnityEditor.EditorApplication.isPlaying = false;
//#else
//                Application.Quit();
//#endif
        });

        SettingsButton.onClick.SubscribeEventAsync(async () =>
        {
            var path = CommonManager.Instance.filePath.PreUIDialogSystemPath;
            await ResourceManager.Instance.ShowDialogAsync<MiUIDialog>(path, "Dialog_MainWindowSettings", CanvasLayer.System);
        });

    }
    public override void OnInit()
    {
        gameObject.SetActive(true);
    }
    public override void OnSetInit(object[] value)
    {
        ShowAsync().Wait();
    }
    public override async Task OnSetInitAsync<T>(params object[] value)
    {
        await AsyncDefaule();
    }
}