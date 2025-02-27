using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Sprite[] _cardFaces;

    [SerializeField] private Card _cardPrefab;

    [SerializeField] private Transform _container;

	[SerializeField] private float _rememberingDelay = 5f;

	[SerializeField] private float _checkDelay = 1f;

    private List<Sprite> _spritePairs;

    private List<Card> _cardList = new List<Card>();

	private Card _firstSelected, _secondSelected;

    private void Start()
    {
        PrepareSprites();
        CreateCards();
		StartCoroutine(RememberingCards());
    }

    private void PrepareSprites()
	{
		_spritePairs = new List<Sprite>(_cardFaces.Length * 2);

		foreach (var face in _cardFaces)
        {
            _spritePairs.Add(face);
            _spritePairs.Add(face);
        }

		ShuffleSprites(_spritePairs);
	}

    private void ShuffleSprites(List<Sprite> spritesList)
	{
		for (int i = spritesList.Count - 1; i > 0; i--)
		{
			int randomIndex = Random.Range(0, i + 1);

			Sprite temp = spritesList[i];
			spritesList[i] = spritesList[randomIndex];
			spritesList[randomIndex] = temp;
		}
	}

    private void CreateCards()
	{
		foreach (var sprite in _spritePairs)
        {
			Card card = Instantiate(_cardPrefab, _container);
			_cardList.Add(card);
			card.SetOpenedIconSprite(sprite);
			card.OnClick += SetSelectedCard;
		}
	}

	private IEnumerator RememberingCards()
	{
		foreach (var card in _cardList)
		{
			card.Show();
		}
		
		yield return new WaitForSeconds(_rememberingDelay);

		foreach (var card in _cardList)
		{
			card.Hide();
		}
	}

	private void SetSelectedCard(Card card)
	{
		if (card.IsSelected) return;

		if (_firstSelected == null)
		{
			card.Show();
			_firstSelected = card;
		}
		else if (_secondSelected == null)
		{
			card.Show();
			_secondSelected = card;
			StartCoroutine(CheckMatching());
		}
	}

	private IEnumerator CheckMatching()
	{
		yield return new WaitForSeconds(_checkDelay);

		if (_firstSelected.OpenedIconSprite == _secondSelected.OpenedIconSprite)
		{
			DestroyCardsAfterMatch();
		}
		else
		{
			_firstSelected.Hide();
			_secondSelected.Hide();
		}

		ResetSelectedCards();
	}

	private void ResetSelectedCards()
	{
		_firstSelected = null;
		_secondSelected = null;
	}

	private void DestroyCardsAfterMatch()
	{
		_firstSelected.OnClick -= SetSelectedCard;
		_secondSelected.OnClick -= SetSelectedCard;
		_firstSelected.Disable();
		_secondSelected.Disable();
	}
}
