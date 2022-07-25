using UnityEngine.SceneManagement;
using UnityEngine;
using System.Threading.Tasks;
using BXB.Core;

public class CharacterSelectWindow : MiUIDialog
{
    [SerializeField] MiUIButton CloseButton;
    protected override async Task OnAwakeAsync()
    {
        await base.OnAwakeAsync();

        CloseButton.onClick.SubscribeEventAsync(async () =>
        {
            gameObject.SetActive(false);
            var path = CommonManager.Instance.filePath.PreUIDialogSystemPath;
            await ResourceManager.Instance.ShowDialogAsync<MiUIDialog>(path, "MainWindow", CanvasLayer.System);

            ResourceManager.Instance.RemoveSceneAsync(ResourceManager.SceneMode.LevelSelect, UnityEngine.SceneManagement.UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
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