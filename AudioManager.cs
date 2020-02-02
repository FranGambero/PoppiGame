using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour {

    public AudioClip[] _audioClips;
    private AudioSource _audioSource;

    public AudioClip jetpackAudio, catchAudio, dangerAudio, endAudio, deathAudio;
    public AudioSource jetpackSource, playerSource;

    void Awake() {
        _audioSource = GetComponent<AudioSource>();
        jetpackSource.clip = jetpackAudio;
        playerSource.clip = catchAudio;
    }

    private void Update() {
        if (Input.GetKey(KeyCode.Space)) {
            //playJetPack();
            //jetpackSource.Play();
        }
    }


    // Use this for initialization
    void Start() {

        StartCoroutine(PlayMusic());
    }

    IEnumerator WaitForMusicEnd() {
        while (_audioSource.isPlaying) {
            yield return null;
        }
    }

    IEnumerator PlayMusic() {
        for (int i = 0; i < _audioClips.Length; i++) {
            _audioSource.PlayOneShot(_audioClips[i]);
            while (_audioSource.isPlaying)
                yield return null;
        }
    }

    public void PlayCatch() {
        playerSource.clip = catchAudio;
        //playerSource.pitch = Random.Range(-3, 3);
        playerSource.Play();
    }

    public void playDanger() {
        stopJetPack();
        playerSource.clip = dangerAudio;
        playerSource.Play();
    }

    public void stopDanger() {
        playerSource.Stop();
    }

    public void playJetPack() {
        //playerSource.clip = jetpackAudio;
        //playerSource.Play();
        jetpackSource.Play();
    }

    public void stopJetPack() {
        jetpackSource.Stop();
    }

    public void playBoredAudio() {
        Debug.Log("estoy aca");
        _audioSource.Stop();
        playerSource.clip = deathAudio;
        playerSource.Play();
        _audioSource.clip = deathAudio;
        _audioSource.Play();
    }

    public void startEndMusic() {
        jetpackSource.clip = endAudio;
        jetpackSource.Play();
    }

}