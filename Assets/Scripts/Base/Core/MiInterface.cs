using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BXB
{
    namespace Core
    {
        public interface IDataItemMothodBase
        {
            public List<object> GetData();
        }
        public interface IUIBase : ICommon_GameObject
        {
            public Task OnSetInitAsync<T>(params object[] value);
            public Task ShowAsync(DialogMode mode);
            public Task HideAsync(DialogMode mode);
        }

        public interface IUIDialog : IUIBase
        {
            
        }
        public interface IUIElementPoolBase: IUIBase
        {
            
        }
        public interface MiIUIPointEvent : IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IDisposable 
        {
            
        }

        public interface IUIPage : ICloneable
        {
            public Task Initialization();
            public Task ShowAsync();
            public Task Distroy();
        }
        public interface IStatusPattern
        {
            public uint GetState();
            public void SetState(uint state);
        }

        public interface IWordGameObject : ICommon_GameObject
        {

        }

        public interface IWeapon
        {
            public GameObject GetMain(GameObject par);
            public void Put();
        }
        public interface IObjPool
        {
            public void SettingOriginal(GameObject original);
            public void SettingId(ulong id);
            public void Destroy();
            public ref readonly ulong GetId();
        }
        public interface ICommon_GameObject : ICloneable
        {
            public GameObject GetMain();
            public void OnInit();
            public void OnSetInit(object[] value);
            public void Destroy();
        }
        public interface ICommon_Weapon : ICommon_GameObject
        {
            public void Action(params object[] value);
        }
        public interface ICommon_Object : ICommon_GameObject
        {
            public CommonGameObjectInfo GetInfo();
            public ObjectType GetObjectType();
            public void AddBloodValue(float value);
            public void AddBSetBloodClick(Action<CommonGameObjectInfo> action);
            public void SetBlood(float value);
        }
        public interface IEffects : ICommon_GameObject, ICommon_Weapon
        {
            public void Play();
            public void Pause();
            public void Continue();
            public void Stop();
        }
        public interface IBuildingFacilities : ICommon_GameObject, ICommon_Weapon
        {
            public void OnMainCollisionEnter2D(Collision2D collision2D);
            public void OnMainCollisionExit2D(Collision2D collision2D);
            public void OnMainCollisionStay2D(Collision2D collision2D);
            public void OnMainTriggerEnter2D(Collider2D collider2D);
            public void OnMainTriggerExit2D(Collider2D collider2D);
            public void OnMainTriggerStay2D(Collider2D collider2D);
        }
        public interface ICommonAnimationHint : ICommon_GameObject, ICommon_Weapon
        {
            public void Show(Action endEvent);
            public void Hide(Action startEvent);
        }
    }
}