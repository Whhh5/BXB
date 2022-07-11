using UnityEngine;
using BXB.Core;

public class Common_Line_1 : CommonEffectsBase
{
    enum Shape
    {
        None,
        Sphere,
        Box,
    }
    [SerializeField] LineRenderer line;
    [SerializeField] Shape shape = Shape.Sphere;
    [SerializeField] int positionCount = 180;
    [SerializeField] ushort angle = 360;
    [SerializeField] float radius = 10;
    [SerializeField] float offsetRadius = 0;
    [SerializeField] bool loop = false;
    [SerializeField, ReadOnly] Transform target;
    [SerializeField] AnimationCurve WidthCurve = new AnimationCurve();
    [SerializeField] Gradient colorCurve = new Gradient();
    protected override void OnStart()
    {
        //line.widthCurve = WidthCurve;
        //line.colorGradient = colorCurve;
        base.OnStart();
    }
    private void Update()
    {
        if (target != null)
        {
            DrawCallSphere();
        }
    }
    public override void Active(params object[] objs)
    {

    }

    public override void OnInit()
    {
        line.loop = loop;
    }

    public override void OnSetInit(params object[] value)
    {
        this.target = value[0] as Transform;
        radius = (float)value[1];
    }
    void DrawCallSphere()
    {
        var unitAngle = angle / positionCount;
        Vector3 localPosition2D = new Vector3();
        line.positionCount = positionCount;
        for (int i = 0; i < positionCount; i++)
        {
            var x = Mathf.Sin(unitAngle * i * Mathf.Deg2Rad) * radius;
            var y = Mathf.Cos(unitAngle * i * Mathf.Deg2Rad) * radius;
            localPosition2D = new Vector3(x, y, 0);
            var pos = transform.TransformVector(localPosition2D);
            line.SetPosition(i, pos);
        }
        main.transform.position = target.position;
    }
    public override void Destroy()
    {
        target = null;
        base.Destroy();
    }
}
