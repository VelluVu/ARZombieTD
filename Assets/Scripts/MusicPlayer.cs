using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

    public List<AudioClip> musics = new List<AudioClip>();
    AudioSource music;
    bool musicIsOn;

    private void Start()
    {
        music = gameObject.GetComponent<AudioSource>();
        musicIsOn = false;
    }

    private void Update()
    {
        if (musicIsOn == false)
        {
            musicIsOn = true;
            music.clip = musics[Random.Range(0, musics.Count)];
            music.Play();
            StartCoroutine(PlayMusic());
        }
    }

    IEnumerator PlayMusic()
    {           
        yield return new WaitForSeconds(music.clip.length);
        musicIsOn = false;
    }
}
