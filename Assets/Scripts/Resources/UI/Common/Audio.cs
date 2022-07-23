using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class Audio : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider slider;
    public Text volumeText;
    public Button btn_down;
    public Button btn_up;
    private int volume_value = 80;

    // Start is called before the first frame update

    private void Awake()
    {
        
    }
    void Start()
    {
        btn_down.onClick.AddListener(OnBtnDownClick);
        btn_up.onClick.AddListener(OnBtnUpClick);
    }

    // Update is called once per frame
    void Update()
    {
        volumeText.text = volume_value.ToString();
    }

    public void SetVolume()
    {
        audioMixer.SetFloat("MainVolume", slider.value);
    }

    private void OnBtnDownClick()
    {
        if (volume_value >= 10)
        {
            volume_value -= 10;
            slider.value = volume_value - 80;
        }
    }

    private void OnBtnUpClick()
    {
        if (volume_value <= 90)
        {
            volume_value += 10;
            slider.value = volume_value - 80;
        }
    }
}
