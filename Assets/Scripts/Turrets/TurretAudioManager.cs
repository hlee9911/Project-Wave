using UnityEngine.Audio;
using System;
using UnityEngine;

public class TurretAudioManager : MonoBehaviour
{
    public TurretSound[] sounds;
    private void Awake()
    {
        foreach (TurretSound s in sounds)
        {
           s.source = gameObject.AddComponent<AudioSource>();
           s.source.clip = s.clip;
           s.source.volume = s.volume;
           s.source.pitch = s.pitch;
        }
    }

    public void PlayTurretSound (string name)
    {
        TurretSound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
    public void PlayWaveSound(string name)
    {
        TurretSound s = Array.Find(sounds, sound => sound.name == name);
        s.source.PlayOneShot(s.clip);
    }
}
