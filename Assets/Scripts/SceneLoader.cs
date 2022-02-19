using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    bool buttonPressed = false;
    public string sceneName;
    public bool playSound;
    public int secondsDelay;
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        if (Input.anyKey && !buttonPressed) {
            buttonPressed = true;
            if(playSound)
                GetComponent<AudioSource>().Play();
            StartCoroutine(LoadScene());
        }
    }

    IEnumerator LoadScene() {
        yield return new WaitForSeconds(secondsDelay);
        SceneManager.LoadSceneAsync(sceneName);
    }
}
