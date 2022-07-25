using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField]
    private AudioSource audioBGMSource;


    [SerializeField]
    private AudioSource audioEnemySESource;
    [SerializeField]
    private AudioSource audioPlayerSESource;
    [SerializeField]
    private AudioSource audioMateSESource;
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
    private AudioClip MateAttackSound;
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
        audioPlayerSESource.clip = playerWalkSound;
        audioPlayerSESource.Play();
    }

    public void PlayerAttackAudio()
    {
        audioPlayerSESource.clip = playerAttackSound;
        audioPlayerSESource.Play();
    }

    public void BossAttackAudio()
    {
        audioEnemySESource.clip = BossAttackSound;
        audioEnemySESource.Play();
    }

    public void EnemyAttackAudio()
    {
        audioEnemySESource.clip = EnemyAttackSound;
        audioEnemySESource.Play();
    }

    public void MateAttackAudio()
    {
        audioMateSESource.clip = MateAttackSound;
        audioMateSESource.Play();
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

    public void StopBossBGM()
    {
        if (audioBGMSource.isPlaying)
        {
            audioBGMSource.Stop();
        }
        audioBGMSource.clip = globalBGMSound;
        audioBGMSource.Play();
    }
}
