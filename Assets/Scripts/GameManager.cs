using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState //Clase tipo enum con los estados del juego, se usa como tipo de variable para indicar a la variable que lo use cual de los datos del enum sera su valor. Se escribe fuera de la clase y despues de los using para que se pueda acceder a los GameStates desde otros cripts
{
	menu,
	inGame,
	gameOver
}

public class GameManager : MonoBehaviour
{
	public GameState currentGameSate = GameState.menu; //Uso de la clase enum GameState en una variable
	public static GameManager sharedInstance; //Instancia compartida; variable singleton(por static) que servira para crear una sola instancia del Game Manager. Otras maneras de usarlo son para el player controller o inventario o menu, si hubiera un solo jugador
	private PlayerController controller; //Variable que servira para poder usar los metodos del PlayerController en este script
	public int collectedObject = 0; //Valor por default al inicio de la partida

	void Awake() //Se ejecuta antes de iniciar el juego, frame 0, o sea, se encarga de "despertar" primero todos los elementos del Game Manager, por lo general aqui se declaran las variables privadas
	{
		if(sharedInstance == null) //Si sharedInstance aun no tiene ningun valor(porque aun no se crea la instancia del Game Manager) se creara
		{
			sharedInstance = this; //Creacion de la unica instancia del Game Manager
		}
	}

	void Start() //Se ejecuta al inicar el juego; 1er frame
	{
		controller = GameObject.Find("Player").GetComponent<PlayerController>(); //Indica que al iniciar el juego(por estar en Start()) la variable controller guardara el PlayerController del game object Player, al buscarlo por su nombre, y asi recuperarlo de el, o sea, el PlayerController del player se guardara en la variable controller
	}

	void Update() //Se ejecuta cada frame, si el codigo de aqui es muy pesado los frames caeran
	{
		/*
		if(Input.GetKeyDown(KeyCode.S)) //Ejecutara el codigo si detecta la que la techa S fue presionada
		{
			StartGame();
		}
		*/

		if(Input.GetButtonDown("Submit") && currentGameSate != GameState.inGame) //Iniciar el juego al presionar enter(que es la tecla asignada en el submit de input manager) y el estado del juego es diferente de(!=) inGame. Hay dos Submit en el Input manager, para evitar errores los dos deben tener diferente Positive Button y ningun Alt Positive Button
		{
			StartGame();
		}
	}

	public void StartGame() //Iniciar juego
	{
		SetGameState(GameState.inGame); //Asigna el estado del juego a inGame
	}

	public void GameOver() //Finalizar juego cuando el player muere
	{
		SetGameState(GameState.gameOver); //Asigna el estado del juego a gameOver
	}

	public void BackToMenu() //Regresar al menu
	{
		SetGameState(GameState.menu); //Asigna el estado del juego a menu
	}

	private void SetGameState(GameState newGameState) //newGameState es una variable local de esta funcion, es su parametro, las variables que se vayan a usar en una funcion se indican entre sus () con todo y su tipo de dato
	{
		if(newGameState == GameState.menu)
		{
			MenuManager.sharedInstance.ShowMainMenu(); //Mostrar el menu principal
		}
		else if(newGameState == GameState.inGame) //Si el estado del juego es inGame...
		{
			LevelManager.sharedInstance.RemoveAllLevelBlocks();
			LevelManager.sharedInstance.GenerateInitialLevelBlocks();
			controller.StartGame(); //Accede al StartGame del PlayerController por medio de la variable controller porque esta lo guarda, indicado en el Start() de aqui

			MainOrGameOverMenu();
			MenuManager.sharedInstance.ShowGameUI(); //Mostrar la UI del juego
		}
		else if(newGameState == GameState.gameOver)
		{
			MenuManager.sharedInstance.HideGameUI(); //Ocultalar la UI del juego
			MenuManager.sharedInstance.ShowGameOverMenu(); //Mostrar el menu de game over
		}

		this.currentGameSate = newGameState; //Actualiza el estado actual del juego(currentGameSate, por eso se usa this., para enfatizar que es una variable del mismo Game Manager) al nuevo estado del juego(newGameState) para que continue
	}

	void MainOrGameOverMenu()
	{
		if(MenuManager.sharedInstance.mainMenuCanvas.enabled)
		{
			MenuManager.sharedInstance.HideMainMenu(); //Ocultar el menu principal
		}
		else if(MenuManager.sharedInstance.gameOverCanvas.enabled)
		{
			MenuManager.sharedInstance.HideGameOverMenu(); //Ocultar el menu de game over
		}
	}

	public void CollectObject(Collectable collectable){
		collectedObject += collectable.value; //Aumentar el valor del objeto recolectado en la cantidad del valor de value que es una variable de Collectable(El script. El valor de value se asigna en el editor porque es una variable publica). Luego, se tiene que actualizar la UI, esto se indica en el Update() del script GameView
	}
}