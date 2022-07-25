using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Threading.Tasks;
using TMPro;
using BXB.Core;

public class LeaveCheckWindow : MiUIDialog
{
    [SerializeField] MiUIButton LeaveBtn;
    [SerializeField] MiUIButton BackBtn;

    // Update is called once per frame
    void  Update()
    {
        
    }

    public override void OnInit()
    {
        LeaveBtn?.onClick.AddListener(OnLeaveClick);
        BackBtn?.onClick.AddListener(OnBackClick);
    }

    public override void OnSetInit(object[] value)
    {
        ShowAsync().Wait();
    }

    public override async Task OnSetInitAsync<T>(params object[] value)
    {

    }

    public void OnLeaveClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }

    public void OnBackClick()
    {
        gameObject.SetActive(false);
    }
}
