using UnityEngine;
using BXB.Core;
using System.Threading.Tasks;
using TMPro;

public class MiUIText : MiBaseMonoBeHaviourClass
{
    [SerializeField] TextMeshProUGUI textPro = null;
    protected override async Task OnAwakeAsync()
    {
        await base.OnAwakeAsync();

        textPro = GetComponent<TextMeshProUGUI>();
        if (textPro == null)
        {
            textPro = gameObject.AddComponent<TextMeshProUGUI>();
        }
    }
    public async Task SetRawText(object value)
    {
        var str = value.ToString();
        textPro.text = str;
        await AsyncDefaule();
    }
    public string GetRawText()
    {
        return textPro.text;
    }
    public async Task SetActive(bool active)
    {
        textPro.enabled = active;
        await AsyncDefaule();
    }
}
