using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Sprite[] _cardFaces;

    [SerializeField] private Card _cardPrefab;

    [SerializeField] private Transform _container;

	[SerializeField] private float _rememberingDelay = 5f;

    private List<Sprite> _spritePairs;

    private List<Card> _cardList = new List<Card>();

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
			card.Show();
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
}
