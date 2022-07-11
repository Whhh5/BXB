using UnityEngine;
using BXB.Core;

public abstract class CommonEffectsBase : MiObjPoolPublicParameter, IEffects
{
    [SerializeField] protected GameObject main;
    [SerializeField] protected ParticleSystem mainParticle;
    [SerializeField, ReadOnly] protected ParticleSystem.MainModule mainMode;
    protected override void OnAwake()
    {
        base.OnAwake();
        mainMode = mainParticle.main;
    }
    public void Pause()
    {
        mainParticle.Pause();
    }

    public void Play()
    {
        Stop();
        mainParticle.Play();
    }
    public void Stop()
    {
        mainParticle.Stop();
        mainParticle.Clear();
        mainParticle.time = 0;
    }
    /// <summary>
    /// 0.StartDelay   1.StartLifetime
    /// </summary>
    /// <param name="objs"></param>
    public abstract void OnSetInit(params object[] objs);

    public void Continue()
    {
        mainParticle.Play();
    }

    public abstract void Active(params object[] objs);

    public abstract void OnInit();


    public object Clone()
    {
        return MemberwiseClone();
    }
    public override void Destroy()
    {
        base.Destroy();
        main.transform.Normalization(gameObject.transform);
    }

    public GameObject GetMain()
    {
        return main;
    }

    public object GetInfo()
    {
        return null;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

}
