using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IZombie
{
    //Public
    void ChangeStateTo(int _state);
    void FootStepEvent();
    void Chase();
    void Attack();
    void TakeDamage();
    void Die();
    void Dance();
    void ToChase();

}

