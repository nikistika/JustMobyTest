using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Logic.Meta.UI.Health
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private Image _healthBar;
        [SerializeField] private TextMeshProUGUI _currentHealthText;
        [SerializeField] private TextMeshProUGUI _maxHealthText;


        public void SetHealth(float health)
        {
            _healthBar.fillAmount = health;
        }

        public void SetMaxHealthText(float health)
        {
            _maxHealthText.text = health.ToString();
        }

        public void SetCurrentHealthText(float health)
        {
            _currentHealthText.text = health.ToString();
        }
    }
}