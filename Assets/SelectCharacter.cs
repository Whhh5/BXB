using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacter : MonoBehaviour
{
    [SerializeField]
    private Image imgSelect1, imgSelect2;
    [SerializeField]
    private Button btnSelect1, btnSelect2, closeBtn;
    // Start is called before the first frame update
    void Start()
    {
        imgSelect1.enabled = false;
        imgSelect2.enabled = false;
        btnSelect1.onClick.AddListener(OnBtnSelect1);
        btnSelect2.onClick.AddListener(OnBtnSelect2);
        closeBtn.onClick.AddListener(OnCloseBtn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnBtnSelect1()
    {
        imgSelect1.enabled = true;
    }

    private void OnBtnSelect2()
    {
        imgSelect2.enabled = true;
    }

    private void OnCloseBtn ()
    {
        
    }
}
