using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BulletHit : MonoBehaviour
{
    AudioSource audioSource;
    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }
    public void AnimationEnded() {
        transform.gameObject.SetActive(false);
    }

    private void OnEnable() {
        audioSource.pitch = 1 + Random.Range(-0.1f, 0.1f);
        audioSource.PlayOneShot(audioSource.clip);
    }
}
