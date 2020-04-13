using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity {

    public virtual void TakeDamage(int damagePoint){}

    public virtual Vector3 GetEntityViewPosition() { return Vector3.zero; }
}
