using System;
using UnityEngine;


public class DoorController : MonoBehaviour, IInteractable
{

    [SerializeField, Tooltip("Whether the door can be opened or not by default")] private bool _isBlocked = false;
    [SerializeField, Tooltip("Whether the door will open towards the player when seen from the outside")] private bool _opensOutwards = true;
    [SerializeField, Range(0, 180), Tooltip("How much the door will rotate, in degrees")] private float _degToTurn = 120f;
    [SerializeField, Tooltip("Total time it takes to open/close the door")] private float _openTime = 0.5f;

    private Quaternion initialRotation;
    private Quaternion openRotation;
    private Quaternion actualRotation;
    private AudioSource audioDoorlocked;

    private bool isOpen = false;
    private float currentTime = 0.0f;

    void Start()
    {
        audioDoorlocked = GetComponent<AudioSource>();
        initialRotation = transform.rotation;
        actualRotation = transform.rotation;

        if (_opensOutwards)
        {
            openRotation = initialRotation * Quaternion.Euler(0, _degToTurn, 0);
        }
        else
        {
            openRotation = initialRotation * Quaternion.Euler(0, -_degToTurn, 0);
        }
    }

    //interface method
    public void Interact()
    {
        if (_isBlocked)
        {
            audioDoorlocked.Play();
        }
        else
        {
            actualRotation = transform.rotation;
            isOpen = !isOpen;
            currentTime = _openTime;
        }
    }

    public void ChangeBlockedStatus()
    {
        _isBlocked = !_isBlocked;
    }

    //This will only work if the Door is locked
    public void InteractKeepingBlockedStatus()
    {
        if (!_isBlocked) Debug.Log("The door you are trying to open while maintaining blocked status is not blocked.");

        ChangeBlockedStatus();
        Interact();
        ChangeBlockedStatus();
    }

    private void Update()
    {
        if (currentTime > 0.0f)
        {
            if (isOpen)
            {
                transform.rotation = Quaternion.Lerp(actualRotation, openRotation, 1.0f - (currentTime / _openTime));
            }
            else
            {
                transform.rotation = Quaternion.Lerp(actualRotation, initialRotation, 1.0f - (currentTime / _openTime));
            }

            currentTime -= Time.deltaTime;
        }
    }
}


