using UnityEngine.Events;
using UnityEngine;
using System;
using System.Threading.Tasks;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BXB
{
    namespace Core
    {
        public partial class MiUIButton : MiBaseMonoBeHaviourClass, MiIUIPointEvent
        {
            [SerializeField] Color enterColor = Color.black;
            [SerializeField] Color downColor = Color.black;

            [HideInInspector] public UnityEvent onClick = new UnityEvent();
            [HideInInspector] public UnityEvent onClickDown = new UnityEvent();
            [HideInInspector] public UnityEvent onClickUp = new UnityEvent();
            [HideInInspector] public UnityEvent onClickEnter = new UnityEvent();
            [HideInInspector] public UnityEvent onClickExit = new UnityEvent();
            [HideInInspector] public UnityEvent onClickPersist = new UnityEvent();


            [SerializeField] bool isEnabledClick = true;
            UnityEvent initEvents = new UnityEvent();
            Color perproColor = new Color();
            [SerializeField,ReadOnly] ButtonStstus buttonStatus = ButtonStstus.None;
            private Func<bool> onLongDownBoolFunc;
            Image buttonColor => GetComponent<Image>();

            public delegate void eventVoid<T>(T value);
            protected override void OnAwake()
            {
                base.OnAwake();
                initEvents.SubscribeEventAsync(InitClick).SubscribeGC(-1);
                initEvents.Invoke();
                onLongDownBoolFunc = () => (buttonStatus & ButtonStstus.Down) != 0;
            }
            protected override async Task OnStartAsync()
            {
                await base.OnStartAsync();
            }

            async Task InitClick()
            {
                await MiAsyncManager.Instance.Default();
                if(!GetComponent<Image>()) gameObject.AddComponent<Image>().color = new Color(0,0,0,0);
            }

            public void OnPointerClick(PointerEventData eventData)
            {
                if (!isEnabledClick) return;
                //Log(color: Color.black, $"OnPointerClick");
                onClick.Invoke();
            }
            public void OnPointerDown(PointerEventData eventData)
            {
                if (!isEnabledClick) return;
                onClickDown.Invoke();
                if (downColor != Color.black)
                {
                    buttonColor.color = downColor;
                }
                if ((buttonStatus & ButtonStstus.Down) != ButtonStstus.Down)
                {
                    buttonStatus = (ButtonStstus)((int)buttonStatus + (int)ButtonStstus.Down);
                }
            }
            public void OnPointerUp(PointerEventData eventData)
            {
                if (!isEnabledClick) return;
                onClickUp.Invoke();
                buttonColor.color = perproColor;
                if ((buttonStatus & ButtonStstus.Down) == ButtonStstus.Down)
                {
                    buttonStatus = (ButtonStstus)(buttonStatus - ButtonStstus.Down);
                }
            }
            public void OnPointerEnter(PointerEventData eventData)
            {
                if (!isEnabledClick) return;
                onClickEnter.Invoke();
                perproColor = buttonColor.color;
                if (enterColor != Color.black)
                {
                    buttonColor.color = enterColor;
                }
                if ((buttonStatus & ButtonStstus.Enter) != ButtonStstus.Enter)
                {
                    buttonStatus = (ButtonStstus)((int)buttonStatus + (int)ButtonStstus.Enter);
                }
            }
            public void OnPointerExit(PointerEventData eventData)
            {
                if (!isEnabledClick) return;
                onClickExit.Invoke();
                buttonColor.color = perproColor;
                if ((buttonStatus & ButtonStstus.Enter) == ButtonStstus.Enter)
                {
                    buttonStatus = (ButtonStstus)(buttonStatus - ButtonStstus.Enter);
                }
            }
            public virtual void AddOnPointerClick(Func<Task> func)
            {
                onClick.SubscribeEventAsync(func).SubscribeGC(0);
            }
            public virtual void AddOnPointerDownClick(Func<Task> func)
            {
                onClickDown.SubscribeEventAsync(func).SubscribeGC(1);
            }
            public virtual void AddOnPointerUpClick(Func<Task> func)
            {
                onClickUp.SubscribeEventAsync(func).SubscribeGC(2);
            }
            public virtual void AddOnPointerEnterClick(Func<Task> func)
            {
                onClickEnter.SubscribeEventAsync(func).SubscribeGC(3);
            }
            public virtual void AddOnPointerExitClick(Func<Task> func)
            {
                onClickExit.SubscribeEventAsync(func).SubscribeGC(4);
            }
            public virtual void AddOnPointerLongDownClick(Func<Task> func)
            {
                onClickPersist.SubscribeEventAsync(func).SubscribeGC(5);
            }

            public void Dispose()
            {
                Debug.Log($"Dispose   {0}");
            }

            private void Update()
            {
                if (onLongDownBoolFunc.Invoke())
                {
                    onClickPersist.Invoke();
                }
            }
            public ButtonStstus GetButtonStatus()
            {
                return buttonStatus;
            }

            ~MiUIButton()
            {
            }
        }
    }
}