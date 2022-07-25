using BXB.Core;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class VerticalLayout : MonoBehaviour
{
    [SerializeField] Vector4 fourInterval;
    [SerializeField] float objectInterval;
    [SerializeField] ushort accordingToChildrenAmendment;

    [SerializeField, ReadOnly] List<RectTransform> chils = new List<RectTransform>();
    private void Start()
    {

    }
    private void Update()
    {
        Active();
    }
    void Active()
    {
        chils.Clear();
        for (int i = accordingToChildrenAmendment; i < transform.childCount; i++)
        {
            chils.Add(transform.GetChild(i).GetComponent<RectTransform>());
        }
        if (chils.Count == 0)
        {
            return;
        }
        var frist = chils[0];
        frist.anchoredPosition3D = new Vector3(fourInterval.x, fourInterval.y, 0);
        for (int i = 1; i < chils.Count; i++)
        {
            float x = frist.anchoredPosition3D.x;
            var y = -(chils[i - 1].sizeDelta.y + objectInterval) + chils[i - 1].anchoredPosition3D.y;

            Vector3 anch = new Vector3(x, y, 0);
            chils[i].anchoredPosition3D = anch;
        }
    }
}
