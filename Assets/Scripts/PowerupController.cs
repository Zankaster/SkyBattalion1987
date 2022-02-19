using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{
    public void ConsumePowerup() {
        transform.position = transform.position + Vector3.up * 1000;
        GetComponent<AudioSource>().Play();
        GameManager.instance.AddScore(500);
        StartCoroutine(DisablePowerup());
    }

    IEnumerator DisablePowerup() {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}
