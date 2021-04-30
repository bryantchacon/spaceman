using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
	void Start()
	{
		
	}
	
	void Update()
	{
		
	}

	private void OnTriggerEnter2D(Collider2D collision) //Funcion que se activa al pasar por un trigger collider. La variable local Collider2D collision hace referencia al objeto que colisionara con el collider, o sea, el player, pero aun asi se comprueba checando su tag
	{
		if (collision.tag == "Player")
		{
			PlayerController controller = collision.GetComponent<PlayerController>(); //Recuperacion del PlayerController para poder acceder a sus metodos desde este script en esta funcion, para todo esto la variable(controller) debe tener como tipo de dato el mismo que va a recuperar, o sea, PlayerController
			controller.Die();
		}
	}
}