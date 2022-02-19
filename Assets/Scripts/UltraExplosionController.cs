using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltraExplosionController : MonoBehaviour {
    void OnEnable() {
        foreach (Transform t in transform)
            StartCoroutine(ActivateChildOnDelay(t, Random.Range(0f, 1f)));
    }

    IEnumerator ActivateChildOnDelay(Transform child, float delay) {
        yield return new WaitForSeconds(delay);
        child.gameObject.SetActive(true);
    }
}
