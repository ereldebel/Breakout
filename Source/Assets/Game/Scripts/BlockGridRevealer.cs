using System.Collections.Generic;
using UnityEngine;

public class BlockGridRevealer : MonoBehaviour
{
	#region Public and Serialized Fields

	[SerializeField] private static float revealRate = 0.06f;
	[SerializeField] private static int blockRows = 5;
	[SerializeField] private static int blockCols = 11;

	#endregion

	#region Private Fields

	private int _distance = 0;
	private int _hiddenBlocks = blockCols * blockRows;
	private int _wavesRevealed = 0;

	private readonly SortedList<float, SortedList<float, Block>> _blocks =
		new SortedList<float, SortedList<float, Block>>();

	#endregion

	#region Function Events

	private void Update()
	{
		if (Time.time < _wavesRevealed * revealRate) return;
		RevealNextWave();
		++_wavesRevealed;
	}

	#endregion

	#region Public Methods

	public void RegisterBlock(Block block)
	{
		Vector2 blockPos = block.transform.position;
		if (!_blocks.ContainsKey(blockPos.x))
			_blocks.Add(blockPos.x, new SortedList<float, Block>());
		_blocks[blockPos.x].Add(-blockPos.y, block);
	}

	#endregion

	#region Private Methods

	/// <summary>
	/// Calls the Reveal method of all blocks with current distance (row + col) from the top left corner.
	/// </summary>
	private void RevealNextWave()
	{
		for (int row = 0, col = _distance; col >= 0; ++row, --col)
		{
			if (row >= blockRows || col >= blockCols) continue;
			_blocks.Values[col].Values[row].Reveal();
			--_hiddenBlocks;
		}

		++_distance;
		if (_hiddenBlocks == 0)
			this.enabled = false;
	}

	#endregion
}