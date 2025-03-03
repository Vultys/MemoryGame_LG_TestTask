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

    private CardMatcher _cardMatcher;
    private CardDeck _cardDeck;
    private int _matchCount;

    private void Start()
    {
        _cardDeck = new CardDeck(_gameSettings, _cardPrefab, _container, AssignOnClickEvent);
        _cardMatcher = new CardMatcher(_gameSettings.checkDelay, OnMatch, RestartGame);
        _cardDeck.LoadAndCreateCards();
    }

    private void AssignOnClickEvent()
    {
		_cardMatcher.AssignOnClickEvent(_cardDeck.Cards);
    }

    private void OnMatch()
    {
        _matchCountText.text = $"Score: {++_matchCount}";
    }

    private void RestartGame()
    {
		_cardDeck.ReshuffleCards();
    }
}