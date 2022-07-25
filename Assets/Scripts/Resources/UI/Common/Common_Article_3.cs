using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using System.Threading.Tasks;
using UnityEngine.UI;

public class Common_Article_3 : UIElementPoolBase
{

    [SerializeField] Image p_icon;
    [SerializeField] MiUIText p_text;

    public override void OnInit()
    {

    }

    public override void OnSetInit(object[] value)
    {

    }

    public override async Task OnSetInitAsync<T>(params object[] value)
    {
        var f_icon = (Sprite)value[0];
        var f_text = (string)value[1];
        p_icon.sprite = f_icon;
        await p_text.SetRawText(f_text);
    }

    public async Task SetUp(Sprite f_icon, string f_text)
    {

    }
}
