using GameDevTV.Inventories;
using RPG.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthPotion", menuName = "Consumables/Health Potion")]
public class HealthPotion : ActionItem
{
    [SerializeField] float amountToHeal;
    public override void Use(GameObject user)
    {
        var userHealth = user.GetComponent<Health>();
        // || (userHealth.GetHealthPoints() < userHealth.GetMaxHealthPoints() )
        //este check em cima devia ser feito no playerController ou no action store para nao usar uma poção se ja tiver full health!
        if (!userHealth.IsDead() )
        {
            user.GetComponent<Health>().Heal(amountToHeal);
        }
    }
}
