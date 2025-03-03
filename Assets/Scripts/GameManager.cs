using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private GameSettings _gameSettings;

    [SerializeField] private Card _cardPrefab;

    [SerializeField] private Transform _container;

	[SerializeField] private TMP_Text _matchCountText;

	[SerializeField] private ImageLoader _imageLoader;

    private List<Sprite> _spritePairs;

    private List<Card> _cardList = new List<Card>();

	private Card _firstSelected, _secondSelected;

	private int _matchCount;

	private int _totalMatchCount;

    private void Start()
    {
        InitGame(true);
    }

	private void InitGame(bool isFirstStart)
	{
		if (isFirstStart)
		{
			LoadSprites();
		}
		else
		{
			ShuffleSprites(_spritePairs);
			AdjustCardListAfterShuffle();
			StartCoroutine(RememberingCards());
		}
	}

	private void LoadSprites()
	{
		_imageLoader.LoadImages(_gameSettings.cardFacesJsonUrl, PrepareSprites);
	}

    private void PrepareSprites(List<Sprite> sprites)
	{
		_spritePairs = new List<Sprite>(sprites.Count * 2);

		foreach (var face in sprites)
        {
            _spritePairs.Add(face);
            _spritePairs.Add(face);
        }

		ShuffleSprites(_spritePairs);
		CreateCards();
		StartCoroutine(RememberingCards());
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
		
		yield return new WaitForSeconds(_gameSettings.rememberingDelay);

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
		yield return new WaitForSeconds(_gameSettings.checkDelay);

		if (_firstSelected.OpenedIconSprite == _secondSelected.OpenedIconSprite)
		{
			_matchCountText.text = $"Score: {++_totalMatchCount}";
			if(++_matchCount == _spritePairs.Count / 2)
			{
				InitGame(false);
				_matchCount = 0;
			}
			else
			{
				DestroyCardsAfterMatch();
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
