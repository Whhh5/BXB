using UnityEngine;
using DG.Tweening;
using BXB.Core;

public class Eff_Common_Aureole_2 : CommonEffectsBase
{
    [SerializeField, ReadOnly] bool isDestroy = false;
    public override void Active(params object[] objs)
    {
        Play();
        gameObject.SetActive(true);
        DOTween.To(() => 2, value => { }, 0, 0.5f)
            .OnComplete(() =>
            {
                isDestroy = true;
            });
    }
    private void Update()
    {
        if (isDestroy && mainParticle.particleCount <= 0)
        {
            Destroy();
        }
    }
    public override void OnInit()
    {
        isDestroy = false;
    }

    public override void OnSetInit(params object[] objs)
    {

    }
    public override void Destroy()
    {
        base.Destroy();
    }
}
