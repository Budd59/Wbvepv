using UnityEngine;

/// <summary>
/// Title screen script
/// </summary>
public class MenuScript : MonoBehaviour
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
		const int buttonwidth = 84;
		const int buttonheight = 60;
		
		// Draw a button to start the game
		if (
			GUI.Button(
			// Center in X, 2/3 of the height in Y
			new Rect(
			Screen.width / 2 - (buttonwidth / 2),
			(2 * Screen.height / 3) - (buttonheight / 2),
			buttonwidth,
			buttonheight
			),
			"Start!"
			)
			)
		{
			// On Click, load the first level.
			// "Stage1" is the name of the first scene we created.
			Application.LoadLevel(1);
		}
	}
}
