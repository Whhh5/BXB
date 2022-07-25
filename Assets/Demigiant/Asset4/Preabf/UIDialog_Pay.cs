using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using System.Threading.Tasks;

public class UIDialog_Pay : MiUIDialog
{
    [SerializeField] MiUIButton btn_100;
    [SerializeField] MiUIButton btn_200;
    [SerializeField] MiUIButton btn_300;
    [SerializeField] MiUIButton btn_648;
    public override void OnInit()
    {
        ShowAsync().Wait();
    }

    public override void OnSetInit(object[] value)
    {
        btn_100.onClick.RemoveAllListeners();
        btn_200.onClick.RemoveAllListeners();
        btn_300.onClick.RemoveAllListeners();
        btn_648.onClick.RemoveAllListeners();
        btn_100.AddOnPointerClick(async () =>
        {
            await AsyncDefaule();
            Pay(100.0f);
        });
        btn_200.AddOnPointerClick(async () =>
        {
            await AsyncDefaule();
            Pay(200.0f);
        });
        btn_300.AddOnPointerClick(async () =>
        {
            await AsyncDefaule();
            Pay(300.0f);
        });
        btn_648.AddOnPointerClick(async () =>
        {
            await AsyncDefaule();
            Pay(648.0f);
        });
    }
    public void Pay(float value)
    {

        SceneDataManager.Instance.mainPlayer.GetSet(WapObjBase.PropertyFloat.attack, value);
        SceneDataManager.Instance.mainPlayer.GetSet(WapObjBase.PropertyFloat.maxBlood, value);
        SceneDataManager.Instance.mainPlayer.GetSet(WapObjBase.PropertyFloat.defend, value);
        BattleSceneManager.Instance.mainConsole.UpdatePlayerProperty().Wait();
    }


    public override async Task OnSetInitAsync<T>(params object[] value)
    {
        await AsyncDefaule();
    }
}
