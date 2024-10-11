using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class BasicAttackSO : ScriptableObject
{
    [SerializeField] private Sprite icon;

    [SerializeField] private string skillName, description;

    [SerializeField] private int damage, range;

    public Sprite Icon { get => icon; }

    public string SkillName { get => skillName; }

    public string Description { get => description; }

    public int Damage { get => damage; }

    public int Range { get => range; }
}
