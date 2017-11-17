using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    public float slowdownFactor = 0.05f;
    public float slowdownLength = 2f;
    private bool slowDownActive;

    void Start()
    {
        slowDownActive = false;
    }

    void Update()
    {
        //Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
        //Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    }

    public void doSlowMotion()
    {
        if (slowDownActive == false)
        {
            //slows down time
            Time.timeScale = slowdownFactor;
            Time.fixedDeltaTime = Time.timeScale * .02f;
            slowDownActive = true;
        }
        else if(slowDownActive == true)
        {
            //accelerates time
            Time.timeScale = 1f;
            slowDownActive = false;
        }
    }
}
