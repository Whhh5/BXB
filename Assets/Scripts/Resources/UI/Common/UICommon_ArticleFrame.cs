using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using BXB.Core;

public class UICommon_ArticleFrame : UIElementPoolBase
{
    [SerializeField] CanvasGroup mainGroup;
    [SerializeField] MiUIText countText;
    [SerializeField] MiUIButton button;
    [SerializeField] Image icon;
    [SerializeField] Image frame;
    [SerializeField] ulong itemId;
    public async Task SetUp(GameObject original, ulong id, Sprite sprite, bool isRaycast = true)
    {
        icon.sprite = sprite;

        mainGroup.blocksRaycasts = isRaycast;
        await AsyncDefaule();
    }
    public async Task SetUp(GameObject original, ulong id,ulong count = 1, bool isRaycast = true)
    {
        //icon.sprite = await ResourceManager.Instance.LoadAsync<Sprite>("Sprite/Icon", id.ToString());
        //await countText.SetRawText(count.ToString());
        //mainGroup.blocksRaycasts = isRaycast;
        await AsyncDefaule();
    }

    public override void OnInit()
    {

    }

    public override void OnSetInit(object[] value)
    {

    }
    public override async Task OnSetInitAsync<T>(params object[] value)
    {
        Func<Task<Sprite>> f_sprite = value.Length > 0 ? (Func<Task<Sprite>>)value[0] : null;
        Func <Task<ulong>> f_count = value.Length > 1 ? (Func<Task<ulong>>)value[1] : null;
        Func<Task<bool>> f_isRaycast = value.Length > 2 ? (Func<Task<bool>>)value[2] : null;
        Func<Task<ulong>> f_itemId = value.Length > 3 ? (Func<Task<ulong>>)value[3] : null;

        Sprite sprite = f_sprite != null ? await f_sprite.Invoke() : null;
        ulong count = f_count != null ? await f_count.Invoke() : 1;
        bool isRaycast = f_isRaycast != null ? await f_isRaycast.Invoke() : true;
        ulong itemId = f_itemId != null ? await f_itemId.Invoke() : 0;

        switch (sprite != null)
        {
            case true:
                icon.sprite = sprite;
                break;
            default:
                icon.enabled = false;
                break;
        }
        switch (count > 1)
        {
            case true:
                await countText.SetRawText(count);
                break;
            default:
                await countText.SetActive(false);
                break;
        }
        this.itemId = itemId;
        mainGroup.blocksRaycasts = isRaycast;
    }
    public async Task<ulong> GetItemID()
    {
        await AsyncDefaule();
        return itemId;
    }
}
