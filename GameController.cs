using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class GameController : MonoBehaviour
{
    List<ObjectController> objects;
    PlayerController playerController;
    public int currentLevel;
    public GameObject fadePanel;
    public float fadeoutTime = 0.25f;
    bool counting = true;
    public float tmpTime;
    public float timerTime;
    public int level;
    public int maxLevels;
    public Image heatBar;
    public Image timeBar;
    public float maxTime = 60;
    public TextMeshProUGUI timeText;

    private void Awake() {
        objects = new List<ObjectController>();
        objects.AddRange(FindObjectsOfType<ObjectController>());
        playerController = FindObjectOfType<PlayerController>();
        AddTime(maxTime);
    }

    private void AddTime(float extratime) {
        timerTime += extratime;
    }

    public void ObjectPlaced() {
        if(objects.FindAll(o => o.placed == true).Count == objects.Count) {
            playerController.OnLevelEnded();
          //  TimeStop();
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
        // HeatBar.fillAmount(playerController.GetFuelPercentage());
        timeBar.fillAmount = GetTimePercent();
        timeText.text = timerTime.ToString();
    }
    public void Timer() {
        if (counting) {
            if (tmpTime>=1) {
                timerTime--;
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
    public float GetTimePercent() {
        return timerTime /  maxTime ;

    }
}
