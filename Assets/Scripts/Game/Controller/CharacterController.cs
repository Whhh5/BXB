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
        //游戏结束
        Destroy();
        Change();
        return new List<string>();
    }

    public override IEnumerator IE_Action(WapObjBase enemy_Lord, List<WapObjBase> enemys)
    {
        idExitCoroutine = false;
        float startIntervalTime = MiDataManager.Instance.dataProceccing.AttackInterval(this.GetSet(WapObjBase.PropertyFloat.attackInterval));
        var allPointList = GetAllPoint();
        foreach (var item in GetSetLegion())
        {
            allPointList.AddRange(item.GetAllPoint());
        }
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
        SceneDataManager.Instance.SetSceneMode(SceneDataManager.SceneMode.Play);

        #region old attack function

        //float startIntervalTime = MiDataManager.Instance.dataProceccing.AttackInterval(this.GetSet(WapObjBase.PropertyFloat.attackInterval));
        //var allPointLiat = GetAllPoint();
        //startIntervalTime = MiDataManager.Instance.dataProceccing.GetStartAttackInterval(startIntervalTime);
        //yield return new WaitForSeconds(startIntervalTime * 0.1f);
        //while (enemys.Count != 0)
        //{
        //    //
        //    List<WapObjBase> attack_targets = new List<WapObjBase>();
        //    //查找范围内 攻击目标
        //    for (int i = -(int)attack_Scope.x; i <= (int)attack_Scope.x; i++)
        //    {
        //        for (int j = -(int)attack_Scope.y; j <= (int)attack_Scope.y; j++)
        //        {
        //            foreach (var item in allPointLiat)
        //            {
        //                var point = item + new Vector2(i, j);
        //                if (BattleSceneManager.Instance.TryGetWap(point, out Wap wap))
        //                {
        //                    if (wap.TryGetObject<WapObjBase>(out WapObjBase obj))
        //                    {
        //                        if (((int)Mathf.Pow(2, obj.gameObject.layer) & layer_attack) != 0)
        //                        {
        //                            if (!attack_targets.Contains(obj))
        //                            {
        //                                attack_targets.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    this.SetAttackTarget(attack_targets);

        //    foreach (var target in attack_targets)
        //    {
        //        if (!(BattleSceneManager.Instance.sceneMode != BattleSceneManager.SceneMode.Acttack) && target != null)
        //        {
        //            Debug.DrawLine(transform.position, target.transform.position, Color.red);
        //            try
        //            {
        //                var data = MiDataManager.Instance.dataProceccing.AttackData(this, target);
        //                var nowEnemyBlood = target.GetSetBlood(-data);

        //                //attack number hint
        //                var targetPos = BattleSceneManager.Instance.sceneMainCamera.WorldToScreenPoint(target.transform.position);
        //                var hintPath = CommonManager.Instance.filePath.PreUIDialogSystemPath;
        //                var hintObj = ResourceManager.Instance.GetUIElementAsync<UIElement_NumberHint>(hintPath, "UIElement_NumberHint", BattleSceneManager.Instance.mainConsole.GetComponent<RectTransform>(), targetPos, -data);

        //                if (nowEnemyBlood <= 0)
        //                {
        //                    var list = target.Die();
        //                    BattleSceneManager.Instance.AddProperty(list);
        //                    enemys.Remove(target);
        //                }
        //            }
        //            catch (Exception)
        //            {

        //            }
        //            BattleSceneManager.Instance.mainConsole.UpdatePlayerProperty().Wait();
        //        }
        //    }

        //    var intervalTime = MiDataManager.Instance.dataProceccing.AttackInterval(this.GetSet(WapObjBase.PropertyFloat.attackInterval));
        //    yield return new WaitForSeconds(intervalTime);
        //}
        //BattleSceneManager.Instance.SetSceneMode(BattleSceneManager.SceneMode.Play);
        #endregion
        idExitCoroutine = true;
    }

    public override void Change()
    {
        
    }
}
