using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class GameController : MonoBehaviour {
    List<ObjectController> objects;
    PlayerController playerController;
    MenuController menuController;
    public GameObject fadePanel;
    public GameObject endFadePanel;
    float fadeoutTime = 0.50f;
    public float ligthTime = 1.25f;
    bool counting = true;
    public float tmpTime;
    public float timerTime;
    public int level;
    public int maxLevels;
    public Image heatBar;
    public Image timeBar;
    public float maxTime = 60;
    public TextMeshProUGUI timeText;
    public Animator EndRainbow;
    public Animator EndLigth;

    private AudioManager audioManagerScript;
    //public Animator EndRainbow;

    private void Awake() {
        objects = new List<ObjectController>();
        objects.AddRange(FindObjectsOfType<ObjectController>());
        playerController = FindObjectOfType<PlayerController>();
        menuController = FindObjectOfType<MenuController>();
        AddTime(maxTime);
        audioManagerScript = FindObjectOfType<AudioManager>();

    }
    private void Start() {
        LetsStart();
    }
    private void AddTime(float extratime) {
        timerTime += extratime;
    }

    public void ObjectPlaced() {
        if (objects.FindAll(o => o.placed == true).Count == objects.Count) {
            playerController.OnLevelEnded();
            //  TimeStop();
            Invoke(nameof(DoFadeout), playerController.outAnimationTime);
            Invoke(nameof(NextLevel), playerController.outAnimationTime + fadeoutTime);
        }
    }

    private void TimeStop() {
        counting = false;
    }

    public void LetsStart() {
        playerController.OnLevelStart();
        Invoke(nameof(LetsStartP2), level == 1 ? (playerController.firstStartAnimationTime / 1.5f) : playerController.startAnimationTime );
    }
    public void LetsStartP2() {
        counting = true;
        objects.ForEach(o => o.letsPlay());
        playerController.GetComponent<Animator>().applyRootMotion = true;

    }
    public void DoFadeout() {
        fadePanel.SetActive(true);
        if(this.level != maxLevels)
        fadePanel.GetComponent<Animator>().Play("Fadeout");
        else {
            endFadePanel.SetActive(true);
        endFadePanel.GetComponent<Animator>().Play("Fadeout");
        }
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
            if (tmpTime >= 1) {
                timerTime--;
                tmpTime = 0;
            }
            tmpTime += Time.fixedDeltaTime;
            if (timerTime <= 0) {
                GameOverFail();
            }
        }
    }

    public void NextLevel() {
        if (this.level < this.maxLevels) {
            SceneManager.LoadScene("Level" + (this.level + 1));
        }
            if (this.level == this.maxLevels) {
        Debug.Log("Nextoooo");
                SceneManager.LoadScene("Credits");
            }
        }
    public float GetTimePercent() {
        return timerTime / maxTime;
    }

    public void GameOverFail() {
        Debug.Log("K BYE");
        audioManagerScript.playBoredAudio();
        counting = false;
        playerController.OnLevelEnded();
        playerController.LaunchAnimDie();
        menuController.SetGameOver(playerController.outAnimationTime);
    }

    public void GameEnded() {
        playerController.OnLevelEnded();
        EndLigth.enabled = true;
        EndRainbow.Play("Rainbow");
        audioManagerScript.startEndMusic();
        Invoke(nameof(DoFadeout), playerController.outAnimationTime);
        Invoke(nameof(NextLevel), playerController.outAnimationTime+fadeoutTime * 2);
    }
}
