using Godot;
using System;

public partial interface IEnemy
{
    public void Die();
    public void TakeDamage() => Die();
}
