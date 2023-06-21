using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The unit stats.
/// </summary>

[System.Serializable]
public class UnitStats
{
    private int armorModifier;
    private int damageModifier;
    private int health;

    public int ArmorModifier { get => armorModifier; set => armorModifier = value; }
    public int DamageModifier { get => damageModifier; set => damageModifier = value; }
    public int Health { get => health; set => health = value; }
}
