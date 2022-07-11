using UnityEngine;
using DG.Tweening;
using BXB.Core;

public class Eff_SelectTarget_1 : CommonEffectsBase
{
    [SerializeField] Animation anima;
    [SerializeField] float durationTime = 3.0f;
    [SerializeField, ReadOnly] Transform target = null;
    [SerializeField,Range(0,10.0f)] float moveSpeed = 5.0f;
    private void Update()
    {
        if (target != null)
        {
            main.transform.position = Vector3.Lerp(main.transform.position, target.position, moveSpeed * Time.deltaTime);
        }
    }

    public override void Active(params object[] target)
    {
        gameObject.SetActive(true);
        if (target == null)
        {
            return;
        }
        anima.Play($"{GetType().Name}_Show");
        DOTween.To(() => 2, value => { }, 0, durationTime).OnComplete(() => { Destroy();});
    }

    public override void OnInit()
    {
        target = null;
        durationTime = 3.0f;
        gameObject.SetActive(false);
    }

    public override void OnSetInit(params object[] value)
    {
        this.durationTime = (float)value[0];
        target = value[1] as Transform;
    }

    public override void Destroy()
    {
        var animaLength =  anima.GetClip($"{GetType().Name}_Hide").length;
        anima.Play($"{GetType().Name}_Hide");
        DOTween.To(() => 2, value => { }, 0, animaLength).OnComplete(() => 
        {
            target = null;
            base.Destroy(); 
        });
        
    }
}
