using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UIElement_CommonText : UIElementPoolBase
{
    [SerializeField] CanvasGroup group;
    [SerializeField] MiUIText text;
    public override void OnInit()
    {
        
    }

    public override void OnSetInit(object[] value)
    {
        
    }
    public CanvasGroup GetGroup()
    {
        if (group == null)
        {
            group = gameObject.AddComponent<CanvasGroup>();
        }
        return group;
    }

    public override async Task OnSetInitAsync<T>(params object[] value)
    {
        await AsyncDefaule();
    }
    public void SetRawText(string str)
    {
        text.SetRawText(str).Wait();
    }
}
