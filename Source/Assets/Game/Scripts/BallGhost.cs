using System.Collections.Generic;
using UnityEngine;

public class BallGhost : MonoBehaviour
{
	#region Public and Serialized Fields

	[SerializeField] private float ghostDelay;

	#endregion

	#region Private fields

	private readonly Queue<Vector3> _parentPositions = new Queue<Vector3>();
	private float _enableTime;
	private Transform _parentTransform;

	#endregion

	#region Function Events

	private void OnEnable()
	{
		_enableTime = Time.time;
		_parentTransform = transform.parent.transform;
	}

	private void FixedUpdate()
	{
		ChangePosToPrevParentPos();
	}

	#endregion

	#region Private Methods
/// <summary>
/// change the BallGhost position to the parents position <c>ghostDelay</c> seconds prior.
/// </summary>
	private void ChangePosToPrevParentPos()
	{
		_parentPositions.Enqueue(_parentTransform.position);
		if (Time.time > _enableTime + ghostDelay)
			transform.position = _parentPositions.Dequeue();
	}

	#endregion
}