using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class PlayerHealthBar : MonoBehaviour
    {

        public Slider slider;
        public Health health;

        private void Start() {
            setMaxHealth(health.GetMaxHealthPoints());
        }
        private void Update() {
            slider.value = health.GetHealthPoints();
        }

        public void setMaxHealth(float maxHealth)
        {
            slider.maxValue = maxHealth;
        }
    }
}
