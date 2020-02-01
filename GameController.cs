﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    List<ObjectController> objects;
    PlayerController playerController;
    public int currentLevel;
    public GameObject fadePanel;
    public float fadeoutTime = 0.25f;
    bool counting = false;
    float tmpTime;
    float timerTime;
    public int level;
    public int maxLevels;
    private void Awake() {
        objects = new List<ObjectController>();
        objects.AddRange(FindObjectsOfType<ObjectController>());
        playerController = FindObjectOfType<PlayerController>();
    }

    public void ObjectPlaced() {
        if(objects.FindAll(o => o.placed == true).Count == objects.Count) {
            playerController.OnLevelEnded();
            TimeStop();
            Invoke(nameof(DoFadeout),playerController.outAnimationTime);
            Invoke(nameof(NextLevel), playerController.outAnimationTime + fadeoutTime);
        }
    }

    private void TimeStop() {
        counting = false;
    }

    public void LetsStart() {
        counting = true;
    }
    public void DoFadeout() {
        fadePanel.SetActive(true);
        fadePanel.GetComponent<Animator>().Play("Fadeout");
    }
    public void DoFadein() {
        fadePanel.SetActive(true);
        fadePanel.GetComponent<Animator>().Play("Fadein");
    }

    private void FixedUpdate() {
        Timer();
    }
    private void LateUpdate() {
        
    }
    public void Timer() {
        if (counting) {
            if (tmpTime==1) {
                timerTime++;
                tmpTime = 0;
            }
            tmpTime += Time.fixedDeltaTime;
        }
    }

    public void NextLevel() {
        if(this.level < this.maxLevels-1) {
            SceneManager.LoadScene(this.level++);
        }
    }
}