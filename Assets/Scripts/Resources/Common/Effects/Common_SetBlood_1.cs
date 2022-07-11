using UnityEngine;
using BXB.Core;

public class Common_SetBlood_1 : CommonAnimationHint
{
    [SerializeField] Common_Horizontal_Word horizontalList;
    [SerializeField] GameObject textPrefab;

    public override void Active(object[] value)
    {
        horizontalList.Setting(textPrefab, interval_MoveTime: new Vector2(0.3f, 0.2f));
        Show(endEvent: () =>
        {
            Hide(()=> { });
        });
    }

    public override void OnInit()
    {
        gameObject.SetActive(false);
        horizontalList.Clear();
    }

    public override void OnSetInit(object[] value)
    {
        GameObject obj;
        SpriteRenderer sprite;
        var number = (string)value[0];
        for (int i = 0; i < number.Length; i++)
        {
            obj = ObjPool.GetObject(textPrefab);
            sprite = obj.GetComponent<SpriteRenderer>();
            sprite.sprite = ResourceManager.Instance.Load<Sprite>(CommonManager.Instance.filePath.ResArt, $"Sprites/DamageNumber/Battle_text_normal_{number[i]}");
            horizontalList.AddElement(obj.transform);
            sprite.color = (Color)value[1];
            obj.SetActive(true);
        }
    }
}
