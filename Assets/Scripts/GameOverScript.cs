using UnityEngine;

/// <summary>
/// Start or quit the game
/// </summary>
public class GameOverScript : MonoBehaviour
{

/* Pour ajouter un skin d'interface
 * private GUISkin skin;
	
	void Start()
	{
		// Load a skin for the buttons
		skin = Resources.Load("GUISkin") as GUISkin;
	}*/

	void OnGUI()
	{
		const int buttonWidth = 120;
		const int buttonHeight = 60;
		
		if (
			GUI.Button(
			// Center in X, 1/3 of the height in Y
			new Rect(
			Screen.width / 2 - (buttonWidth / 2),
			(1 * Screen.height / 3) - (buttonHeight / 2),
			buttonWidth,
			buttonHeight
			),
			"Retry!"
			)
			)
		{
			// Reload the level
			Application.LoadLevel(1);
		}
		
		if (
			GUI.Button(
			// Center in X, 2/3 of the height in Y
			new Rect(
			Screen.width / 2 - (buttonWidth / 2),
			(2 * Screen.height / 3) - (buttonHeight / 2),
			buttonWidth,
			buttonHeight
			),
			"Back to menu"
			)
			)
		{
			// Back to menu
			Application.LoadLevel(0);
		}
	}
}
