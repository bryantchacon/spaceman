using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Libreria para el texto de la interfaz de usuario

public class GameView : MonoBehaviour
{
	public Text coinsText, scoreText, maxScoreText; //Variables del canvas al jugar

	private PlayerController controller; //Variable controller para acceder a los metodos del PlayerController, y no estar preguntando por ellos todo el tiempo si el codigo que los pedira estara en el Update(), pero antes de esto, el componente que se guardara en la variable se pide en el Start()

	void Start(){
		controller = GameObject.Find("Player").GetComponent<PlayerController>(); //Se asigna que el valor de controller sera la obtencion del PlayerController del Player al encontrarlo, para esto, controller(la variable donde se guarda) es de tipo de dato PlayerController
	}
	
	void Update(){
		if(GameManager.sharedInstance.currentGameSate == GameState.inGame) //Si se esta jugando...
		{
			int coins = GameManager.sharedInstance.collectedObject; //Actualizara en la UI el numero de monedas que se vayan recolectando y su valor lo obtiene del Game Manager
			float score = controller.GetTravelDistance(); //Uso del metodo GetTravelDistance() del PlayerController por medio de la variable controller
			float maxScore = PlayerPrefs.GetFloat("maxscore", 0f); //Muestra en el In Game Canvas cual fue la puntuacion maxima anterior, pero si es la primer partida sera 0

			coinsText.text = coins.ToString(); //Al acceder al metodo .text de coinsText(CoinsText en el editor) se indicara que tenga el valor de coins y se convertira en string con .ToString()
			scoreText.text = "Score: " + score.ToString("f1"); //Cuando un float se convierte a string, el numero de decimales a mostrar se indica en el parentesis de .ToString(), entre comillas f sinifica que es float y 1 el numero de decimales a mostrar
			maxScoreText.text = "MaxScore: " + maxScore.ToString("f1");
		}
	}
}