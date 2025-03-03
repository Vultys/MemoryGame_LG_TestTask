using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents a card
/// </summary>
public class Card : MonoBehaviour
{    
    public event Action<Card> OnClick;

    [SerializeField] private Image _openedIconImage;

    [SerializeField] private Image _hiddenIconImage;

    [SerializeField] private Animator _animator;

    private bool _isSelected;

    public bool IsSelected => _isSelected;

    private Sprite _openedIconSprite;

    public Sprite OpenedIconSprite => _openedIconSprite;

    private readonly int _turnBackAnimationTrigger = Animator.StringToHash("turnBack");

    private readonly int _turnFrontAnimationTrigger = Animator.StringToHash("turnFront");

    private readonly int _hiddenAnimationTrigger = Animator.StringToHash("hidden");

    /// <summary>
    /// Sets sprite of the face of the card
    /// </summary>
    /// <param name="sprite"> Sprite to set </param>
    public void SetOpenedIconSprite(Sprite sprite)
    {
        _openedIconSprite = sprite;
        _openedIconImage.sprite = sprite;
    }

    /// <summary>
    /// Shows face of the card
    /// </summary>
    public void Show()
    {
        _animator.SetTrigger(_turnFrontAnimationTrigger);
        _isSelected = true;
    }

    /// <summary>
    /// Hides face of the card
    /// </summary>
    public void Hide()
    {
        _animator.SetTrigger(_turnBackAnimationTrigger);
       _isSelected = false;
    }

    /// <summary>
    /// Disables visual representation of the card
    /// </summary>
    public void Disable()
    {
        _animator.SetTrigger(_hiddenAnimationTrigger);       
        _isSelected = false;
    }

    /// <summary>
    /// On click event
    /// Invokes by Unity
    /// </summary>
    public void OnCardClick()
    {
        OnClick?.Invoke(this);
    }
}   
