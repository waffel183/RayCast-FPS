﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : MonoBehaviour {

    public Animator animator;
    public GameObject scopeOverlay;
    public GameObject weaponCamera;
    public Camera mainCamera;

    public float scopedFOV = 15f;
    private float normalFOV;

    public bool isScoped = false;

    private void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            isScoped = !isScoped;
            animator.SetBool("Scoped", isScoped);

            if (isScoped)
            {
                StartCoroutine(OnScoped());
            }
            else
            {
                onUnScoped();
            }
        }
    }

    //when returning from scoped view
    void onUnScoped()
    {
        scopeOverlay.SetActive(false);
        weaponCamera.SetActive(true);

        mainCamera.fieldOfView = normalFOV;
    }

    //when having a scoped view
    IEnumerator OnScoped()
    {
        yield return new WaitForSeconds(.15f);

        scopeOverlay.SetActive(true);
        weaponCamera.SetActive(false);

        normalFOV = mainCamera.fieldOfView;
        mainCamera.fieldOfView = scopedFOV;
    }
}
