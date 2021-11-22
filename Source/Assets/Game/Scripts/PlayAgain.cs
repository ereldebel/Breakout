using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAgain : MonoBehaviour
{
	[SerializeField] private KeyCode playAgainKey = KeyCode.Space;

	private void Update()
	{
		if (Input.GetKey(playAgainKey))
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}