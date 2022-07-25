using UnityEngine.SceneManagement;
using UnityEngine;
using System.Threading.Tasks;
using TMPro;
using BXB.Core;

public class RegisterWindow : MiUIDialog
{
    [SerializeField] MiUIButton refisterButton;
    [SerializeField] MiUIButton loginButton;
    [SerializeField] MiUIButton logOutButton;
    [SerializeField] TMP_InputField userName;
    [SerializeField] TMP_InputField password;
    protected override async Task OnAwakeAsync()
    {
        await base.OnAwakeAsync();
        refisterButton.onClick.SubscribeEventAsync(async () =>
        {
            await AsyncDefaule();
            var user = userName.text;
            var pass = password.text;
            MiDataService.Register(user, pass);
        });
        loginButton.onClick.SubscribeEventAsync(async () =>
        {
            var user = userName.text;
            var pass = password.text;
            //await Server.Instance.LinkServer();
            if (MiDataService.Login(user, pass))
            {
                await MainSceneManager.Instance.GameStart();
                Destroy();
                ResourceManager.Instance.LoadSceneAsync( ResourceManager.SceneMode.LevelSelect, LoadSceneMode.Additive);
            }
        });
        logOutButton.onClick.SubscribeEventAsync(async () =>
        {
            await AsyncDefaule();
            var user = userName.text;
            var pass = password.text;
            MiDataService.LogOut(user, pass);
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