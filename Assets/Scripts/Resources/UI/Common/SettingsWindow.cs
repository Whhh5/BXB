using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Threading.Tasks;
using TMPro;
using BXB.Core;

public class SettingsWindow : MiUIDialog
{
    public Image image;
    [SerializeField] MiUIButton CloseButton;
    protected override async Task OnAwakeAsync()
    {
        await base.OnAwakeAsync();

        CloseButton.onClick.SubscribeEventAsync(async () =>
        {
            Destroy();
           image.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        });

    }
    public override void OnInit()
    {
        ShowAsync().Wait();
        image.color = new Color(0.0f, 0.0f, 0.0f, 0.88f);
    }
    public override void OnSetInit(object[] value)
    {

    }
    public override async Task OnSetInitAsync<T>(params object[] value)
    {
        await AsyncDefaule();
    }
}