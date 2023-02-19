using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Reborn
{
    public class HealthBar : MonoBehaviour
    {
        public Slider slider;
        public TMP_Text healthText;
        [SerializeField] int maxHealth;

        public void SetMaxHealth(int health)
        {
            slider.maxValue = health;
            slider.value = health;
        }

        public void SetHealth(int health)
        {
            slider.value = health;
            healthText.text = health.ToString();
        }
    }

}
