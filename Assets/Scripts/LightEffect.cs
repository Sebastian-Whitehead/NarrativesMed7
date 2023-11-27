using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
[RequireComponent(typeof(AudioSource))]
public class LightEffect : MonoBehaviour, ITrigger
{
    private Light _lightObject; // The light object to be controlled
    private AudioSource _soundEmitter; // The AudioSource to play the sound

    [Tooltip("The sound to be played when the light is turned on")]
    public AudioClip soundClip;

    [Tooltip("The speed at which the light fades in")]
    public float fadeSpeed = 1f;

    [Tooltip("Delay before the light turns on")]
    public float delay = 0f;

    [Tooltip("Whether other effects should wait for this one to complete")]
    public bool waitForCompletion = false;

    [Tooltip("Whether the light starts on")]
    public bool startOn = false;

    [Tooltip("Target light intensity for when in the on state")]
    public float onIntensity = 5000;
    

    private bool _isOn = false; // Whether the light is currently on

    void Start()
    {
        _lightObject = GetComponent<Light>();
        _soundEmitter = GetComponent<AudioSource>();
        _isOn = startOn;
        if (startOn)
        {
            _lightObject.intensity = onIntensity;
        }
        else
        {
            _lightObject.intensity = 0;
            _lightObject.enabled = false;
        }
    }

    public float Duration => fadeSpeed + delay;
    public bool WaitForCompletion => waitForCompletion;
    public Fx Type => Fx.Light;

    public void Trigger()
    {
        if (_isOn)
        {
            _lightObject.intensity = 0;
            _isOn = false;
            _lightObject.enabled = false;
        }
        else
        {
            StartCoroutine(FadeInLightWithDelay());
            _isOn = true;
        }
    }

    private IEnumerator FadeInLightWithDelay()
    {
        yield return new WaitForSeconds(delay);
        _lightObject.enabled = true;
        
        float elapsedTime = 0;
        if (_soundEmitter != null && soundClip != null)
        {
            _soundEmitter.clip = soundClip;
            _soundEmitter.Play();
        }
        while (elapsedTime < fadeSpeed)
        {
            _lightObject.intensity = Mathf.Lerp(0, onIntensity, (elapsedTime / fadeSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _lightObject.intensity = onIntensity;
    }
}