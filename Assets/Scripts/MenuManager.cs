using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
	public static MenuManager sharedInstance; //Variable para el singleton de esta clase
	public Canvas mainMenuCanvas; //Variable que se utilizara para acceder a los elementos del Canvas(como boones etc.) porque este script(MenuManager) se asigna al Game Manager, y despues el canvas del menu principal(Main Menu Canvas) se asigna a esta variable desde el editor
	public Canvas inGameCanvas;
	public Canvas gameOverCanvas;

	void Awake()
	{
		if(sharedInstance == null) //Creacion del singleton de esta clase
		{
			sharedInstance = this;
		}
	}

	//Mostrar el menu principal
	public void ShowMainMenu()
	{
		mainMenuCanvas.enabled = true;
	}

	//Ocultar el menu principal
	public void HideMainMenu()
	{
		mainMenuCanvas.enabled = false;
	}

	//Mostrar la UI del juego
	public void ShowGameUI()
	{
		inGameCanvas.enabled = true;
	}

	//Ocultar la UI del juego
	public void HideGameUI()
	{
		inGameCanvas.enabled = false;
	}

	//Mostrar el menu de game over
	public void ShowGameOverMenu()
	{
		gameOverCanvas.enabled = true;
	}

	//Ocultar el menu de game over
	public void HideGameOverMenu()
	{
		gameOverCanvas.enabled = false;
	}

	public void ExitGame() //Salir del juego
	{
		#if UNITY_EDITOR //Si esta en el editor de Unity...
			UnityEditor.EditorApplication.isPlaying = false; //... saldra del juego(es lo mismo que detener el juego desde el editor con el boton play)
		#else //Si no, si esta en cualquier otra plataforma...
			Application.Quit(); //... saldra del juego
		#endif
	}
}