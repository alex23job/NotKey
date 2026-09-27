using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySounds : MonoBehaviour
{
    [SerializeField] private AudioClip[] _clips;
    [SerializeField] private AudioSource _source;

    public void PlayClip(int num)
    {
        if (num < _clips.Length)
        {
            _source.clip = _clips[num];
            _source.Play();
        }
    }

    public void PauseSounds()
    {
        _source.Pause();
    }

    public void SetVolume(float vol)
    {
        _source.volume = vol / 100f;
    }
}
