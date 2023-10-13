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
    int SPEED = 100;
    float t = 0f;
    
    private bool _hasToMove = false;


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

        _hasToMove = true;
    }

    void Close() {
        _isOpen = false;

        _hasToMove = true;
    }

    public void changeBlockedStatus()
    {
        _isBlocked = !_isBlocked;
    }

    private void Update()
    {
        if (_hasToMove)
        {
            if (_isOpen) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, _degToTurn, 0)), LERP_TIME * Time.deltaTime * SPEED);
            else transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(Vector3.zero), LERP_TIME * Time.deltaTime * SPEED);

            t = Mathf.Lerp(t, LERP_TIME, Time.deltaTime * SPEED);

            if (t >= .7f)
            {
                t = 0f;
            }

            //this.transform.Rotate(Vector3.up, _degToTurn * Time.deltaTime * _speed);

            //Debug.Log("y: " + this.transform.eulerAngles.y);
           /* Debug.Log("deg to turn: " + _degToTurn);

            if (_isOpen && this.transform.eulerAngles.y < _degToTurn)
            {
                this.transform.Rotate(Vector3.up, _degToTurn * Time.deltaTime * _speed);
            }
            else if (!_isOpen && this.transform.eulerAngles.y < _degToTurn)
            else if
                if (this.transform.eulerAngles.y < _degToTurn && this.transform.eulerAngles.y >= 0) {
                //If the rotation of the door is not bigger than the total rotation it has to do to open or close itself.

                if (_isOpen) this.transform.Rotate(Vector3.up, _degToTurn * Time.deltaTime * _speed);
                
                Debug.Log("ESTA ROTANDO");
                Debug.Log("IS OPEN? " + _isOpen);
            }
            else _hasToMove = false;*/
            
        }
    }
}

