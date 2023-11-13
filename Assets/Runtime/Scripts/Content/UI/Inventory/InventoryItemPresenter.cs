using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Content.UI.Inventory
{
    public sealed class InventoryItemPresenter : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _itemCount;

        public Sprite Icon
        {
            get => _iconImage.sprite;
            set => _iconImage.sprite = value;
        }
        public string Name
        {
            get => _itemName.text;
            set => _itemName.text = value;
        }
        public int Count
        {
            get => Convert.ToInt32(_itemCount.text);
            set => _itemCount.text = value.ToString();
        }
    }
}