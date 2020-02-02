using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject pausePanel, gameoverPanel, instructionsPanel;
    private bool _paused, showingInstructions;
    public bool credits;

    // Start is called before the first frame update
    void Start()
    {
        SetPauseGame(false);
        ToggleInstructions(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (credits) {
                GoStartMenu();
            } else {

            SetPauseGame(!_paused);
            }
        }
    }
    public void SetPauseGame(bool pause) {
        if (pausePanel) {
            _paused = pause;
            Time.timeScale = pause ? 0 : 1;
            pausePanel.SetActive(pause);
        }
    }

    public void SetGameOver(float time) {
        Invoke(nameof(SetGameOver), time);
    }
    public void SetGameOver() {
        if (gameoverPanel) {
            Time.timeScale = 1;
            _paused = true;
            gameoverPanel.SetActive(true);
        }
    }

    public void startGame() {
        SceneManager.LoadScene(1);
    }
    public void GoStartMenu() {
        SceneManager.LoadScene(0);
    }

    public void exitGame() {
        Application.Quit();
    }

    public void ToggleInstructions(bool showing) {
        if(instructionsPanel)
            this.instructionsPanel.SetActive(showing);
    }
}
