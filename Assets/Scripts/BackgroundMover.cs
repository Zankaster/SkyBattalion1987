using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMover : MonoBehaviour{

    public float speed = 5f;
    public int horizontalThresholdForDisable = -650;

    void Update() {
        if (GameManager.instance.GetGameStatus() == GameStatus.Continue)
            return;
        transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
        if (transform.position.x <= horizontalThresholdForDisable)
            gameObject.SetActive(false);
    }
}
