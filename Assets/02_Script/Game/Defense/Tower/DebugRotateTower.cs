using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRotateTower : TargetRotateTower
{
    protected override void DoAttack()
    {

        Debug.Log("Attack");

    }

}
