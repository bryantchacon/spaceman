using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectableType{
	healthPotion,
	manaPotion,
	coin
}

public class Collectable : MonoBehaviour{
	public CollectableType type = CollectableType.coin; //Valor por defecto de la variable

	private SpriteRenderer sprite; //Referencia al sprite del collectable al que se asigne este script
	private CircleCollider2D itemCollider; //Referencia al collider del collectable al que se asigne este script

	bool hasBeenCollected = false;

	public int value = 1;

	GameObject player; //Variable para usarse en la funcion Collect() en la recoleccion de vida y mana, pero primero, se localiza el Player en el Start()

	private void Awake(){
		sprite = GetComponent<SpriteRenderer>();
		itemCollider = GetComponent<CircleCollider2D>();
	}
	
	private void Start(){
		player = GameObject.Find("Player"); //Se obtiene el player por medio de su etiqueta, esto cuando un game object no es un singleton, o sea, un shared instance
	}

	void Show(){
		sprite.enabled = true;
		itemCollider.enabled = true;
		hasBeenCollected = false;
	}

	void Hide(){
		sprite.enabled = false;
		itemCollider.enabled = false;
	}

	void Collect(){ //Funcion que indicara que hacer al recolectar un item, dependiendo del tipo que sea, esto dependera del tipo de item al que se agregue ESTE SCRIPT ya que se selecciona en la variable Collectable Type desde el editor
		Hide();
		hasBeenCollected = true;

		switch (this.type){ //Dependiendo del tipo de item al que se agregue este script, se ejecutara un caso u otro del switch
			case CollectableType.coin:
				GameManager.sharedInstance.CollectObject(this); //Aumenta la cantidad de monedas recolectadas al acceder al GameManager, usa la funcion CollectObject() y pasa por parametro este mismo script que esta agregado a la moneda en el editor
				GetComponent<AudioSource>().Play(); //Al recolectar la moneda reproduce el sonido de recolectar moneda por medio del componente AudioSource
				break;
			case CollectableType.healthPotion:
				player.GetComponent<PlayerController>().CollectHealth(this.value); //Del player se obtiene su script y se llama la funcion CollectHealth() con value(variable de aqui por eso el this., y su valor sera el que se le haya asignado en el editor. Aparte del tipo de collectable que es) como parametro
				break;
			case CollectableType.manaPotion:
				player.GetComponent<PlayerController>().CollectMana(this.value);
				break;
		}
	}

	void OnTriggerEnter2D(Collider2D collision){
		if (collision.tag == "Player"){
			Collect(); //Ejecucion de la funcion Collect()

			//Destroy(gameObject);
		}
	}
}