using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;


public class DoorController : MonoBehaviour , IInteractable
{
    [SerializeField] private bool _isBlocked = false;
    [SerializeField] private bool _opensOutwards = true;
    [SerializeField] private float _degToTurn = 120f;
    [SerializeField] private int _speed = 200;

    private const float LERP_TIME = 0.01f;
    private float t = 0f;
    private float _startingAngle;
    private bool _isOpen = false;
    

    private void Start()
    {
        _startingAngle = transform.localEulerAngles.y;
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
        if (_isOpen)
        {
            Vector3 targetRotation = new Vector3(0, _degToTurn + _startingAngle, 0);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetRotation, LERP_TIME * Time.deltaTime * _speed);
        }
        else
        {
            Vector3 targetRotation = new Vector3(0, _startingAngle, 0);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetRotation, LERP_TIME * Time.deltaTime * _speed);
        }

        t = Mathf.Lerp(t, LERP_TIME, Time.deltaTime * _speed);

        if (t >= 1f)
        {
            t = 0f;
        }
    }
}

