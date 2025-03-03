using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMatcher
{
    private readonly float _checkDelay;
    private readonly Action _onMatch;
    private readonly Action _onGameRestart;
    private Card _firstSelected;
    private Card _secondSelected;
    private int _matchCount;
    private int _pairsCount;

    public CardMatcher(float checkDelay, Action onMatch, Action onGameRestart)
    {
        _checkDelay = checkDelay;
        _onMatch = onMatch;
        _onGameRestart = onGameRestart;
    }

    public void AssignOnClickEvent(IReadOnlyList<Card> cards)
    {
        foreach (var card in cards)
        {
            card.OnClick += SetSelectedCard;
        }

        _pairsCount = cards.Count / 2;
    }

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

    private void ResetSelectedCards()
    {
        _firstSelected = null;
        _secondSelected = null;
    }

    private void DisableCardsAfterMatch()
	{
		_firstSelected.OnClick -= SetSelectedCard;
		_secondSelected.OnClick -= SetSelectedCard;
		_firstSelected.Disable();
		_secondSelected.Disable();
	}
}

