using System;
using UnityEngine;


public class DoorController : MonoBehaviour, IInteractable
{

    [SerializeField, Tooltip("Whether the door can be opened or not by default")] private bool _isBlocked = false;
    [SerializeField, Tooltip("Whether the door will open towards the player when seen from the outside")] private bool _opensOutwards = true;
    [SerializeField, Range(0, 180), Tooltip("How much the door will rotate, in degrees")] private float _degToTurn = 120f;
    [SerializeField, Tooltip("Total time it takes to open/close the door")] private float _openTime = 0.5f;

    [SerializeField, Tooltip("Audio of the door locked")] private AudioClip _audioDoorLocked;
    [SerializeField, Tooltip("Audio of the door opening")] private AudioClip _audioDoorOpen;
    [SerializeField, Tooltip("Audio of the door closing")] private AudioClip _audioDoorClose;

    private AudioSource audioSource;

    private Quaternion initialRotation;
    private Quaternion openRotation;
    private Quaternion actualRotation;

    private bool isOpen = false;
    private float currentTime = 0.0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

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
            audioSource.clip = _audioDoorLocked;
            audioSource.Play();
        }
        else
        {
            actualRotation = transform.rotation;
            isOpen = !isOpen;
            currentTime = _openTime;

            if (isOpen)
            {
                audioSource.clip = _audioDoorOpen;
                audioSource.Play();
            }
            else
            {
                audioSource.clip = _audioDoorClose;
                audioSource.Play();
            }
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

    public bool IsOpen()
    {
        return isOpen;
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
        else
        {
            if (isOpen)
            {
                transform.rotation = openRotation;
            }
            else
            {
                transform.rotation = initialRotation;
            }
        }
    }
}