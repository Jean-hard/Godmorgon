using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity {

    public virtual void TakeDamage(int damagePoint, bool isPlayerAttacking){}

    public virtual Vector3 GetEntityViewPosition() { return Vector3.zero; }

    public virtual void UpdateHealthBar() { }

    public virtual void OnDamage() { }

    public virtual float GetDamageHitDuration() { return 0; }

    public virtual int DoDamage(int damagePoint) { return damagePoint; }

    public virtual bool IsDead() { return false; }
}
