using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

public class UIDialog_Battle_MainConsole_Bottom_Process : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] MiUIText processText;
    [SerializeField] MiUIText level;
    [SerializeField] Slider slider;
    [SerializeField] SceneDataManager scene;
    float processValue;
    void Start()
    {
        scene = SceneDataManager.Instance;
        slider = GetComponentInChildren<Slider>();
        level = GetComponentsInChildren<MiUIText>()[0];
        processText = GetComponentsInChildren<MiUIText>()[1];
        slider.onValueChanged.AddListener((float value) => FillSlider(value, slider));
    }

    // Update is called once per frame
    void Update()
    {
        SetProcess();
    }
    public void FillSlider(float value,Slider slider)
    {
        slider.value = processValue;
    }
    private void SetProcess()
    {
        
        

            //processValue = (float)(scene.schedule / scene.nextSceneSchedule);
            processValue = scene.allSchedule;
            if (processValue > 1) {
                processValue = 1.0f;
            }
            level.SetRawText("Level" + 1).Wait();
            processText.SetRawText((int)(processValue*100) + "%").Wait();
            slider.value = processValue;
       
        
        
           // processText.SetRawText("0"+ "%").Wait();
           // slider.value = 0.0f;
        
        
        
       
    }
}
