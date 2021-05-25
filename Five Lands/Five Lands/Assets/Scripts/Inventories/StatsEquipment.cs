using GameDevTV.Inventories;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
    public class StatsEquipment : Equipment, IModifierProvider
    {
        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
           foreach(var slot in GetAllPopulatedSlots())
            {
                var item = GetItemInSlot(slot) as IModifierProvider;
                if (item == null) continue; //no caso de nao ter nenhum modifier, nenhum stat!

                foreach(float modifier in item.GetAdditiveModifiers(stat))
                {
                    yield return modifier;
                }

            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            foreach (var slot in GetAllPopulatedSlots())
            {
                var item = GetItemInSlot(slot) as IModifierProvider;
                if (item == null) continue; //no caso de nao ter nenhum modifier, nenhum stat!

                foreach (float modifier in item.GetPercentageModifiers(stat))
                {
                    yield return modifier;
                }

            }
        }
    }
}
