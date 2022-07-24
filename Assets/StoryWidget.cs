using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using BXB.Core;
using TMPro;
public class StoryWidget : MiUIDialog
{
    [SerializeField]
    private Button closeBtn;
    public override void OnInit()
    {
      
        ShowAsync().Wait();
    }

    public override void OnSetInit(object[] value)
    {
        closeBtn.onClick.RemoveAllListeners();
        closeBtn.onClick.AddListener(OnCloseBtnClick);
    }

    public override async Task OnSetInitAsync<T>(params object[] value)
    {
        await AsyncDefaule();
    }

    private void OnCloseBtnClick()
    {
        Destroy();
    }

}
