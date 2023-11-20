using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]  // This line ensures that an AudioSource component is added when this script is added to a GameObject

// Button class that can be attached to a GameObject in Unity
public class Button : MonoBehaviour
{
    //Unity defined Variables
    public float maxDistance = 5f;
    public List<MonoBehaviour> boundObjects = new List<MonoBehaviour>();
    public AudioClip buttonSound; // Public AudioClip variable for the sound effect
    public KeyCode inputKey;
    
    //Private Variables
    private List<ITrigger> _mBoundTriggers = new List<ITrigger>(); // List to hold the triggers bound to this button
    private AudioSource _audioSource; // Private AudioSource variable
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        // Filter boundObjects to only include MonoBehaviours that implement the ITrigger interface
        _mBoundTriggers = boundObjects.OfType<ITrigger>().ToList();
        boundObjects.Clear();
        
        _audioSource = GetComponent<AudioSource>(); // Initialize the AudioSource component
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Check if a raycast from the camera in the direction of the mouse cursor hits this button and if 'E' key was pressed
        if (Physics.Raycast(ray, out hit, maxDistance) && hit.transform == this.transform && Input.GetKeyDown(KeyCode.E))
        {
            _audioSource.PlayOneShot(buttonSound);
            StartCoroutine(TriggerBoundObjects());
        }
    }
    
    IEnumerator TriggerBoundObjects()
    {
        foreach (var trigger in _mBoundTriggers)
        {
            trigger.Trigger();

            if (trigger.WaitForCompletion)
            {
                yield return new WaitForSeconds(trigger.Duration);
            }
        }
    }
}