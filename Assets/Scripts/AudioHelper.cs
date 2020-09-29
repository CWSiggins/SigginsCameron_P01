using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioHelper
{
    public static AudioSource PlayClip2D(AudioClip clip, float volume)
    {
        //create 
        GameObject audioObject = new GameObject("Audio2d");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        //configure
        audioSource.clip = clip;
        audioSource.volume = volume;

        //activate
        audioSource.Play();
        Object.Destroy(audioObject, clip.length);
        //return in case the call wants to do other things
        return audioSource;
    }
}
