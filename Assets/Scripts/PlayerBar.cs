using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BarType{
	healthBar,
	manaBar
}

public class PlayerBar : MonoBehaviour
{
	private Slider slider; //Referencia al slider(bar)
	public BarType type; //Variable para seleccionar el tipo de bar desde el editor

	void Start ()
	{
		slider = GetComponent<Slider>(); //Obtiene el componente Slider de la variable slider

		//Dependiendo del tipo de barra que se elija en el editor...
		switch (type)
		{
			case BarType.healthBar:
				slider.value = PlayerController.INITIAL_HEALTH; //... al iniciar el juego el valor de la barra se asigna por medio de la constante INITIAL_HEALTH del PlayerController. .value es una funcion de Slider, el tipo de dato de slider
				break;
			case BarType.manaBar:
				slider.value = PlayerController.INITIAL_MANA;
				break;
		}
	}

	void Update ()
	{
		switch (type)
		{
			case BarType.healthBar:
				slider.value = GameObject.Find("Player").GetComponent<PlayerController>().GetHealth();
				break;
			case BarType.manaBar:
				slider.value = GameObject.Find("Player").GetComponent<PlayerController>().GetMana();
				break;
		}
	}
}