using BXB.Core;
using UnityEngine;

public class Eff_Common_MotionTrail_1 : CommonEffectsBase
{
    [SerializeField] float moveSpeed;
    [SerializeField, ReadOnly] Transform target;
    [SerializeField, ReadOnly] bool isDestroy;
    public override void Active(params object[] target)
    {
        Play();
    }
    private void Update()
    {
        if (target != null)
        {
            main.transform.position = Vector3.Lerp(main.transform.position, target.position, moveSpeed * Time.deltaTime);
        }
        if (isDestroy && mainParticle.particleCount <= 0)
        {
            base.Destroy();
        }
    }

    public override void OnInit()
    {
        isDestroy = false;
        target = null;
    }

    public override void OnSetInit(params object[] value)
    {
        target = value[0] as Transform;
    }
    public override void Destroy()
    {
        isDestroy = true;
        target = null;
    }
}
