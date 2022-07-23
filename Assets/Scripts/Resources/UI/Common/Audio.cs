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
    private int BGMvolumeValue = 50, SEvolumeValue = 50;
    private int minVolume = -40;
    private int soundConvert;

    // Start is called before the first frame update

    private void Awake()
    {
        
    }
    void Start()
    {
        soundConvert = Mathf.Abs(minVolume) / 10 - 10;
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
            sliderBGM.value = BGMvolumeValue + (soundConvert * multiple + minVolume);
        }
    }

    private void OnBGMBtnUpClick()
    {
        if (BGMvolumeValue <= 90)
        {
            BGMvolumeValue += 10;
            int multiple = BGMvolumeValue / 10;
            sliderBGM.value = BGMvolumeValue + (soundConvert * multiple + minVolume);
        }
    }

    private void OnSEBtnDownClick()
    {
        if (SEvolumeValue >= 10)
        {
            SEvolumeValue -= 10;
            int multiple = SEvolumeValue / 10;
            sliderSE.value = SEvolumeValue + (soundConvert * multiple + minVolume);
        }
    }

    private void OnSEBtnUpClick()
    {
        if (SEvolumeValue <= 90)
        {
            SEvolumeValue += 10;
            int multiple = SEvolumeValue / 10;
            sliderSE.value = SEvolumeValue + (soundConvert * multiple + minVolume);
        }
    }
}
