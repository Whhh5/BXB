using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using System.Threading.Tasks;

public class Dialog_MainLevelSelect : MiUIDialog
{
    [SerializeField] MiUIButton btn_Level1;
    [SerializeField] MiUIButton btn_Level2;
    [SerializeField] MiUIButton btn_Level3;
    [SerializeField] MiUIButton btn_Close;
    [SerializeField] MiUIButton btn_Story;
    public override void OnInit()
    {
        ShowAsync().Wait();
    }

    public override void OnSetInit(object[] value)
    {
        btn_Level1.onClick.RemoveAllListeners();
        btn_Level2.onClick.RemoveAllListeners();
        btn_Level3.onClick.RemoveAllListeners();
        btn_Close.onClick.RemoveAllListeners();
        btn_Story.onClick.RemoveAllListeners();

        btn_Level1.AddOnPointerClick(async () =>
        {
            await AsyncDefaule();
            foreach (var item in ResourceManager.Instance.dialogs)
            {
                item.Value.Destroy();
            }
            SceneDataManager.Instance.CreateLevelSceneData(1);
            var operation = ResourceManager.Instance.LoadSceneAsync(ResourceManager.SceneMode.Battle, UnityEngine.SceneManagement.LoadSceneMode.Additive);
            ResourceManager.Instance.RemoveSceneAsync(ResourceManager.SceneMode.LevelSelect, UnityEngine.SceneManagement.UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        });
        btn_Level2.AddOnPointerClick(async () =>
        {
            await AsyncDefaule();
            foreach (var item in ResourceManager.Instance.dialogs)
            {
                item.Value.Destroy();
            }
            SceneDataManager.Instance.CreateLevelSceneData(2);
            var operation = ResourceManager.Instance.LoadSceneAsync(ResourceManager.SceneMode.Battle, UnityEngine.SceneManagement.LoadSceneMode.Additive);
            ResourceManager.Instance.RemoveSceneAsync(ResourceManager.SceneMode.LevelSelect, UnityEngine.SceneManagement.UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        });
        btn_Level3.AddOnPointerClick(async () =>
        {
            await AsyncDefaule();
            foreach (var item in ResourceManager.Instance.dialogs)
            {
                item.Value.Destroy();
            }
            SceneDataManager.Instance.CreateLevelSceneData(3);
            var operation = ResourceManager.Instance.LoadSceneAsync(ResourceManager.SceneMode.Battle, UnityEngine.SceneManagement.LoadSceneMode.Additive);
            ResourceManager.Instance.RemoveSceneAsync(ResourceManager.SceneMode.LevelSelect, UnityEngine.SceneManagement.UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        });

        btn_Close.onClick.SubscribeEventAsync(async () =>
        {
            var path = CommonManager.Instance.filePath.PreUIDialogSystemPath;
            await ResourceManager.Instance.ShowDialogAsync<MiUIDialog>(path, "MainWindow", CanvasLayer.System);

            ResourceManager.Instance.RemoveSceneAsync(ResourceManager.SceneMode.LevelSelect, UnityEngine.SceneManagement.UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        });

        btn_Story.onClick.SubscribeEventAsync(async () =>
        {
            var path = CommonManager.Instance.filePath.PreUIDialogSystemPath;
            await ResourceManager.Instance.ShowDialogAsync<MiUIDialog>(path, "Dialog_StoryWidget", CanvasLayer.System);

        });
    }

    public override async Task OnSetInitAsync<T>(params object[] value)
    {
        await AsyncDefaule();
    }
}
