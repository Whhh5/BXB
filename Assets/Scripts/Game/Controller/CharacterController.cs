using BXB.Core;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : WapObjBase
{
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
        BattleSceneManager.Instance.GameFinish();
        Destroy();
        return new List<string>();
    }

    public override IEnumerator IE_Action(WapObjBase enemy_Lord, List<WapObjBase> enemys)
    {
        idExitCoroutine = false;

        float startIntervalTime = MiDataManager.Instance.dataProceccing.AttackInterval(this.GetSet(WapObjBase.PropertyFloat.attackInterval));
        startIntervalTime = MiDataManager.Instance.dataProceccing.GetStartAttackInterval(startIntervalTime);
        yield return new WaitForSeconds(startIntervalTime * 0.1f);
        while (enemys.Count != 0)
        {
            //
            WapObjBase attack_target = null;
            this.SetAttackTarget(attack_target);
            if (attack_target == null)
            {
                //查找范围内 攻击目标
                for (int i = -(int)attack_Scope.x; i <= (int)attack_Scope.x; i++)
                {
                    for (int j = -(int)attack_Scope.y; j <= (int)attack_Scope.y; j++)
                    {
                        var point = GetPoint() + new Vector2(i, j);
                        if (BattleSceneManager.Instance.TryGetWap(point, out Wap wap))
                        {
                            if (wap.TryGetObject<WapObjBase>(out WapObjBase obj))
                            {
                                if (((int)Mathf.Pow(2, obj.gameObject.layer) & layer_attack) != 0)
                                {
                                    attack_target = obj;
                                    this.SetAttackTarget(attack_target);
                                    break;
                                }

                            }
                        }
                    }
                    if (attack_target != null)
                    {
                        break;
                    }
                }
            }

            //if (attack_target == null && (StatusMode.Manual & moveMode) == 0)
            //{
            //    //移动
            //    var pathList = BattleSceneManager.Instance.MoveToTarget(this, enemy_Lord.GetPoint());
            //    if (pathList.Count > 0 && BattleSceneManager.Instance.TryGetWap(pathList[0], out Wap targteWap))
            //    {
            //        if (!targteWap.TryGetObject(out Transform tr))
            //        {
            //            BattleSceneManager.Instance.MoveToVector2(this, pathList[0]);
            //        }
            //    }
            //}



            if (!(BattleSceneManager.Instance.sceneMode != BattleSceneManager.SceneMode.Acttack) && attack_target != null)
            {
                Debug.DrawLine(transform.position, attack_target.transform.position, Color.red);
                try
                {
                    var data = MiDataManager.Instance.dataProceccing.AttackData(this, attack_target);
                    var nowEnemyBlood = attack_target.GetSetBlood(-data);

                    //attack number hint
                    var targetPos = BattleSceneManager.Instance.sceneMainCamera.WorldToScreenPoint(attack_target.transform.position);
                    var hintPath = CommonManager.Instance.filePath.PreUIDialogSystemPath;
                    var hintObj = ResourceManager.Instance.GetUIElementAsync<UIElement_NumberHint>(hintPath, "UIElement_NumberHint", BattleSceneManager.Instance.mainConsole.GetComponent<RectTransform>(), targetPos, -data);
                    //

                    if (nowEnemyBlood <= 0)
                    {
                        var list = attack_target.Die();
                        BattleSceneManager.Instance.AddProperty(list);
                        enemys.Remove(attack_target);
                    }
                }
                catch (Exception)
                {

                }
                BattleSceneManager.Instance.mainConsole.UpdatePlayerProperty().Wait();
            }
            var intervalTime = MiDataManager.Instance.dataProceccing.AttackInterval(this.GetSet(WapObjBase.PropertyFloat.attackInterval));
            yield return new WaitForSeconds(intervalTime);
        }
        BattleSceneManager.Instance.SetSceneMode(BattleSceneManager.SceneMode.Play);
        idExitCoroutine = true;
    }
}
