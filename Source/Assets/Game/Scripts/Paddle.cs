using System;
using UnityEngine;

public class Paddle : MonoBehaviour
{
	#region Public and Serialized Fields

	public const string Tag = "Paddle";
	[SerializeField] private Rigidbody2D rigid;
	[SerializeField] private float speed;
	[SerializeField] private Animator animator;
	[SerializeField] private KeyCode leftKey = KeyCode.LeftArrow;
	[SerializeField] private KeyCode rightKey = KeyCode.RightArrow;

	#endregion

	#region Private Fields

	private const string PaddleHitTrigger = "Paddle Hit";
	private Vector2 _left = Vector2.left;
	private Vector2 _right = Vector2.right;
	private readonly Vector2 _stop = Vector2.zero;
	private Vector2 _currentMovement;
	private Vector3 _initialPosition;
	private bool _gameIdle = true;

	#endregion

	#region Function Events

	private void Awake()
	{
		InitializeFields();
		GameManager.ContinueGame += ResetPaddle;
		GameManager.EndGame += ResetPaddle;
	}

	private void OnDestroy()
	{
		GameManager.ContinueGame -= ResetPaddle;
		GameManager.EndGame -= ResetPaddle;
	}

	private void Update()
	{
		UpdateMovementByInput();
	}

	private void FixedUpdate()
	{
		if (_gameIdle) return;
		rigid.AddForce(_currentMovement);
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (!other.gameObject.CompareTag(Ball.Tag)) return;
		animator.SetTrigger(PaddleHitTrigger);
	}

	#endregion

	#region Private Methods

	/// <summary>
	/// Initialize the Paddle's private fields
	/// </summary>
	private void InitializeFields()
	{
		_initialPosition = transform.position;
		_left.x *= speed;
		_right.x *= speed;
		_currentMovement = _stop;
	}

	/// <summary>
	/// Get input from user and use change paddle movement vector accordingly.
	/// </summary>
	private void UpdateMovementByInput()
	{
		if (_gameIdle)
		{
			if (!Input.GetKey(KeyCode.Space)) return;
			_gameIdle = false;
			rigid.bodyType = RigidbodyType2D.Dynamic;
		}

		_currentMovement = _stop;
		if (Input.GetKey(leftKey))
			_currentMovement += _left;
		if (Input.GetKey(rightKey))
			_currentMovement += _right;
	}

	/// <summary>
	/// Places paddle at initial position and makes it static until ball release.
	/// </summary>
	private void ResetPaddle()
	{
		transform.position = _initialPosition;
		_gameIdle = true;
		rigid.bodyType = RigidbodyType2D.Static;
	}

	#endregion
}