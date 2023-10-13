using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;


public class DoorController : MonoBehaviour , IInteractable
{
    [SerializeField] private bool _isBlocked = false;
    [SerializeField] private bool _isOpen = false;
    [SerializeField] private bool _opensOutwards = false;
    [SerializeField] private float _degToTurn = 160f;

    float LERP_TIME = 0.01f;
    int SPEED = 200;
    float t = 0f;


    private void Start()
    {
        if (_opensOutwards) _degToTurn *= -1;
    }

    //interface method
    public void Interact()
    {
        if (!_isBlocked) 
        {
            if (!_isOpen) {
                Open();
            }
            else
            {
                Close();
            }
        }
        
    }

    void Open()
    {
        _isOpen = true;
    }

    void Close() {
        _isOpen = false;
    }

    public void changeBlockedStatus()
    {
        _isBlocked = !_isBlocked;
    }

    private void Update()
    {
        if (_isOpen) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, _degToTurn, 0)), LERP_TIME * Time.deltaTime * SPEED);
        else transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(Vector3.zero), LERP_TIME * Time.deltaTime * SPEED);

        t = Mathf.Lerp(t, LERP_TIME, Time.deltaTime * SPEED);

        if (t >= 1f)
        {
            t = 0f;
        }            
    }
}

