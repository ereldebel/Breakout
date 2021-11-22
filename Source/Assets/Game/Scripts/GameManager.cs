using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
	#region Public and Serialized Fields

	[SerializeField] private GameObject heartPrefab;
	[SerializeField] private BlockGridRevealer gridRevealer;
	private static BlockGridRevealer GridRevealer => _shared.gridRevealer;
	[SerializeField] private Animator cameraAnimator;
	private static Animator CameraAnimator => _shared.cameraAnimator;
	[SerializeField] private TextMeshProUGUI TMP;
	[SerializeField] private KeyCode quitKey = KeyCode.Escape;

	#endregion

	#region Private Fields

	private static GameManager _shared;
	private const int StartingLives = 3;
	private const float FirstHeartXValue = -7.95f;
	private const float HeartsYValue = 4.4f;
	private const float DistanceBetweenHearts = 1.59f;
	private const string CameraShakeTrigger = "ball fell";
	private const string RemoveLifeTrigger = "life used";
	private const string WinMessage = "WIN!";
	private const string GameOverMessage = "GAME OVER";

	private int _blockCount;

	private static int BlockCount
	{
		get => _shared._blockCount;
		set => _shared._blockCount = value;
	}

	private int _remainingLives;

	private static int RemainingLives
	{
		get => _shared._remainingLives;
		set => _shared._remainingLives = value;
	}

	private List<GameObject> _livesList;

	private static List<GameObject> LivesList
	{
		get => _shared._livesList;
		set => _shared._livesList = value;
	}

	#endregion

	#region C# Events

	/// <summary>
	/// Prepare for new ball release.
	/// </summary>
	public static event Action ContinueGame;

	/// <summary>
	/// Game is over. No more lives.
	/// </summary>
	public static event Action EndGame;

	#endregion

	#region Function Events

	private void Awake()
	{
		_shared = this;
		RemainingLives = StartingLives;
	}

	private void Start()
	{
		InitializeLivesList();
	}

	private void Update()
	{
		if (Input.GetKeyDown(quitKey))
			Application.Quit();
	}

	#endregion

	#region Public Methods

	/// <summary>
	/// Removes a life from the list, and shakes camera. if there are no more lives, ends the game.
	/// otherwise, prepares for new ball release.
	/// </summary>
	public static void LoseLife()
	{
		CameraAnimator.SetTrigger(CameraShakeTrigger);
		--RemainingLives;
		Destroy(LivesList[RemainingLives], 1);
		LivesList[RemainingLives].GetComponent<Animator>().SetTrigger(RemoveLifeTrigger);
		LivesList.RemoveAt(RemainingLives);
		if (RemainingLives == 0)
		{
			EndGame?.Invoke();
			InitPlayAgain(GameOverMessage);
			return;
		}

		ContinueGame?.Invoke();
	}

	/// <summary>
	/// Registers a block to the grid.
	/// </summary>
	/// <param name="block"></param>
	public static void RegisterBlock(Block block)
	{
		++BlockCount;
		GridRevealer.RegisterBlock(block);
	}

	/// <summary>
	/// Marks the Block as removed. 
	/// </summary>
	public static void RemoveBlock()
	{
		++BlockCount;
		if (BlockCount > 0) return;
		EndGame?.Invoke();
		InitPlayAgain(WinMessage);
	}

	#endregion

	#region Private Methods

	/// <summary>
	/// Writes the game result and initiates a play again object searching for a Space bar press to start over the game.
	/// </summary>
	/// <param name="message"> The game result message. </param>
	private static void InitPlayAgain(string message)
	{
		_shared.gameObject.AddComponent<PlayAgain>();
		_shared.TMP.gameObject.SetActive(true);
		_shared.TMP.text = message;
	}
	
	/// <summary>
	/// Creates a new list of lives filled with instants of the Heart Prefab.
	/// </summary>
	private void InitializeLivesList()
	{
		LivesList = new List<GameObject>();
		for (int i = 0; i < RemainingLives; ++i)
		{
			LivesList.Add(Instantiate(
				heartPrefab,
				new Vector3(FirstHeartXValue + i * DistanceBetweenHearts, HeartsYValue, 0),
				Quaternion.identity
			));
		}
	}

	#endregion
}