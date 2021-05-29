using Cysharp.Threading.Tasks;
using SonicBloom.Koreo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayController : MonoBehaviour
{
    public AudioSource audioSource;
    public UICountdown uiCountDown;
    bool isPlayed = false;
    public float advanceTime;
    public float currentAdvanceTime;
    public NoteSpawner nodeSpawner;
    public Scroller scroller;

    public EnemySpawner enemySpawner;

    Koreography playingKoreo;
    

    public int DelayedSampleTime
    {
        get
        {
            // Offset the time reported by Koreographer by a possible leadInTime amount.
            return playingKoreo.GetLatestSampleTime() - (int)(currentAdvanceTime * SampleRate);
        }
    }

    public int SampleRate
    {
        get
        {
            return playingKoreo.SampleRate;
        }
    }

    private void Start()
    {
        playingKoreo = Koreographer.Instance.GetKoreographyAtIndex(0);
    }
    private void FixedUpdate()
    {
        if (Input.anyKey && !isPlayed)
        {
            uiCountDown.Out().Forget();
            isPlayed = true;
            currentAdvanceTime = advanceTime;
            enemySpawner.StartSpawn(this.advanceTime, this.playingKoreo.SampleRate);
        }

        if (isPlayed)
        {
            if (currentAdvanceTime >= 0)
            {
                currentAdvanceTime -= Time.deltaTime;
                if (currentAdvanceTime < 0)
                {
                    audioSource.time = -currentAdvanceTime;
                    audioSource.Play();
                }
            }
            enemySpawner.UpdateSpawning(this.DelayedSampleTime);
        }
    }
}
