using BXB.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : WapObjBase
{
    public LayerMask playerNoDetectionLayer;
    public LayerMask playerDetectionLayer;

    public override void OnInit()
    {
        base.OnInit();
    }
    public override void OnSetInit(object[] value)
    {
    }



    public override List<string> Die()
    {
        //”Œœ∑Ω· ¯
        Destroy();
        Change();
        return new List<string>();
    }

    public override IEnumerator IE_Action(WapObjBase enemy_Lord, List<WapObjBase> enemys)
    {
        idExitCoroutine = false;
        float startIntervalTime = MiDataManager.Instance.dataProceccing.AttackInterval(this.GetSet(WapObjBase.PropertyFloat.attackInterval));
        startIntervalTime = MiDataManager.Instance.dataProceccing.GetStartAttackInterval(startIntervalTime);
        yield return new WaitForSeconds(startIntervalTime * 0.1f);
        while (true)
        {
            //
            var allPointList = GetAllPoint();
            foreach (var item in GetSetLegion())
            {
                allPointList.AddRange(item.GetAllPoint());
            }
            List<WapObjBase> attack_targets = GetAtactTargets(allPointList);

            if (attack_targets.Count == 0)
            {
                break;
            }
            foreach (var target in attack_targets)
            {
                AttactTarget(target, () => { /*enemys.Remove(target);*/ });
            }
            var intervalTime = MiDataManager.Instance.dataProceccing.AttackInterval(this.GetSet(WapObjBase.PropertyFloat.attackInterval));
            yield return new WaitForSeconds(intervalTime);
        }
        SceneDataManager.Instance.SetSceneMode(SceneDataManager.SceneMode.Play);

        idExitCoroutine = true;
    }

    public override void Change()
    {
        
    }

    public void OnAttackPlaySE()
    {
        SoundManager.instance.PlayerWalkAudio();
    }
}
