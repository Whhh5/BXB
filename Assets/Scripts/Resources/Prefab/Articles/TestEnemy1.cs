using BXB.Core;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy1 : WapObjBase
{
    public override List<string> Die()
    {
        Destroy();
        var gets = GetSet(WapObjBase.PropertyListString.recruitGetArticle);
        return gets;
    }

    public override void OnInit()
    {
        base.OnInit();
    }

    public override void OnSetInit(object[] value)
    {

    }

    public override IEnumerator IE_Action(WapObjBase enemy_Lord, List<WapObjBase> enemys)
    {
        /*
         * 1.��⹥����Χ�Ƿ��е���
         * 
         * 1.1.�е��˹���
         * 1.2.�޵��� �򽫾������ƶ�һ��
         */

        idExitCoroutine = false;
        float startIntervalTime = MiDataManager.Instance.dataProceccing.AttackInterval(this.GetSet(WapObjBase.PropertyFloat.attackInterval));
        var allPointList = GetAllPoint();
        startIntervalTime = MiDataManager.Instance.dataProceccing.GetStartAttackInterval(startIntervalTime);
        yield return new WaitForSeconds(startIntervalTime * 0.1f);
        while (true)
        {
            //
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
        BattleSceneManager.Instance.SetSceneMode(BattleSceneManager.SceneMode.Play);
    }
}