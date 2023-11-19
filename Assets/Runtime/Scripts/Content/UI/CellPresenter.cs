using Components.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CellPresenter : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _itemsQuantity;
    [SerializeField] private SCUIHighlighter _highlighter;

    private Cell _cell;

    public SCUIHighlighter Highlighter => _highlighter;
    public Cell Cell { get => _cell; set => _cell = value; }

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
