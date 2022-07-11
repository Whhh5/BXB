using UnityEngine;
using System.Threading.Tasks;
using DG.Tweening;
using BXB.Core;

public class Dialog_Common_Hint_01 : MiUIDialog
{
    [SerializeField] MiUIText title;
    [SerializeField] CanvasGroup group;
    [Tooltip("Show Time - Stay Time - Hide Time"),SerializeField] Vector3 time;

    public override void OnInit()
    {
        throw new System.NotImplementedException();
    }
    public override async Task OnSetInitAsync<T>(params object[] value)
    {
        var str = (string)value[0];

        await title.SetRawText(str);
        await ShowAsync();
        group.alpha = 0;
        DOTween.To(() => group.alpha, value => { group.alpha = value; }, 1, time.x)
            .OnComplete(() =>
            {
                DOTween.To(() => 2, value => { }, 1, time.y)
                    .OnComplete(() =>
                    {
                        DOTween.To(() => group.alpha, value => { group.alpha = value; }, 0, time.z)
                            .OnComplete(() => { HideAsync().Wait(); });
                    });
            })
            .SetUpdate(false);

       
    }
    public override void OnSetInit(object[] value)
    {

    }
    public override async Task ShowAsync(DialogMode mode = DialogMode.none)
    {
        await base.ShowAsync();
        transform.SetSiblingIndex(transform.parent.childCount - 1);
    }
}
