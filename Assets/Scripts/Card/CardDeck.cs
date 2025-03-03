using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a deck of cards
/// </summary>
public class CardDeck
{
    private readonly GameSettings _gameSettings;

    private readonly Card _cardPrefab;

    private readonly Transform _container;

    private List<Card> _cards = new();
    
    private List<Sprite> _spritePairs;

    private Action _onDeckEnabled;

    private readonly ImageLoader _imageLoader;

    public IReadOnlyList<Card> Cards => _cards;

    /// <summary>
    /// Creates a deck of cards
    /// </summary>
    /// <param name="gameSettings"> Game settings </param>
    /// <param name="cardPrefab"> Card prefab </param>
    /// <param name="container"> Container to place cards </param>
    /// <param name="onDeckEnabled"> Callback after deck is enabled </param>
    public CardDeck(GameSettings gameSettings, Card cardPrefab, Transform container, Action onDeckEnabled)
    {
        _imageLoader = new ImageLoader();
        _gameSettings = gameSettings;
        _cardPrefab = cardPrefab;
        _container = container;
        _onDeckEnabled = onDeckEnabled;
    }

    /// <summary>
    /// Loads and creates cards
    /// </summary>
    public void LoadAndCreateCards()
    {
        _imageLoader.LoadImages(_gameSettings.cardFacesJsonUrl, OnLoadingComplete);
    }

    /// <summary>
    /// Reshuffles cards
    /// </summary>
    public void ReshuffleCards()
    {
        ShuffleSpritePairs();
        AdjustCardListAfterShuffle();
        _onDeckEnabled?.Invoke();
        CoroutineRunner.Instance.StartCoroutine(RememberingCards());
    }

    /// <summary>
    /// Prepares sprite pairs and creates cards
    /// </summary>
    /// <param name="sprites"> Sprites to form pairs </param>
    private void OnLoadingComplete(List<Sprite> sprites)
    {
        PrepareSpritePairs(sprites);
        ShuffleSpritePairs();
        CreateCards();
        _onDeckEnabled?.Invoke();
        CoroutineRunner.Instance.StartCoroutine(RememberingCards());
    }

    /// <summary>
    /// Prepares sprite pairs
    /// </summary>
    /// <param name="sprites"> Sprites to form pairs </param>
    private void PrepareSpritePairs(List<Sprite> sprites)
    {
        _spritePairs = new List<Sprite>();
        for (int i = 0; i < sprites.Count; i++)
        {
            if(i <= _gameSettings.maxMatchingCardsCount)
            {
                _spritePairs.Add(sprites[i]);
                _spritePairs.Add(sprites[i]);
            }
        }
    }

    /// <summary>
    /// Shuffles sprite pairs
    /// </summary>
    private void ShuffleSpritePairs()
    {
        for (int i = _spritePairs.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            
            Sprite temp = _spritePairs[i];
			_spritePairs[i] = _spritePairs[randomIndex];
			_spritePairs[randomIndex] = temp;
        }
    }

    /// <summary>
    /// Creates cards
    /// </summary>
    private void CreateCards()
    {
        _cards.Clear();
        foreach (var sprite in _spritePairs)
        {
            var card = UnityEngine.Object.Instantiate(_cardPrefab, _container);
            card.SetOpenedIconSprite(sprite);
            _cards.Add(card);
        }
    }

    /// <summary>
    /// Opens cards and hides them after remembering delay
    /// </summary>
    /// <returns></returns>
    private IEnumerator RememberingCards()
	{
		foreach (var card in Cards)
		{
			card.Show();
		}
		
		yield return new WaitForSeconds(_gameSettings.rememberingDelay);

		foreach (var card in Cards)
		{
			card.Hide();
		}
	}

    /// <summary>
    /// Adjusts card list after shuffling
    /// </summary>
    private void AdjustCardListAfterShuffle()
	{
		for (int i = 0; i < _cards.Count; i++)
		{
			_cards[i].SetOpenedIconSprite(_spritePairs[i]);
		}
	}
}
