using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitZone : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D collision) //Reafirmacion de que el game object al que esta adjunto este script es un trigger
	{
		if(collision.tag == "Player") //Si lo que colisione con el trigger tiene la etiqueta Player...
		{
			LevelManager.sharedInstance.RemoveLevelBlock(); //Borra el primer bloque de nivel de los que esten en escena actualmente
			LevelManager.sharedInstance.AddLevelBlock(); //Agrega un bloque de nivel al final del ultimo
		}
	}
}