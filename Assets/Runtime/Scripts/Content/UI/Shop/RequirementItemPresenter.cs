using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Content.UI.Shop
{
    public sealed class RequirementItemPresenter : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _itemCount;

        public Sprite Icon
        {
            get => _iconImage.sprite;
            set => _iconImage.sprite = value;
        }
        public int Count
        {
            get => Convert.ToInt32(_itemCount.text);
            set => _itemCount.text = value.ToString();
        }
    }
}