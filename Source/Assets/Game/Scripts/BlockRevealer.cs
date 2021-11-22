using UnityEngine;

public class BlockRevealer : MonoBehaviour
{
	#region Private Fields

	private Vector3 _scaleTarget;
	private float _relativeScale = 0;
	private const float RevealRate = 0.1f;

	#endregion

	#region Function Events

	private void OnEnable()
	{
		_scaleTarget = transform.localScale;
		transform.localScale = Vector3.zero;
	}

	private void FixedUpdate()
	{
		_relativeScale += RevealRate;
		ChangeBlockToRelativeScale();
	}

	#endregion

	#region Private Methods

	/// <summary>
	/// Changes the block's scale to be the relative scale. if the relative scale is 1 ends the revealing.
	/// </summary>
	private void ChangeBlockToRelativeScale()
	{
		if (_relativeScale < 1)
		{
			transform.localScale = _scaleTarget * _relativeScale;
			return;
		}

		transform.localScale = _scaleTarget;
		Destroy(this);
	}

	#endregion
}