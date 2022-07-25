using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ContentSize : MonoBehaviour
{
    [SerializeField] RectTransform contentSize;
    [SerializeField] RectTransform accordingToChildren;
    [SerializeField] RectTransform toChildren;
    [SerializeField] ushort accordingToChildrenAmendment;
    [SerializeField, Tooltip("Left-Top_Right_Bottom")] Vector4 fourInterval;
    [SerializeField] Direction direction;
    [SerializeField] float objectInterval;

    private void Update()
    {
        Active();
    }

    public void Active()
    {
        Vector2 height = new Vector2(0, 0);
        for (int i = accordingToChildrenAmendment; i < accordingToChildren.childCount; i++)
        {
            if (accordingToChildren.GetChild(i).TryGetComponent<ContentSizeMainDelatSize>(out ContentSizeMainDelatSize cont))
            {
                height += cont.mainRectTr.sizeDelta;
            }
        }
        height.y += (accordingToChildren.childCount - accordingToChildrenAmendment - 1) * objectInterval;

        var unitSizeDelta = toChildren.sizeDelta;
        var fourIntervals = new Vector2(fourInterval.x + fourInterval.z + unitSizeDelta.x, fourInterval.y + fourInterval.w + height.y);
        var sizeDelat = fourIntervals;
        contentSize.sizeDelta = sizeDelat;
    }

    enum Direction
    {
        Horizontal,
        Vertical,
    }
}
