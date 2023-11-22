using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Button : MonoBehaviour
{
    [Tooltip("Maximum distance for the button interaction.")]
    public float maxDistance = 5f;

    [Tooltip("List of objects bound to this button.")]
    public List<MonoBehaviour> boundObjects = new List<MonoBehaviour>();

    [Tooltip("Delay before triggering the final triggers.")]
    public float delayBeforeFinalTriggers = 0f;
    
    [Tooltip("List of final triggers that will run after bound triggers.")]
    public List<MonoBehaviour> finalTriggers = new List<MonoBehaviour>();

    [Tooltip("Sound played when the button is pressed.")]
    public AudioClip buttonSound;

    [Tooltip("Key code to trigger the button.")]
    public KeyCode inputKey;

    private List<ITrigger> _mBoundTriggers = new List<ITrigger>();
    private List<ITrigger> _mFinalTriggers = new List<ITrigger>();
    private AudioSource _audioSource;

    private bool _boundTriggersCompleted = false;

    void Start()
    {
        _mBoundTriggers = boundObjects.OfType<ITrigger>().ToList();
        boundObjects.Clear();

        _mFinalTriggers = finalTriggers.OfType<ITrigger>().ToList();
        finalTriggers.Clear();

        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, maxDistance) && hit.transform == this.transform && Input.GetKeyDown(inputKey))
        {
            _audioSource.PlayOneShot(buttonSound);

            // Start the TriggerBoundObjects coroutine without waiting for completion
            StartCoroutine(TriggerBoundObjects());
        }
    }

    IEnumerator TriggerBoundObjects()
    {
        float totalWaitForCompletionDuration = 0f;
        bool narrationTriggerEncountered = false;

        foreach (var trigger in _mBoundTriggers)
        {
            trigger.Trigger();

            if (trigger.Type == Fx.Narration)
            {
                narrationTriggerEncountered = true;
                // Add the duration of the Narration trigger
                totalWaitForCompletionDuration += trigger.Duration;
            }

            if (!narrationTriggerEncountered && trigger.WaitForCompletion)
            {
                // If WaitForCompletion is true and narration has not been encountered,
                // include the duration in the totalWaitForCompletionDuration
                totalWaitForCompletionDuration += trigger.Duration;
            }

            if (trigger.WaitForCompletion)
            {
                yield return new WaitForSeconds(trigger.Duration);
            }
        }

        // Mark bound triggers as completed
        _boundTriggersCompleted = true;

        // Check if both bound triggers and final triggers are completed
        if (_boundTriggersCompleted)
        {
            // Calculate the time to wait before triggering final triggers
            float delay = totalWaitForCompletionDuration + delayBeforeFinalTriggers;

            // Delay before triggering final triggers
            yield return new WaitForSeconds(delay);

            foreach (var finalTrigger in _mFinalTriggers)
            {
                finalTrigger.Trigger();

                if (finalTrigger.WaitForCompletion)
                {
                    yield return new WaitForSeconds(finalTrigger.Duration);
                }
            }
        }
    }
}
