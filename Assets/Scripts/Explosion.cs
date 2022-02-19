using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Explosion : MonoBehaviour
{
    public bool alterPitch = true;
    AudioSource audioSource;
    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void DisableOnAnimationEnd() {
        gameObject.SetActive(false);
    }
    
    private void OnEnable() {
        if(alterPitch)
            audioSource.pitch = 1 + Random.Range(-0.2f, 0.2f);
        audioSource.PlayOneShot(audioSource.clip);
    }
}
