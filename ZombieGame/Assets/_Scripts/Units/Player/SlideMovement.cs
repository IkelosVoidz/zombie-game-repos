using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlideMovement : MonoBehaviour
{
    [Header("References")]

    [SerializeField, Tooltip("")] private Transform _orientation;
    [SerializeField, Tooltip("")] private Transform _playerObj;
    private Rigidbody rb;
    private PlayerMovement _player;


    [Header("Sliding")]

    [SerializeField, Tooltip("")] private float _maxSlideTime;
    [SerializeField, Tooltip("")] private float _slideForce;
    private float _slideTimer;

    [SerializeField, Tooltip("")] private float _slideYScale;
    private float _startYScale;


    private bool _sliding;

   
    public void OnSlide(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && (_player._moveAxis.x != 0 || _player._moveAxis.y != 0))
        {
            StartSlide();
        }
        else
        {
            StopSlide();
        }
    }



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        _player = GetComponent<PlayerMovement>();
        _startYScale = _playerObj.localScale.y;
    }

  
    private void FixedUpdate() 
    {
        if (_sliding)
            SlidingMovement();
        
    }

    private void StartSlide()
    {
        _sliding = true;

        _playerObj.localScale = new Vector3(_playerObj.localScale.x,_slideYScale, _playerObj.localScale.z);
        rb.AddForce(Vector3.down*5f,ForceMode.Impulse);

        _slideTimer = _maxSlideTime;
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = _orientation.forward * _player._moveAxis.y + _orientation.right * _player._moveAxis.x;

        rb.AddForce(inputDirection.normalized*_slideForce,ForceMode.Force);

        _slideTimer -= Time.deltaTime;

        if (_slideTimer < 0)
            StopSlide();
    }

    private void StopSlide()
    {
        _sliding = false;
        _playerObj.localScale = new Vector3(_playerObj.localScale.x, _startYScale, _playerObj.localScale.z);
    }

}
