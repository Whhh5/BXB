using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLogic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayerWalkPlaySound()
    {
        SoundManager.instance.PlayerWalkAudio();
    }

    public void OnPlayerAttackPlaySound()
    {
        SoundManager.instance.PlayerAttackAudio();
    }

    public void OnBossAttackPlaySound()
    {
        SoundManager.instance.BossAttackAudio();
    }

    public void OnEnemyAttackPlaySound()
    {
        SoundManager.instance.EnemyAttackAudio();
    }

    public void OnMateAttackPlaySound()
    {
        SoundManager.instance.MateAttackAudio();
    }

}
