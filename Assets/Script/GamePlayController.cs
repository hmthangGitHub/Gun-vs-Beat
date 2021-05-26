using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayController : MonoBehaviour
{
    public AudioSource audioSource;
    public UICountdown uiCountDown;
    bool isPlayed = false;

    private void Update()
    {
        if (Input.anyKey && !isPlayed)
        {
            uiCountDown.Out().Forget();
            audioSource.Play();
            isPlayed = true;
        }
    }
}
