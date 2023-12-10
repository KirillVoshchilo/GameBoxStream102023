using SimpleComponents.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class CellPresenter : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _itemsQuantity;
    [SerializeField] private SCUIHighlighter _selectionHighlighter;
    [SerializeField] private SCUIHighlighter _interactableHighlighter;

    private Cell _cell;
    private bool _isInteractable;

    public SCUIHighlighter Highlighter => _selectionHighlighter;
    public Cell Cell { get => _cell; set => _cell = value; }
    public bool IsInteractable
    {
        get => _isInteractable;
        set
        {
            _isInteractable = value;
            if (value)
                _interactableHighlighter.TurnOffHighlight();
            else _interactableHighlighter.Highlight();
        }
    }

    public void SetSprite(Sprite value)
    {
        _icon.gameObject.SetActive(true);
        _icon.sprite = value;
    }
    public void SetCount(int count)
    {
        _itemsQuantity.gameObject.SetActive(true);
        _itemsQuantity.text = count.ToString();
    }
    public void Clear()
    {
        _cell = null;
        _icon.gameObject.SetActive(false);
        _itemsQuantity.gameObject.SetActive(false);
    }
}
