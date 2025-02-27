using System;
using UnityEngine;
using UnityEngine.UI;
public class Card : MonoBehaviour
{
    public event Action<Card> OnClick;

    [SerializeField] private Image _openedIconImage;

    [SerializeField] private Image _hiddenIconImage;

    private bool _isSelected;

    public bool IsSelected => _isSelected;

    private Sprite _openedIconSprite;

    public Sprite OpenedIconSprite => _openedIconSprite;

    public void SetOpenedIconSprite(Sprite sprite)
    {
        _openedIconSprite = sprite;
        _openedIconImage.sprite = sprite;
    }

    public void Show()
    {
        _openedIconImage.gameObject.SetActive(true);
        _hiddenIconImage.gameObject.SetActive(false);
        _isSelected = true;
    }

    public void Hide()
    {
       _hiddenIconImage.gameObject.SetActive(true);
       _openedIconImage.gameObject.SetActive(false);
       _isSelected = false;
    }

    public void OnCardClick()
    {
        OnClick?.Invoke(this);
    }
}   
