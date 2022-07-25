using UnityEngine;
using UnityEngine.Events;

namespace BXB
{
    namespace Core
    {
        public class MiButtonObject : MiBaseMonoBeHaviourClass
        {
            [SerializeField] Color enterColor;
            [SerializeField] Color downColor;
            [SerializeField] Color dragColor;
            [HideInInspector] public UnityEvent onClickEnter = new UnityEvent();
            [HideInInspector] public UnityEvent onClickOver = new UnityEvent();
            [HideInInspector] public UnityEvent onClickExit = new UnityEvent();
            [HideInInspector] public UnityEvent onClickDown = new UnityEvent();
            [HideInInspector] public UnityEvent onClickDrag = new UnityEvent();
            [HideInInspector] public UnityEvent onClickUp = new UnityEvent();



            [SerializeField, ReadOnly] bool isExecute = true;
            [SerializeField, ReadOnly] Color perproColor = new Color();
            //[SerializeField] Material buttonColor => GetComponent<MeshRenderer>().materials[0];
            [SerializeField] Material spriteRenderer => GetComponent<SpriteRenderer>().material;
            protected virtual void OnMouseEnter()
            {
                if (!isExecute) return;
                AddOnMouseEnterClick();
            }
            protected virtual void OnMouseOver()
            {
                if (!isExecute) return;
                AddOnMouseOverClick();
            }
            protected virtual void OnMouseExit()
            {
                if (!isExecute) return;
                AddOnMouseExitClick();
            }


            protected virtual void OnMouseDown()
            {
                if (!isExecute) return;
                AddOnMouseDownClick();
            }
            protected virtual void OnMouseDrag()
            {
                if (!isExecute) return;
                AddOnMouseDragClick();
            }
            protected virtual void OnMouseUp()
            {
                if (!isExecute) return;
                AddOnMouseUpClick();
            }

            public virtual void AddOnMouseEnterClick()
            {
                onClickEnter.SubscribeEventAsync(async () => { await MiAsyncManager.Instance.Default(); Debug.Log($"{this.gameObject.name}  OnPointerClock"); }).SubscribeGC(0);
                perproColor = ObjectColor(default);
                ObjectColor(enterColor);
            }
            public virtual void AddOnMouseOverClick()
            {
                onClickOver.SubscribeEventAsync(async () => { await MiAsyncManager.Instance.Default(); Debug.Log($"{this.gameObject.name}  OnPointerClockDown"); }).SubscribeGC(1);
            }
            public virtual void AddOnMouseExitClick()
            {
                onClickExit.SubscribeEventAsync(async () => { await MiAsyncManager.Instance.Default(); Debug.Log($"{this.gameObject.name}  OnPointerClockUp"); }).SubscribeGC(2);
                ObjectColor(perproColor);
            }
            public virtual void AddOnMouseDownClick()
            {
                onClickDown.SubscribeEventAsync(async () => { await MiAsyncManager.Instance.Default(); Debug.Log($"{this.gameObject.name}  OnPointerClockEnter"); }).SubscribeGC(3);
                ObjectColor(downColor);
            }
            public virtual void AddOnMouseDragClick()
            {
                onClickDrag.SubscribeEventAsync(async () => { await MiAsyncManager.Instance.Default(); Debug.Log($"{this.gameObject.name}  OnPointerClockExit"); }).SubscribeGC(4);
                ObjectColor(dragColor);
            }
            public virtual void AddOnMouseUpClick()
            {
                onClickUp.SubscribeEventAsync(async () => { await MiAsyncManager.Instance.Default(); Debug.Log($"{this.gameObject.name}  OnPointerClockExit"); }).SubscribeGC(4);
                ObjectColor(perproColor);
            }

            private Color ObjectColor(Color color = default)
            {
                if (color != default)
                {
                    //if (buttonColor != null)
                    //{
                    //    buttonColor.color = color;
                    //    return buttonColor.color;
                    //}
                    //else 
                    if(spriteRenderer != null)
                    {
                        spriteRenderer.color = color;
                        return spriteRenderer.color;
                    }
                }
                else
                {
                    //if (buttonColor != null)
                    //{
                    //    return buttonColor.color;
                    //}
                    //else 
                    if (spriteRenderer != null)
                    {
                        return spriteRenderer.color;
                    }
                }
                return default;
            }
        }
    }
}