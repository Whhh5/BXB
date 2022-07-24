using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using System.Threading.Tasks;
using TMPro;
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
    public override void OnInit()
    {

    }
    private void Start()
    {
        mainPlayer = SceneDataManager.Instance.mainPlayer;
        param = mainPlayer.articleGet;
        leadPower = GetComponentsInChildren<MiUIText>()[0];
        gold = GetComponentsInChildren<MiUIText>()[1];
        speech = GetComponentsInChildren<MiUIText>()[2];
    }
    public override void OnSetInit(object[] value)
    {
    }
    private void Update()
    {
        SetUpperCenterParam();
    }
    public void SetUpperCenterParam()
    {

        param.TryGetValue(leaderPowerId,out leaderNum);
        param.TryGetValue(goldId,out goldNum);
        param.TryGetValue(speechId,out speechNum);
        leadPower.SetRawText(leaderNum).Wait();
        gold.SetRawText(goldNum).Wait();
        speech.SetRawText(speechNum).Wait();
       
    }
    public override async Task OnSetInitAsync<T>(params object[] value)
    {
        await AsyncDefaule();
    }

    // Start is called before the first frame update


    // Update is called once per frame

}
