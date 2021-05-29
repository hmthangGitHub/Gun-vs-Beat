using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    public float beatPerDistance = 10;
    public float bpm = 60;
    public float speed;
    public bool isStarted = false;
    private void Awake()
    {
        speed = beatPerDistance * bpm / 60;
    }

    private void FixedUpdate()
    {
        if (isStarted)
        {
            this.transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
        }
    }

    public void StartScrolling(float speed)
    {
        isStarted = true;
        this.speed = speed;
    }
}
