using System;
using System.Reflection;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace BXB
{
    namespace Core
    {
        public class MiInputManager : MiSingletonMonoBeHaviour<MiInputManager>
        {
            UnityEvent clickDownTab = new UnityEvent();
            UnityEvent clickdownCtrlTab = new UnityEvent();

            Dictionary<MiKeyCode, Action> keyCodeDictionary = new Dictionary<MiKeyCode, Action>();

            protected override void OnAwake()
            {
                base.OnAwake();

            }

            protected override void Start()
            {
                clickDownTab.SubscribeEventAsync(async () => { await AsyncDefaule(); Debug.Log($"{GetType()} Get Tab Down"); });
            }

            void Update()
            {
                if (Input.GetKeyDown(KeyCode.F1)) clickdownCtrlTab.Invoke();
                if (Input.GetKeyDown(KeyCode.Tab)) clickDownTab.Invoke();
            }

            public void AddKeyCodeClick<T>(MiKeyCode f_keyCode, MiKeyCodeStatus status, Action f_func)
            {
                var keyCode = f_keyCode;
                Action func = f_func;

                try
                {
                    if (keyCodeDictionary.TryGetValue(keyCode, out Action value))
                        value += func;
                    else
                    {
                        Action click = () => { };
                        keyCodeDictionary.Add(f_keyCode, click);
                        click += func;
                    }
                }
                catch (Exception exp)
                {
                    Log(Color.red, $"{f_keyCode} May is Null - {exp.ToString()}");
                }

            }
            public void RemoveKeyCodeClick<T>(MiKeyCode f_keyCode, Action f_func)
            {
                var keyCode = f_keyCode;
                Action func = f_func;

                Action click = () => { };
                if (keyCodeDictionary.TryGetValue(keyCode, out Action value))
                {
                    try
                    {
                        var array = value.GetInvocationList();
                        int exist = Array.IndexOf(array, (Action)func);
                        if (exist != -1)
                            value -= func;
                    }
                    catch (Exception exp)
                    {
                        Log(Color.red, $"{f_keyCode} May is Null - {exp.ToString()}");
                    }
                }
            }
            public void RemoveAllKeyCodeClic<T>(MiKeyCode f_keyCode)
            {
                var keyCode = f_keyCode;

                Action click = () => { };
                if (keyCodeDictionary.TryGetValue(keyCode, out Action value))
                    value = () => { };
            }


            public async Task AddTabDown(Func<Task> func)
            {
                clickDownTab.SubscribeEventAsync(func);
                await AsyncDefaule();
            }

            public async Task AddCtrlTabDown(Func<Task> func)
            {
                clickdownCtrlTab.SubscribeEventAsync(func);
                await AsyncDefaule();
            }
        }
    }
}

