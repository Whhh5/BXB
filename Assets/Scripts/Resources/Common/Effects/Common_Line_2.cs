using UnityEngine;
using BXB.Core;

public class Common_Line_2 : CommonEffectsBase
{
    [SerializeField] LineRenderer mainLine;
    [SerializeField] float unitLength;
    [SerializeField, ReadOnly] float tiling;
    [SerializeField, ReadOnly] Transform startTarget;
    [SerializeField, ReadOnly] Transform endTarget;

    private void Update()
    {
        if (startTarget != null && endTarget != null)
        {
            var startPos = GetGameObject().transform.InverseTransformPoint(startTarget.position);
            var endPos = GetGameObject().transform.InverseTransformPoint(endTarget.position);
            mainLine.SetPositions(new Vector3[] { startPos, endPos });
        }
    }
    public override void Active(params object[] objs)
    {

    }

    public override void OnInit()
    {
        mainLine.positionCount = 2;
        startTarget = null;
        endTarget = null;
    }

    public override void OnSetInit(params object[] value)
    {
        startTarget = value[0] as Transform;
        endTarget = value[1] as Transform;
    }

    public override void Destroy()
    {
        base.Destroy();
    }
}
