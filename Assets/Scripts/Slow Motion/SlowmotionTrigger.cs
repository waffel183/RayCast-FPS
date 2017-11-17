using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowmotionTrigger : MonoBehaviour {

    public TimeManager timeManager;
	
	void Update () {
        if (Input.GetMouseButtonDown(4))
        {
            timeManager.doSlowMotion();
        }

    }
}
