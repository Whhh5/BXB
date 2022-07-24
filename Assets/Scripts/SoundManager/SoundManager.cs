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
    private AudioClip globalBGMSound;
    [SerializeField]
    private AudioClip playerWalkSound; /*playerAttackSound;*/
    [SerializeField]
    private AudioClip playerAttackSound;
    [SerializeField]
    private AudioClip BossAttackSound;
    [SerializeField]
    private AudioClip EnemyAttackSound;
    [SerializeField]
    private AudioClip BossBgmSound;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioBGMSource.clip = globalBGMSound;
        audioBGMSource.Play();
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

    public void PlayerAttackAudio()
    {
        audioSESource.clip = playerAttackSound;
        audioSESource.Play();
    }

    public void BossAttackAudio()
    {
        audioSESource.clip = BossAttackSound;
        audioSESource.Play();
    }

    public void EnemyAttackAudio()
    {
        audioSESource.clip = EnemyAttackSound;
        audioSESource.Play();
    }

    public void EntryBossEnviroment()
    {
        if (audioBGMSource.isPlaying)
        {
            audioBGMSource.Stop();
        }
        audioBGMSource.clip = BossBgmSound;
        audioBGMSource.Play();
    }
}
