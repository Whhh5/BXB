using System.Collections;
using UnityEngine;
using System.Threading.Tasks;
using BXB.Core;
public class Dialog_LoadingScene : MiUIDialog
{
    [SerializeField] MiUISlider slider;
    [SerializeField] MiUIButton nextBtn;
    [SerializeField] MiUIText loadingHint;

    protected override void OnStart()
    {
        base.OnStart();
    }
    //public async Task SetUpShow(AsyncOperation opera)
    //{
    //    await ShowAsync( DialogMode.none);
    //    Log(color: Color.black, $"{opera != null}");
    //    if (opera!=null)
    //    {
    //        StartCoroutine(LoadScene(opera));
    //    }
    //}

    IEnumerator LoadScene(AsyncOperation opera)
    {
        ShowAsync(DialogMode.none).Wait();
        opera.allowSceneActivation = false;
        float schedule = 0.0f;
        while (schedule < 1)
        {
            schedule = opera.progress * 100 / 90.0f;
            slider.SetValue(schedule);
            yield return null;
        }
        nextBtn.onClick.SubscribeEventAsync(async () => 
        {
            await HideAsync();
            opera.allowSceneActivation = true;
        });
        loadingHint.SetRawText("Finished Click On Screen").Wait();

    }

    public override void OnInit()
    {
        loadingHint.SetRawText("Loading......").Wait();
    }

    public override void OnSetInit(object[] value)
    {
        AsyncOperation opera = (AsyncOperation)value[0];
        ShowAsync(DialogMode.none).Wait();
        //Log(color: Color.black, $"{opera != null}");
        if (opera != null)
        {
            StartCoroutine(LoadScene(opera));
        }
    }
    public override async Task OnSetInitAsync<T>(params object[] value)
    {
        await AsyncDefaule();
    }
}
