using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UIDialog_Battle_MainConsole_PlayerOriginal : UIElementPoolBase
{
    [SerializeField] Image icon;
    [SerializeField] MiUIText number;
    public override void OnInit()
    {
        
    }

    public override void OnSetInit(object[] value)
    {
        var icon = (Sprite)value[0];
        var number = (string)value[1];

        this.icon.sprite = icon;
        this.number.SetRawText(number).Wait();
    }

    public override async Task OnSetInitAsync<T>(params object[] value)
    {
        await AsyncDefaule();
    }
}
