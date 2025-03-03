using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Checking cards for matching
/// </summary>
public class CardMatcher
{
    private readonly float _checkDelay;
    private readonly Action _onMatch;
    private readonly Action _onGameRestart;
    private Card _firstSelected;
    private Card _secondSelected;
    private int _matchCount;
    private int _pairsCount;

    /// <summary>
    /// Creates a matcher
    /// </summary>
    /// <param name="checkDelay"> Delay between checking cards </param>
    /// <param name="onMatch"> Callback after match </param>
    /// <param name="onGameRestart"> Callback after game restart </param>
    public CardMatcher(float checkDelay, Action onMatch, Action onGameRestart)
    {
        _checkDelay = checkDelay;
        _onMatch = onMatch;
        _onGameRestart = onGameRestart;
    }

    /// <summary>
    /// Assigns OnClick event to cards
    /// </summary>
    /// <param name="cards"> Cards to assign event to </param>
    public void AssignOnClickEvent(IReadOnlyList<Card> cards)
    {
        foreach (var card in cards)
        {
            card.OnClick += SetSelectedCard;
        }

        _pairsCount = cards.Count / 2;
    }

    /// <summary>
    /// Sets selected card
    /// </summary>
    /// <param name="card"> Card to set </param>
    private void SetSelectedCard(Card card)
    {
        if (card.IsSelected) return;

        if (_firstSelected == null)
        {
            _firstSelected = card;
            card.Show();
        }
        else if (_secondSelected == null)
        {
            _secondSelected = card;
            card.Show();
            CoroutineRunner.Instance.StartCoroutine(CheckMatching());
        }
    }

    /// <summary>
    /// Checks matching
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckMatching()
    {
        yield return new WaitForSeconds(_checkDelay);

        if (_firstSelected.OpenedIconSprite == _secondSelected.OpenedIconSprite)
        {
            _onMatch.Invoke();
            _matchCount++;
            if (_matchCount == _pairsCount)
            {
                _onGameRestart.Invoke();
                _matchCount = 0;
            }
            else
            {
                DisableCardsAfterMatch();
            }
        }
        else
        {
            _firstSelected.Hide();
            _secondSelected.Hide();
        }

        ResetSelectedCards();
    }

    /// <summary>
    /// Resets selected cards
    /// </summary>
    private void ResetSelectedCards()
    {
        _firstSelected = null;
        _secondSelected = null;
    }

    /// <summary>
    /// Disables cards after match
    /// </summary>
    private void DisableCardsAfterMatch()
	{
		_firstSelected.OnClick -= SetSelectedCard;
		_secondSelected.OnClick -= SetSelectedCard;
		_firstSelected.Disable();
		_secondSelected.Disable();
	}
}

