using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetRotateTower : TowerRoot
{

    protected override void Update()
    {

        base.Update();

        Rotate();

    }

    protected virtual void Rotate()
    {

        if (target == null) return;

        Vector2 dir = transform.position - target.transform.position;
        transform.up = -dir.normalized;

    }

}
