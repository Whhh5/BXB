using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;

public class UIContentSizeDelat : MiBaseMonoBeHaviourClass
{
    [SerializeField] RectTransform autoPivot;
    [SerializeField, Range(0, 20)] float moveSpeed;

    Vector2 pivot;
    void OnEnable()
    {
        pivot = new Vector2(0, 1);
    }

    void Update()
    {
        Active();
        autoPivot.pivot = Vector2.Lerp(autoPivot.pivot, pivot, moveSpeed * Time.deltaTime);
    }

    private void Active()
    {
        var pixelWidth = Camera.main.pixelWidth;
        var pixelHeight = Camera.main.pixelHeight;
        var mousePosition = Input.mousePosition;

        var pivotX = pivot.x;
        var pivotY = pivot.y;

        var value = 0.0f;
        //Y
        var downHeight = autoPivot.rect.height * pivot.y;
        var upHeight = autoPivot.anchoredPosition3D.y + autoPivot.rect.height * (1 - pivotY);
        if (upHeight > pixelHeight)
        {
            value = 1;
        }
        else if (downHeight > mousePosition.y && pixelHeight - upHeight > 10)
        {
            value = -1;
        }
        pivotY += value * Time.deltaTime;

        //X
        value = 0;
        var rightWidth = autoPivot.rect.width * (1 - pivotX);
        var leftWidth = autoPivot.rect.width * pivotX;
        if (mousePosition.x > leftWidth && pixelWidth - mousePosition.x + rightWidth > 10)
        {
            value = -1;
        }
        if (mousePosition.x + rightWidth > pixelWidth)
        {
            value = 1;
        }
        pivotX += value * Time.deltaTime;

        pivotY = pivotY > 1 ? 1 : pivotY;
        pivotY = pivotY < 0 ? 0 : pivotY;
        pivotX = pivotX > 1 ? 1 : pivotX;
        pivotX = pivotX < 0 ? 0 : pivotX;
        pivot = new Vector2(pivotX, pivotY);
    }
}
