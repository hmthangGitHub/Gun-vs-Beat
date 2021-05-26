using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    public float beatPerDistance = 25;
    public float bpm = 60;

    public float speed;
    public bool isStarted = false;
    private void Awake()
    {
        speed = beatPerDistance * bpm / 60;
    }

    private void Update()
    {
        if (Input.anyKey)
        {
            isStarted = true;
        }
        if (isStarted)
        {
            this.transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
        }
    }
}
