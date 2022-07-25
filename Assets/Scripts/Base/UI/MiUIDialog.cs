using UnityEngine;
using System.Threading.Tasks;
namespace BXB
{
    namespace Core
    {
        public abstract class MiUIDialog : MiUIBase, IUIDialog
        {

            [Header("Buttton"),
                SerializeField]
            MiUIButton close;
            protected UISceneManager manager => UISceneManager.Instance;
            protected override async Task OnAwakeAsync()
            {
                await base.OnAwakeAsync();
                if (close != null)
                {
                    close.onClick.SubscribeEventAsync(async () => { await HideAsync(); await MiAsyncManager.Instance.Default(); }).SubscribeGC(1);
                }
            }
            public override async Task ShowAsync(DialogMode mode = DialogMode.none)
            {
                await base.ShowAsync(mode);
                switch (mode)
                {
                    case DialogMode.stack:
                        await UISceneManager.Instance.stack.Push(this);
                        break;
                    case DialogMode.none:
                        break;
                    default:
                        break;
                }
                GetComponent<RectTransform>().SetSiblingIndex(transform.parent.childCount - 1);
            }

            public override void Destroy()
            {
                HideAsync().Wait();
            }
        }
    }
}
