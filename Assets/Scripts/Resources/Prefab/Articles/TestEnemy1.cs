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
        if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            SceneDataManager.Instance.RangArticle(20, 40);
            var sched = MasterData.Instance.GetTableData<LocalRolesData>(GetId()).forward;
            SceneDataManager.Instance.GetSetLevelSchedule(sched);
            var level = GetSet(PropertyFloat.level);
            var upLevelExp = MasterData.Instance.GetTableData<LocalRolesLevelData>((ulong)level);
            var exp = MasterData.Instance.GetTableData<LocalRolesData>(GetId()).exp;
            SceneDataManager.Instance.AddExp(exp);
        }
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
         * 1.检测攻击范围是否有敌人
         * 
         * 1.1.有敌人攻击
         * 1.2.无敌人 向将军方向移动一格
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
        idExitCoroutine = true;
        SceneDataManager.Instance.SetSceneMode(SceneDataManager.SceneMode.Play);
    }

    public override void Change()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //var sched = MasterData.Instance.GetTableData<LocalRolesData>(GetId()).forward;
            //SceneDataManager.Instance.GetSetLevelSchedule(sched);
        }
    }
}
