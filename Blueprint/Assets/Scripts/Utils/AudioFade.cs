using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFade : MonoBehaviour {
    public float fadeInDelay;
    public float fadeInPeriod;
    public float fadeOutDelay;
    public float fadeOutPeriod;
    public float maxVolume;
    private AudioSource audioSource;
    
    void Start() {
        audioSource = gameObject.GetComponent<AudioSource>();
        StartCoroutine(ManageFadeIn());
        StartCoroutine(ManageFadeOut());
    }

    void Update() {
        
    }

    private IEnumerator ManageFadeIn() {
        yield return new WaitForSeconds(fadeInDelay);

        float framePeriod = (1.0f / 60.0f);
        float speed = maxVolume * framePeriod / fadeInPeriod;

        for (float volume = 0.0f; volume < maxVolume; volume += speed) {
            audioSource.volume = volume;
            yield return new WaitForSeconds(framePeriod);
        }
        audioSource.volume = maxVolume;
    }

    private IEnumerator ManageFadeOut() {
        yield return new WaitForSeconds(fadeOutDelay);

        float framePeriod = (1.0f / 60.0f);
        float speed = maxVolume * framePeriod / fadeOutPeriod;

        for (float volume = maxVolume; volume > 0.03f; volume -= speed) {
            audioSource.volume = volume;
            yield return new WaitForSeconds(framePeriod);
        }
        audioSource.volume = 0.0f;
    }
}
