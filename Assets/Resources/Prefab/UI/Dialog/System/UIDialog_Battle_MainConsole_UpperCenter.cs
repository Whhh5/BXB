using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;
public class UIDialog_Battle_MainConsole_UpperCenter : MiUIBase
{
    [SerializeField] MiUIText leadPower;
    [SerializeField] MiUIText gold;
    [SerializeField] MiUIText speech;
    [SerializeField] WapObjBase mainPlayer;
    [SerializeField] Dictionary<ulong, int> param;
    [SerializeField] ulong leaderPowerId = 120010001;
    [SerializeField] ulong goldId = 120010002;
    [SerializeField] ulong speechId = 120010003;
    [SerializeField] int leaderNum=0;
    [SerializeField] int goldNum=0;
    [SerializeField] int speechNum=0;
    [SerializeField] Image iconLead;
    [SerializeField] Image iconGold;
    [SerializeField] Image iconSpeech;
    public override void OnInit()
    {

    }
    protected override void Start()
    {
        base.Start();
    }
    public override void OnSetInit(object[] value)
    {
    }
    //private void Update()
    //{
    //    SetUpperCenterParam();
    //}
    public void SetUpperCenterParam()
    {
        if (param == null)
        {
            Debug.Log("param == null");
        }
        mainPlayer = SceneDataManager.Instance.mainPlayer;
        param = mainPlayer.articleGet;
        iconLead = GetComponentsInChildren<Image>()[1];
        iconGold = GetComponentsInChildren<Image>()[2];
        iconSpeech = GetComponentsInChildren<Image>()[3];

        leadPower = GetComponentsInChildren<MiUIText>()[0];
        gold = GetComponentsInChildren<MiUIText>()[1];
        speech = GetComponentsInChildren<MiUIText>()[2];
        param.TryGetValue(leaderPowerId,out leaderNum);
        param.TryGetValue(goldId,out goldNum);
        param.TryGetValue(speechId,out speechNum);
        leadPower.SetRawText(leaderNum).Wait();
        gold.SetRawText(goldNum).Wait();
        speech.SetRawText(speechNum).Wait();

        string path = "Images/Sprite/Icon/";
        Sprite icon1=Resources.Load<Sprite>(path+leaderPowerId);
        iconLead.sprite = icon1;
        Sprite icon2 = Resources.Load<Sprite>(path+goldId);
        iconGold.sprite = icon2;
        Sprite icon3 = Resources.Load<Sprite>(path+speechId);
        iconSpeech.sprite = icon3;


       
    }
    public override async Task OnSetInitAsync<T>(params object[] value)
    {
        await AsyncDefaule();
    }

    // Start is called before the first frame update


    // Update is called once per frame

}
