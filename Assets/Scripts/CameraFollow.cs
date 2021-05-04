using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform target; //Variable del objetivo que seguira la camara, el cual se referencia en el editor
	public Vector3 offset = new Vector3(5.0f, 0.0f, -10f);//Distancia a la que la camara seguira el target
	public float dampingTime = 0.3f;//Tiempo que tardara la camara en seguir el target cuando se empiece a mover
	public Vector3 velocity = Vector3.zero; //Velocidad a la que ira la camara, inicia en 0

	void Awake()
	{
		Application.targetFrameRate = 60; //FPS a los que ira el juego
	}

	void Update()
	{
		MoveCamera(true); //La camara seguira al personaje suavemente mientras se juega
	}

	public void ResetCameraPosition() //Resetea la posicion de la camara
	{
		MoveCamera(false); //La camara se movera instantaneamente al inicio(porque sigue al player) cuando el player muera o se reinicie la partida
	}

	void MoveCamera(bool smooth) //Funcion para que la camara siga al personaje suavemente(true) o que se mueva instantaneamente(false)
	{		
		Vector3 destination = new Vector3(target.position.x - offset.x,	offset.y, offset.z); //Variable local de las coordenadas del target(variable) que seguira la camara

		if (smooth) //Si el parametro smooth es true...
		{
			this.transform.position = Vector3.SmoothDamp(this.transform.position, destination, ref velocity, dampingTime); //... seguira al target con un movimiento suavisado. SmoothDamp es para hacer un seguiemiento suavisado. Valores entre parentesis; 1. Posicion actual de la camara, 2. Coordenadas del target que seguira la camara, 3. Velocidad a la que ira la camara. Esta es una variable de paso por referencia, en resumen lo que hace es tomar prestada la variable mientras la usa y luego la devuelve a su lugar, o sea, velocity volvera a su valor original, 4. Tiempo que tardara la camara en seguir el target cuando se empiece a mover
		}
		else //Si es false...
		{
			this.transform.position = destination; //... la camara se movera instantaneamente al inicio(porque sigue al player) cuando el player muera o se reinicie la partida
		}
	}
}