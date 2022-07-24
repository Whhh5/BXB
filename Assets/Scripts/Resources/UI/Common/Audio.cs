using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class Audio : MonoBehaviour
{
    public AudioMixer audioMixer;
    [SerializeField]
    private Slider sliderBGM;
    [SerializeField]
    private Text volumeTextBGM;
    [SerializeField]
    private Button btnBGMDown;
    [SerializeField]
    private Button btnBGMUp;
    [SerializeField]
    private Slider sliderSE;
    [SerializeField]
    private Text volumeTextSE;
    [SerializeField]
    private Button btnSEDown;
    [SerializeField]
    private Button btnSEUp;
    private int BGMvolumeValue = 80, SEvolumeValue = 80;
    public int minBGMVolume, minSEVolume;
    private int BGMsoundConvert, SEsoundConvert;

    // Start is called before the first frame update

    private void Awake()
    {
        
    }
    void Start()
    {
        BGMsoundConvert = Mathf.Abs(minBGMVolume) / 10 - 10;
        SEsoundConvert = Mathf.Abs(minSEVolume) / 10 - 10;
        btnBGMDown.onClick.AddListener(OnBGMBtnDownClick);
        btnBGMUp.onClick.AddListener(OnBGMBtnUpClick);
        btnSEDown.onClick.AddListener(OnSEBtnDownClick);
        btnSEUp.onClick.AddListener(OnSEBtnUpClick);

    }

    // Update is called once per frame
    void Update()
    {
        volumeTextBGM.text = BGMvolumeValue.ToString() + "%";
        volumeTextSE.text = SEvolumeValue.ToString() + "%";
    }

    public void SetBGMVolume()
    {
        audioMixer.SetFloat("BGMVolume", sliderBGM.value);
    }

    public void SetSEVolume()
    {
        audioMixer.SetFloat("SEVolume", sliderSE.value);
    }

    private void OnBGMBtnDownClick()
    {
        if (BGMvolumeValue >= 10)
        {
            BGMvolumeValue -= 10;
            int multiple = BGMvolumeValue / 10;
            sliderBGM.value = BGMvolumeValue + (BGMsoundConvert * multiple + minBGMVolume);
        }
    }

    private void OnBGMBtnUpClick()
    {
        if (BGMvolumeValue <= 90)
        {
            BGMvolumeValue += 10;
            int multiple = BGMvolumeValue / 10;
            sliderBGM.value = BGMvolumeValue + (BGMsoundConvert * multiple + minBGMVolume);
        }
    }

    private void OnSEBtnDownClick()
    {
        if (SEvolumeValue >= 10)
        {
            SEvolumeValue -= 10;
            int multiple = SEvolumeValue / 10;
            sliderSE.value = SEvolumeValue + (SEsoundConvert * multiple + minSEVolume);
        }
    }

    private void OnSEBtnUpClick()
    {
        if (SEvolumeValue <= 90)
        {
            SEvolumeValue += 10;
            int multiple = SEvolumeValue / 10;
            sliderSE.value = SEvolumeValue + (SEsoundConvert * multiple + minSEVolume);
        }
    }
}
