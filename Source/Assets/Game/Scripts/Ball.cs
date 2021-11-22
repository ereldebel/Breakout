using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
	#region Public and Serialized Fields

	public const string Tag = "Ball";
	[SerializeField] private Rigidbody2D rigidbody2d;
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private float baseSpeed = 10; // norm of the initial speed vector
	[SerializeField] private GameObject firstBallGhost;
	[SerializeField] private KeyCode ballReleaseKey = KeyCode.Space;
	[SerializeField] private GameObject text;

	#endregion

	#region Private Fields

	private Vector3 _initialPosition;
	private Vector2 _currentVelocity;
	private bool _ballIsIdle = true;
	private int _paddleHits = 0;
	private float _speedBoost = 0;
	private SpriteRenderer[] _ghostRenderers;
	private const float MaxSpeed = 2.4f;

	private static readonly Dictionary<int, float> SpeedByNumberOfPaddleHits = new Dictionary<int, float>()
	{
		{4, 1.5f}, {8, 2f}, {12, MaxSpeed}
	};

	private const float MiddleBlockBoost = MaxSpeed;

	#endregion

	#region Function Events

	private void Awake()
	{
		InitFields();
		InitRandomDiagonalVelocity();
		GameManager.ContinueGame += ResetBall;
		GameManager.EndGame += DisableBall;
	}

	private void OnDestroy()
	{
		GameManager.ContinueGame -= ResetBall;
		GameManager.EndGame -= DisableBall;
	}

	private void Update()
	{
		CheckStartBallMovement();
		CheckBallStuck();
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (_ballIsIdle) return;
		PaddleCollision(other);
		BlockCollision(other);
		ChangeBallDirection(other);
	}

	#endregion

	#region Private Methods

	/// <summary>
	/// initializes the ghostRenderers and initialPosition private field.
	/// </summary>
	private void InitFields()
	{
		_ghostRenderers = firstBallGhost.gameObject.GetComponentsInChildren<SpriteRenderer>();
		_initialPosition = transform.position;
	}

	/// <summary>
	/// Sets the Ball's game object as disabled.
	/// </summary>
	private void DisableBall()
	{
		gameObject.SetActive(false);
	}

	/// <summary>
	/// Return the ball to initial position, stop ball movement and await player input if applicable. 
	/// </summary>
	private void ResetBall()
	{
		_ballIsIdle = true;
		_paddleHits = 0;
		_speedBoost = 0;
		foreach (var ghost in _ghostRenderers)
			ghost.enabled = false;
		rigidbody2d.bodyType = RigidbodyType2D.Static;
		transform.position = _initialPosition;
		InitRandomDiagonalVelocity();
	}

	/// <summary>
	/// Release the ball to start a match if user pressed the ball release key while the ball is idle.
	/// </summary>
	private void CheckStartBallMovement()
	{
		if (!_ballIsIdle || !Input.GetKeyUp(ballReleaseKey)) return;
		text.SetActive(false);
		_ballIsIdle = false;
		rigidbody2d.bodyType = RigidbodyType2D.Dynamic;
		rigidbody2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
		rigidbody2d.velocity = _currentVelocity;
	}

	/// <summary>
	/// Reset the ball if it stopped.
	/// </summary>
	private void CheckBallStuck()
	{
		if (rigidbody2d.velocity != Vector2.zero) return;
		ResetBall();
	}

	/// <summary>
	/// Increments the paddle hits counter and changes the ball's boost if needed. 
	/// </summary>
	/// <param name="other"> object Ball collided with. </param>
	private void PaddleCollision(Collision2D other)
	{
		if (!other.gameObject.CompareTag(Paddle.Tag)) return;
		++_paddleHits;
		if (SpeedByNumberOfPaddleHits.ContainsKey(_paddleHits))
			UpdateSpeedBoost(SpeedByNumberOfPaddleHits[_paddleHits]);
	}

	/// <summary>
	/// Disable the collided with block, change the ball's (and ghosts') color to the block's color and change the Ball
	/// boost if needed.
	/// </summary>
	/// <param name="other"> object Ball collided with. </param>
	private void BlockCollision(Collision2D other)
	{
		if (other.gameObject.CompareTag(Block.MiddleBlockTag))
			UpdateSpeedBoost(MiddleBlockBoost);
		else if (!other.gameObject.CompareTag(Block.BlockTag)) return;
		var color = other.gameObject.GetComponent<SpriteRenderer>().color;
		spriteRenderer.color = color;
		foreach (var ghostRenderer in _ghostRenderers)
		{
			color.a = ghostRenderer.color.a;
			ghostRenderer.color = color;
		}

		other.gameObject.SetActive(false);
	}

	/// <summary>
	/// If the new boost is bigger than the current, update the boost and update the velocity vector's norm to be the
	/// base speed times the new boost.
	/// </summary>
	/// <param name="newBoost"> new boost to check. </param>
	private void UpdateSpeedBoost(float newBoost)
	{
		if (newBoost <= _speedBoost) return;
		if (newBoost >= SpeedByNumberOfPaddleHits[8])
			foreach (var ghost in _ghostRenderers)
				ghost.enabled = true;
		_speedBoost = newBoost;
		_currentVelocity.Normalize();
		_currentVelocity.x *= baseSpeed * _speedBoost;
		_currentVelocity.y *= baseSpeed * _speedBoost;
	}

	/// <summary>
	/// Reflects the ball.
	/// </summary>
	/// <param name="other">The collided object.</param>
	private void ChangeBallDirection(Collision2D other)
	{
		var newDirection = other.contacts[0].normal;
		_currentVelocity = Vector2.Reflect(_currentVelocity, newDirection);
		rigidbody2d.velocity = _currentVelocity;
	}

	/// <summary>
	/// Initiates the velocity vector to a random diagonal vector with the given baseSpeed norm.
	/// </summary>
	private void InitRandomDiagonalVelocity()
	{
		float x = baseSpeed / 2 + Random.value;
		float y = Mathf.Sqrt(Mathf.Pow(baseSpeed, 2) - Mathf.Pow(x, 2));
		if (RandomBool()) x = -x;
		// norm([x,y]) = sqrt(x^2 + y^2) = Speed  <==>  y = sqrt(Speed^2-x^2)
		_currentVelocity = new Vector2(x, y);
	}

	/// <summary>
	/// returns a random boolean.
	/// </summary>
	/// <returns>random boolean. </returns>
	private static bool RandomBool() => Random.value > 0.5f;

	#endregion
}