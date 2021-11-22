using UnityEngine;

public class Block : MonoBehaviour
{
	#region Public and Serialized Fields

	public const string BlockTag = "Block";
	public const string MiddleBlockTag = "Middle Block";
	[SerializeField] private new SpriteRenderer renderer;

	#endregion

	#region Function Events

	private void Start()
	{
		GameManager.RegisterBlock(this);
	}

	private void OnDisable()
	{
		renderer.enabled = false;
		GameManager.RemoveBlock();
	}

	#endregion

	#region Public Methods

	/// <summary>
	/// Reveal the Block using procedural animation.
	/// </summary>
	public void Reveal()
	{
		gameObject.AddComponent<BlockRevealer>();
		renderer.enabled = true;
	}

	#endregion
}