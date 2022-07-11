using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using System.Threading.Tasks;
using DG.Tweening;

public class GridLayoutGroup : MiUIDialog
{
    enum Direction
    {
        horizontal,
        vertical,
    }
    [SerializeField] RectTransform main;
    [SerializeField] GameObject primary;
    [SerializeField] Direction startDirection;
    [Tooltip("Horizontal - Vertical"), SerializeField] Vector2 ranksCount;
    [Tooltip("Horizontal - Vertical"), SerializeField] Vector2 intervalPosition;
    [Tooltip("Horizontal - Vertical"), SerializeField] Vector2 intervalTime;

    Dictionary<ushort, List<RectTransform>> data = new Dictionary<ushort, List<RectTransform>>();
    [SerializeField,ReadOnly] RectTransform loopHorFirst;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {

        }
    }
    public async Task SetupShow(List<RectTransform> rects)
    {
        await Destroy();
        if (rects == null || rects.Count == 0)
        {
            Log(color: Color.black, "Nont SetUpShow() ,List Is Null Or Count Is Zero");
            return;
        }
        loopHorFirst = rects[0];
        ushort index = 0;
        data.Add(index, new List<RectTransform>());
        foreach (var parameter in rects)
        {
            if (data[index].Count >= ranksCount.x)
            {
                index++;
                data.Add(index, new List<RectTransform>());
                loopHorFirst = null;
            }
            if (loopHorFirst == null)
            {
                loopHorFirst = parameter;
            }
            data[index].Add(parameter);
            parameter.Normalization(main);
            parameter.anchoredPosition3D = loopHorFirst.anchoredPosition3D;

            parameter.DOAnchorPos3DX((data[index].Count - 1) * intervalPosition.x, intervalTime.x * (data[index].Count - 1), false);
            parameter.DOAnchorPos3DY((data.Count - 1) * intervalPosition.y, intervalTime.y * (data.Count - 1), false);
        }
        await ShowAsync();
    }

    public override async Task ShowAsync(DialogMode mode = DialogMode.none)
    {
        await base.ShowAsync(mode);



    }

    public async Task AddAsync(RectTransform rect)
    {

    }

    public async Task RemoveAsync(RectTransform rect)
    {

    }

    public async Task Destroy()
    {
        foreach (var parameter in data)
        {
            foreach (var para in parameter.Value)
            {
                para.gameObject.SetActive(false);
                para.Normalization(null);
                await ObjPool.Repulace(primary, para.gameObject);
            }
        }
        data = new Dictionary<ushort, List<RectTransform>>();
    }

    public override void OnInit()
    {
        throw new System.NotImplementedException();
    }

    public override void OnSetInit(object[] value)
    {
        throw new System.NotImplementedException();
    }

    public override Task OnSetInitAsync<T>(params object[] value)
    {
        throw new System.NotImplementedException();
    }
}
