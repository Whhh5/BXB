using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField]
    private AudioSource audioBGMSource;

    [SerializeField]
    private AudioSource audioSESource;
    [SerializeField]
    private AudioClip playerWalkSound; /*playerAttackSound;*/
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerWalkAudio() 
    {
        audioSESource.clip = playerWalkSound;
        audioSESource.Play();
    }

   /* public void PlayerAttackAudio()
    {
        audioSource.clip = playerAttackSound;
        audioSource.Play();
    }*/
}
