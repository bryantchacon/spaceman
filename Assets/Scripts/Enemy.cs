using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public float runningSpeed = 1.5f; //Velocidad del enemigo
	Rigidbody2D rigidBody; //Variable para recuperar el rigidbody del enemigo, no es publica porque este script esta en el enemigo
	public bool facingRight = false; //Variable que indica la direccion en la que ve el enemigo al iniciar el juego, se activa o desactiva en el editor
	private Vector3 startPosition; //Variable donde se almacenara la posicion del enemigo
	public int enemyDamage = 10; //Daño del enemigo, cuando la variable se use se debe poner el signo de menos para que la cantidad se reste

	private void Awake()
	{
		rigidBody = GetComponent<Rigidbody2D>(); //Obtencion del rigidbody del enemigo
		//startPosition = this.transform.position; //Obtencion de la posicion del enemigo al iniciar el juego. NOTA: En realidad esta funcion NO ES NECESARIA
	}

	void Start ()
	{
		//this.transform.position = startPosition; //La posicion del enemigo sera la que se guardo en el Awake() al iniciar el juego, para que cada vez que el bloque de nivel se genere, el enemigo salga siempre en el mismo lugar y no en el ultimo en el que quedo cuando el bloque de nivel en el que estaba se destruyo. NOTA: En realidad esta funcion NO ES NECESARIA
	}

	//En FixedUpdate() se pone el codigo que tenga que ver con modificar algo graficamente en el juego, para que no caigan los frames
	void FixedUpdate ()
	{
		float currentRunningSpeed = runningSpeed; //Variable local del FixedUpdate() para usar el runningSpeed de la clase

		if (facingRight) //Si facingRight es true...
		{
			currentRunningSpeed = runningSpeed; //... se indica con la velocidad que la direccion en la que ira el enemigo es positiva(hacia la derecha)...
			this.transform.eulerAngles = new Vector3(0, 180, 0); //... y el enemigo girara 180° para mirar a la derecha
		}
		else //Si no, si facingRight es false...
		{
			currentRunningSpeed = -runningSpeed; //... la direccion en la que ira el enemigo es negativa(hacia la izquierda)...
			this.transform.eulerAngles = Vector3.zero; //... y el enemigo regresa a mirar a la izquierda
		}

		if (GameManager.sharedInstance.currentGameSate == GameState.inGame) //Si se esta jugando...
		{
			rigidBody.velocity = new Vector2 (currentRunningSpeed, rigidBody.velocity.y); //... el enemigo se movera, o sea, lo que hace esta funcion es aplicar correctamente la velocidad del enemigo solo cuando se este jugando, para que no se mueva en cualquiera de los otros estados del juego, indicar asi y es para que no cambie de posicion porque vale 0
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) //La funcion se llama cuando el enemigo colisiona con un collider tipo trigger, la variable collision en el parametro se refiere al game object contra el que chocara el enemigo
	{
		if (collision.tag == "Coin") //Si el game object contra el que choca tiene el tag coin...
		{
			return; //... no hara nada
		}
		else if (collision.tag == "Player") //Si la etiqueta es Player...
		{
			collision.gameObject.GetComponent<PlayerController>().CollectHealth(-enemyDamage); //... le restara lo del enemyDamage. Explicacion: Al chocar contra el Player se accede al PlayerController por medio de collision.. etc, se invoca la funcion de recoleccion de vida y el daño que causa el enemigo se pasa como parametro negativo para restarlo de la vida
			return;
		}
		facingRight = !facingRight; //Si no choca ni con una moneda(no hara nada) ni con el player(lo dañara), habra chocado con algo del escenario u otro enemigo y rotara hacia el otro lado porque este codigo invierte el valor que en ese momento tenga facingRight, sea cual sea
	}
}