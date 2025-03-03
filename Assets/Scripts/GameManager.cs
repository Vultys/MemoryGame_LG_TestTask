using TMPro;
using UnityEngine;

/// <summary>
/// Game manager
/// </summary>
public class GameManager : MonoBehaviour
{
	[SerializeField] private GameSettings _gameSettings;
    [SerializeField] private Card _cardPrefab;
    [SerializeField] private Transform _container;
    [SerializeField] private TMP_Text _matchCountText;
	[SerializeField] private GameObject _loadingScreen;

    private CardMatcher _cardMatcher;
    private CardDeck _cardDeck;
    private int _matchCount;

	/// <summary>
	/// Start is called before the first frame update
	/// </summary>
    private void Start()
    {
        _cardDeck = new CardDeck(_gameSettings, _cardPrefab, _container, AssignOnClickEvent);
        _cardMatcher = new CardMatcher(_gameSettings.checkDelay, OnMatch, RestartGame);
        _cardDeck.LoadAndCreateCards();
    }

	/// <summary>
	/// Assign OnClick event to cards after loading
	/// </summary>
    private void AssignOnClickEvent()
    {
		_loadingScreen.SetActive(false);
		_cardMatcher.AssignOnClickEvent(_cardDeck.Cards);
    }

	/// <summary>
	/// Update match count text
	/// </summary>
    private void OnMatch()
    {
        _matchCountText.text = $"Score: {++_matchCount}";
    }

	/// <summary>
	/// Restarts the game
	/// </summary>
    private void RestartGame()
    {
		_cardDeck.ReshuffleCards();
    }
}