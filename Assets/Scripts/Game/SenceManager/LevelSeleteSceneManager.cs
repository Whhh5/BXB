using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelSeleteSceneManager : MiSingletonMonoBeHaviour<LevelSeleteSceneManager>
{

    UnityAction<Scene> unLoadSceneClick;
    protected override async Task OnAwakeAsync()
    {
        await base.OnAwakeAsync();
        unLoadSceneClick = new UnityAction<Scene>((sc) => { });
        SceneManager.sceneUnloaded += unLoadSceneClick;
        var path = CommonManager.Instance.filePath.PreUIDialogSystemPath;
        var dialog = await ResourceManager.Instance.ShowDialogAsync<Dialog_MainLevelSelect>(path, "Dialog_MainLevelSelect", CanvasLayer.System);
    }
    public void AddDestroySceneClick(UnityAction<Scene> click)
    {
        unLoadSceneClick += click;
    }
}
