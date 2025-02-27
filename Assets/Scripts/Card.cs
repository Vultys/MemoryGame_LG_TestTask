using UnityEngine;
using UnityEngine.UI;
public class Card : MonoBehaviour
{
    [SerializeField] private Image _openedIconImage;

    public void SetOpenedIconSprite(Sprite sprite)
    {
        _openedIconImage.sprite = sprite;
    }
}   
