using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDealDamage
{
    public void DealDamage(int damage, List<Transform> targetList);
    public int CalcDamage(SkillSO damageSkill);
}
