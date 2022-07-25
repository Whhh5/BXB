using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using System.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;

public class UIDialog_BattleFalse : MiUIDialog
{
    [SerializeField] MiUIButton CloseBtn;

    public override void OnInit()
    {
        ShowAsync().Wait();

        CloseBtn.onClick.RemoveAllListeners();

        CloseBtn.AddOnPointerClick(async () =>
        {
            await AsyncDefaule();
            await AsyncDefaule();
            foreach (var item in ResourceManager.Instance.dialogs)
            {
                item.Value.Destroy();
            }
            BattleSceneManager.Instance.mainPlayer.gameObject.SetActive(false);
            ResourceManager.Instance.LoadSceneAsync(ResourceManager.SceneMode.LevelSelect, LoadSceneMode.Additive);
            ResourceManager.Instance.RemoveSceneAsync(ResourceManager.SceneMode.Battle, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            ResourceManager.Instance.RemoveSceneAsync(ResourceManager.SceneMode.Boss, UnityEngine.SceneManagement.UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            SoundManager.instance.StopBossBGM();
        });
    }

    public override void OnSetInit(object[] value)
    {
        //CloseBtn.onClick.RemoveAllListeners();

        //CloseBtn.onClick.SubscribeEventAsync(async () =>
        //{
        //    await AsyncDefaule();
        //    await AsyncDefaule();
        //    foreach (var item in ResourceManager.Instance.dialogs)
        //    {
        //        item.Value.Destroy();
        //    }

        //    var path = CommonManager.Instance.filePath.PreUIDialogSystemPath;
        //    await ResourceManager.Instance.ShowDialogAsync<MiUIDialog>(path, "MainWindow", CanvasLayer.System);

        //    BattleSceneManager.Instance.mainPlayer.gameObject.SetActive(false);
        //    ResourceManager.Instance.RemoveSceneAsync(ResourceManager.SceneMode.Battle, UnityEngine.SceneManagement.UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        //    ResourceManager.Instance.RemoveSceneAsync(ResourceManager.SceneMode.Boss, UnityEngine.SceneManagement.UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        //});
    }

    public override async Task OnSetInitAsync<T>(params object[] value)
    {
        await AsyncDefaule();
    }
}
