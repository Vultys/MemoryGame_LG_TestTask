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

	private int _matchCounts;

    private void Start()
    {
        InitGame(true);
    }

	private void InitGame(bool isFirstStart)
	{
		if (isFirstStart)
		{
			PrepareSprites();
			ShuffleSprites(_spritePairs);
			CreateCards();
		}
		else
		{
			ShuffleSprites(_spritePairs);
			AdjustCardListAfterShuffle();
		}

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
		}
	}

	private void AdjustCardListAfterShuffle()
	{
		for (int i = 0; i < _cardList.Count; i++)
		{
			_cardList[i].SetOpenedIconSprite(_spritePairs[i]);
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
			card.OnClick += SetSelectedCard;
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
			if(++_matchCounts == _spritePairs.Count / 2)
			{
				InitGame(false);
				_matchCounts = 0;
			}
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
