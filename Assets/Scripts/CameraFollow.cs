using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform target; //Variable del objetivo que seguira la camara
	public Vector3 offset = new Vector3(5.0f, 0.0f, -10f);//Distancia a la que seguira el target
	public float dampingTime = 0.3f;//Tiempo que tardara la camara en seguir el target cuando se empiece a mover
	public Vector3 velocity = Vector3.zero; //Velocidad a la que ira la camara, inicia en 0

	void Awake()
	{
		Application.targetFrameRate = 60; //FPS a los que ira el juego
	}

	void Start()
	{
		
	}

	void Update()
	{
		MoveCamera(true); //La camara seguira al personaje suavemente
	}

	public void ResetCameraPosition() //Resetea la posicion de la camara
	{
		MoveCamera(false); //La camara se movera instantaneamente al inicio(porque sigue al player) cuando el player muera o se reinicie la partida
	}

	void MoveCamera(bool smooth) //Funcion para que la camara siga al personaje suavemente(true) o que se mueva instantaneamente(false)
	{
		//Coordenadas del target que seguira la camara
		Vector3 destination = new Vector3(
			target.position.x - offset.x,
			offset.y,
			offset.z);

		if(smooth) //Si el parametro smooth es true...
		{
			this.transform.position = Vector3.SmoothDamp( //Metodo para hacer un seguiemiento suavisado
				this.transform.position, //Posicion actual de la camara
				destination, //Coordenadas del target que seguira la camara
				ref velocity, //Velocidad a la que ira la camara(calculada por SmoothDamp()). Esta es una variable de paso por referencia, en resumen lo que hace es tomar prestada la variable mientras la usa y luego la devuelve a su lugar, o sea, velocity volvera a su valor original
				dampingTime); //Tiempo que tardara la camara en seguir el target cuando se empiece a mover
		}
		else //Si es false...
		{
			this.transform.position = destination; //La camara se movera instantaneamente al inicio(porque sigue al player) cuando el player muera o se reinicie la partida
		}
	}
}