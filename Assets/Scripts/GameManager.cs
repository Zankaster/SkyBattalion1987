using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public int lives = 2;
    NumbersDisplay scoreController, livesController, countdownController;
    ShipController shipController;
    GameObject gameOver, stageComplete, thankYou;
    GameStatus gameStatus;
    int gameOverCountdown;
    Coroutine gameoverCoroutine = null;
    AudioSource musicPlayer;
    public AudioClip bossMusic, stageCompleteMusic;

    private void Start() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(this);
        }
        scoreController = GameObject.FindGameObjectWithTag("Score").GetComponent<NumbersDisplay>();
        livesController = GameObject.FindGameObjectWithTag("Lives").GetComponent<NumbersDisplay>();
        countdownController = GameObject.FindGameObjectWithTag("Countdown").GetComponent<NumbersDisplay>();
        countdownController.transform.parent.gameObject.SetActive(false);
        shipController = FindObjectOfType<ShipController>();
        gameOver = GameObject.FindGameObjectWithTag("GameOver");
        stageComplete = GameObject.FindGameObjectWithTag("StageComplete");
        thankYou = GameObject.FindGameObjectWithTag("ThankYou");
        gameOver.SetActive(false);
        stageComplete.SetActive(false);
        thankYou.SetActive(false);
        scoreController.SetValue(0);
        livesController.SetValue(lives);
        gameStatus = GameStatus.Playing;
        musicPlayer = GetComponent<AudioSource>();
    }

    private void Update() {      
        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        if (gameStatus == GameStatus.Continue) {
            if (Input.GetButtonDown("Fire1")) {
                StopCoroutine(gameoverCoroutine);
                countdownController.transform.parent.gameObject.SetActive(false);
                gameStatus = GameStatus.Playing;
                lives = 2;
                livesController.SetValue(2);
                shipController.Respawn();
            }
            else if (Input.GetButtonDown("Vertical")) {
                gameOverCountdown--;
                if (gameOverCountdown >= 0)
                    countdownController.SetValue(gameOverCountdown);
                else
                    GameOver();
            }
        }
    }

    public void GameOver() {
        gameStatus = GameStatus.GameOver;
        Debug.Log("GameOver");
        countdownController.transform.parent.gameObject.SetActive(false);
        gameOver.SetActive(true);
        StartCoroutine(ReturnToMainMenuAfterSeconds(3));
    }

    public void PlayerDied() {
        if (lives == 0) {
            shipController.StayDead();
            gameStatus = GameStatus.Continue;
            gameoverCoroutine = StartCoroutine(CountDown());
        }
        else {
            lives--;
            livesController.AddValue(-1);
            shipController.Respawn();
        }
    }

    private IEnumerator CountDown() {
        gameStatus = GameStatus.Continue;
        countdownController.transform.parent.gameObject.SetActive(true);
        for(gameOverCountdown = 9; gameOverCountdown >= 0; gameOverCountdown--) {
            countdownController.SetValue(gameOverCountdown);
            yield return new WaitForSeconds(1);
        }
        Time.timeScale = 1;
        GameOver();
    }

    private IEnumerator ReturnToMainMenuAfterSeconds(int seconds) {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("MainMenu");
    }

    public GameStatus GetGameStatus() {
        return gameStatus;
    }

    public void AddScore(int value) {
        scoreController.AddValue(value);
        
    }

    public void PauseGame() {
        Time.timeScale = 0;
    }

    public void StageCompleted() {
        gameStatus = GameStatus.StageCompleted;
        StartCoroutine(StopMusicToVictoryFanfare());
        stageComplete.SetActive(true);
        StartCoroutine(ShowThankYou());
        StartCoroutine(ReturnToMainMenuAfterSeconds(8));
    }

    public void StartBossMusic() {
        StartCoroutine(FadeMusicToBoss());
    }

    IEnumerator FadeMusicToBoss() {
        float elapsedTime = 0, waitTime = 1;
        while (elapsedTime < waitTime) {
            musicPlayer.volume = Mathf.Lerp(1f, 0f, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        musicPlayer.volume = 1;
        musicPlayer.Stop();
        musicPlayer.clip = bossMusic;
        musicPlayer.Play();
    }

    IEnumerator StopMusicToVictoryFanfare() {
        musicPlayer.Stop();
        musicPlayer.loop = false;
        yield return new WaitForSeconds(1f);
        musicPlayer.clip = stageCompleteMusic;
        musicPlayer.Play();
    }

    IEnumerator ShowThankYou() {
        yield return new WaitForSeconds(3f);
        thankYou.SetActive(true);
    }
}
