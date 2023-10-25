using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundTrigger : MonoBehaviour, ITrigger
{
    public AudioClip soundClip;
    public bool repeatSound;  // Defined weather the sound should be looped or not
    public bool isNarration;  


    private AudioSource _audioSource;

    // Interface variables
    public float Duration => soundClip.length;
    public bool WaitForCompletion { get; set; }
    public Fx Type => isNarration ? Fx.Narration : Fx.Sound;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = soundClip;
        _audioSource.loop = repeatSound;  // Sets weather sound source should loop
    }

    // Called by a trigger function
    public void Trigger()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }
    
    // Stops the playing audio source if called
    public void StopSound() 
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
    }
}
