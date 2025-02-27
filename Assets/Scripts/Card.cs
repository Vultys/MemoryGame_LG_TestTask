using UnityEngine;
using UnityEngine.UI;
public class Card : MonoBehaviour
{
    [SerializeField] private Image _openedIconImage;
    [SerializeField] private Image _hiddenIconImage;

    public void SetOpenedIconSprite(Sprite sprite)
    {
        _openedIconImage.sprite = sprite;
    }

    public void Show()
    {
        _openedIconImage.gameObject.SetActive(true);
        _hiddenIconImage.gameObject.SetActive(false);
    }

    public void Hide()
    {
       _hiddenIconImage.gameObject.SetActive(true);
       _openedIconImage.gameObject.SetActive(false);
    }
}   
