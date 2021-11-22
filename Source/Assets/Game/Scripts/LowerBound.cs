using UnityEngine;

public class LowerBound : MonoBehaviour
{
	#region Function Events

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.gameObject.CompareTag(Ball.Tag)) return;
		GameManager.LoseLife();
	}

	#endregion
}