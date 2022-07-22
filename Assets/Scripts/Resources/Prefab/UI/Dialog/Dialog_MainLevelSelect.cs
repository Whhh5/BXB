using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using System.Threading.Tasks;

public class Dialog_MainLevelSelect : MiUIDialog
{
    [SerializeField] MiUIButton btn_Level1;
    [SerializeField] MiUIButton btn_Close;
    public override void OnInit()
    {
        ShowAsync().Wait();
    }

    public override void OnSetInit(object[] value)
    {
        btn_Level1.onClick.RemoveAllListeners();
        btn_Level1.AddOnPointerClick(async () =>
        {
            await AsyncDefaule();
            foreach (var item in ResourceManager.Instance.dialogs)
            {
                item.Value.Destroy();
            }
            var operation = ResourceManager.Instance.LoadSceneAsync(ResourceManager.SceneMode.Battle, UnityEngine.SceneManagement.LoadSceneMode.Additive);
            ResourceManager.Instance.RemoveSceneAsync(ResourceManager.SceneMode.LevelSelect, UnityEngine.SceneManagement.UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        });

        btn_Close.onClick.SubscribeEventAsync(async () =>
        {
            var path = CommonManager.Instance.filePath.PreUIDialogSystemPath;
            await ResourceManager.Instance.ShowDialogAsync<MiUIDialog>(path, "MainWindow", CanvasLayer.System);

            ResourceManager.Instance.RemoveSceneAsync(ResourceManager.SceneMode.LevelSelect, UnityEngine.SceneManagement.UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        });
    }

    public override async Task OnSetInitAsync<T>(params object[] value)
    {
        await AsyncDefaule();
    }
}
