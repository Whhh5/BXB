using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class UIDialog_BattleSettingMenu : MiUIDialog
{
    [SerializeField] MiUIButton btn_Close;
    [SerializeField] MiUIButton btn_Open;
    [SerializeField] GameObject mainMenu;

    [SerializeField] MiUIButton btn_ReturnSelectInterface;
    public override void OnInit()
    {
        ShowAsync().Wait();

        mainMenu.SetActive(false);
        btn_Open.onClick.RemoveAllListeners();
        btn_Close.onClick.RemoveAllListeners();
        btn_ReturnSelectInterface.onClick.RemoveAllListeners();
        btn_Open.AddOnPointerClick(async () =>
        {
            await OpenMainMenu();
        });

        btn_ReturnSelectInterface.AddOnPointerClick(async () =>
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
        });
        btn_Close.AddOnPointerClick(async () =>
        {
            await AsyncDefaule();
            await CloseMainMenu();
        });
    }

    public override void OnSetInit(object[] value)
    {

    }

    public override async Task OnSetInitAsync<T>(params object[] value)
    {
        await AsyncDefaule();
    }
    private async Task OpenMainMenu()
    {
        await AsyncDefaule();
        mainMenu.SetActive(true);
    }
    private async Task CloseMainMenu()
    {
        await AsyncDefaule();
        mainMenu.SetActive(false);
    }
}
