using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    public void DealDamage(int damage, List<Transform> targetList);
}
