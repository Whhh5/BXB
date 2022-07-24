using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using BXB.Core;

public abstract class MiUIBase : MiObjPoolPublicParameter, IUIDialog
{
    [Header("Start Game Hide Obj"),
            SerializeField]
    List<GameObject> hideObjs = new List<GameObject>();
    [SerializeField] protected GameObject main;
    [Header("Animation"),
        SerializeField, ReadOnly]
    protected Animation anima;
    [SerializeField] protected AnimationClip showClip;
    [SerializeField] protected AnimationClip hideClip;

    protected override async Task OnAwakeAsync()
    {
        await base.OnAwakeAsync();
        if (GetComponent<Animation>() != null && anima == null)
        {
            anima = GetComponent<Animation>();
        }

        foreach (var item in hideObjs)
        {
            item.SetActive(false);
        }
    }
    public virtual async Task ShowAsync(DialogMode mode = DialogMode.none)
    {
        await OnShowAsync();
        if (anima != null && showClip != null)
        {
            anima.GetClip(showClip.name).events = null;
            anima.GetClip(showClip.name).AddEvent(new AnimationEvent()
            {
                functionName = "OnShowAsync",
                time = anima.GetClip(showClip.name).length,
            });
            await PlayClip(anima, showClip);
        }
    }
    public virtual async Task HideAsync(DialogMode mode = DialogMode.stack)
    {
        if (anima != null && hideClip != null)
        {
            anima.GetClip(hideClip.name).events = null;
            anima.GetClip(hideClip.name).AddEvent(new AnimationEvent()
            {
                functionName = "OnHideAsync",
                time = anima.GetClip(hideClip.name).length,
            });
            await PlayClip(anima, hideClip);
        }
        else
        {
            await OnHideAsync();
        }
    }

    protected async Task OnShowAsync()
    {
        gameObject.SetActive(true);
        await AsyncDefaule();
    }
    protected async Task OnHideAsync()
    {
        gameObject.SetActive(false);
        await AsyncDefaule();
    }

    public async Task PlayClip(Animation anima, AnimationClip clip)
    {
        if (anima != null && clip != null)
        {
            anima.Play(clip.name);
        }
        await AsyncDefaule();
    }

    public GameObject GetMain()
    {
        return main;
    }

    public abstract void OnInit();

    public abstract void OnSetInit(object[] value);

    public object Clone()
    {
        return MemberwiseClone();
    }

    public abstract Task OnSetInitAsync<T>(params object[] value);
}
