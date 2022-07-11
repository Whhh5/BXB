using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using BXB;

public class Eff_Common_Aureole_1 : CommonEffectsBase
{
    private void Update()
    {
        if (mainParticle != null && mainParticle.particleCount <= 0)
        {
            Destroy();
        }
    }

    public override void OnSetInit(params object[] objs)
    {
        transform.SetPositionAndRotation((Vector3)objs[0], (Quaternion)objs[1]);
    }

    public override void Active(params object[] objs)
    {
        gameObject.SetActive(true);
        Play();
    }

    public override void OnInit()
    {
        gameObject.SetActive(false);
    }
}
