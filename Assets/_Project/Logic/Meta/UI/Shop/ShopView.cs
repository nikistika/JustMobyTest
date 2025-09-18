using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Logic.Meta.UI.Shop
{
    public class ShopView : MonoBehaviour
    {
        public Button UpgradeSpeedButton;
        public Button UpgradeDamageButton;
        public Button ExitButton;
        public Button UpgradeHealthButton;
        public Button ShowShopButton;
        public Button ApplyUpgradeButton;

        [SerializeField] private TextMeshProUGUI _costHealth;
        [SerializeField] private TextMeshProUGUI _costSpeed;
        [SerializeField] private TextMeshProUGUI _costDamage;
        [SerializeField] private TextMeshProUGUI _levelHealth;
        [SerializeField] private TextMeshProUGUI _levelSpeed;
        [SerializeField] private TextMeshProUGUI _levelDamage;
        [SerializeField] private TextMeshProUGUI _points;
        [SerializeField] private Image _shopPopup;

        private void Start()
        {
            HideShop();
        }

        public void SetCostHealth(float value)
        {
            _costHealth.text = $"{value.ToString()}";
        }

        public void SetCostSpeed(float value)
        {
            _costSpeed.text = $"{value.ToString()}";
        }

        public void SetCostDamage(float value)
        {
            _costDamage.text = $"{value.ToString()}";
        }

        public void SetLevelHealth(float value)
        {
            _levelHealth.text = $"{value.ToString()}";
        }

        public void SetLevelSpeed(float value)
        {
            _levelSpeed.text = $"{value.ToString()}";
        }

        public void SetLevelDamage(float value)
        {
            _levelDamage.text = $"{value.ToString()}";
        }

        public void SetPoints(float value)
        {
            _points.text = value.ToString();
        }

        public void HideShop()
        {
            _shopPopup.gameObject.SetActive(false);
        }

        public void ShowShop()
        {
            _shopPopup.gameObject.SetActive(true);
        }
    }
}