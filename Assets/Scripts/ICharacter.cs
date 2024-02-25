using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public interface ICharacter
{
    public float Health { get; set; }
    public float MoveSpeed { get; set; }

    void Move(Vector2 moveDir);

    void Attack();

    public void TakeDamage(float amount);

    public void Die();
}
