using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void DamageReceived(int damageReceived, GameObject damageFrom);

    public void ShowDamageText(int damageAmount);
}
