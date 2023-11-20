﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
[RequireComponent(typeof(AudioSource))]
public class LightEffect : MonoBehaviour, ITrigger
{
    
    private Light _lightObject; //The light object to be controlled
    private AudioSource _soundEmitter; //The AudioSource to play the sound
    
    [Tooltip("The sound to be played when the light is turned on")]
    public AudioClip soundClip;

    [Tooltip("The speed at which the light fades in")]
    public float fadeSpeed = 1f;

    [Tooltip("Whether other effects should wait for this one to complete")]
    public bool waitForCompletion = false;

    [Tooltip("Whether the light starts on")]
    public bool startOn = false;

    private bool _isOn = false; // Whether the light is currently on

    void Start()
    {
        _lightObject = GetComponent<Light>();
        _soundEmitter = GetComponent<AudioSource>();
        _isOn = startOn;
        if (_isOn)
        {
            _lightObject.intensity = 1;
        }
        else
        {
            _lightObject.intensity = 0;
        }
    }

    public float Duration => fadeSpeed;
    public bool WaitForCompletion => waitForCompletion;
    public Fx Type => Fx.Light;

    public void Trigger()
    {
        if (_isOn)
        {
            _lightObject.intensity = 0;
            _isOn = false;
        }
        else
        {
            StartCoroutine(FadeInLight());
            if (_soundEmitter != null && soundClip != null)
            {
                _soundEmitter.clip = soundClip;
                _soundEmitter.Play();
            }
            _isOn = true;
        }
    }

    private IEnumerator FadeInLight()
    {
        float elapsedTime = 0;

        while (elapsedTime < fadeSpeed)
        {
            _lightObject.intensity = Mathf.Lerp(0, 1, (elapsedTime / fadeSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _lightObject.intensity = 1;
    }
}