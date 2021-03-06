using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	public static LevelManager sharedInstance; //Singleton para generar una unica instancia del Level Manager en el Awake()

	public List<LevelBlock> allTheLevelBlocks = new List<LevelBlock>(); //Declaracion(reservacion en memoria) de una lista(por List<>), de objetos Level Block(por que esta entre los <>), que seran todos los bloques de nivel que formen parte de el, los cuales se indican en el editor
	public List<LevelBlock> currentLevelBlocks = new List<LevelBlock>(); //Del mismo tipo que allTheLevelBlocks, pero esta lista solo tendra los bloques de nivel que esten actualmente en la escena
	
	public Transform levelStartPosition; //Posicion inicial del primer nivel, al igual que los puntos de inicio y fin, es de tipo Transform POR SER UN GAME OBJECT. Se pasa como parametro en el editor

	void Awake()
	{
		if (sharedInstance == null) //Generacion de la unica instancia del Leven Manager
		{
			sharedInstance = this;
		}
	}

	void Start()
	{
		GenerateInitialLevelBlocks();
	}
	
	public void AddLevelBlock() //Agregar un nuevo bloque de nivel a la lista de bloques actuales(currentLevelBlocks) desde todos los bloques disponibles(allTheLevelBlocks)
	{
		int randomIdx = Random.Range(0, allTheLevelBlocks.Count); //Varibale generadora de numeros leatorios usando la clase Random de Unity y su funcion Range(rango), sus parametros indican que el numero aleatorio sera entre 0(primer posicion) y la cantidad total de elementos que tenga la lista allTheLevelBlocks(por .Count)
		LevelBlock block; //Variable block de tipo Level Block donde se guardara CADA nuevo bloque de nivel que se valla creando
		Vector3 spawnPosition = Vector3.zero; //Variable de las coordenadas donde se anclara el Start Point del nuevo bloque. Empieza inicializado en 0, 0, 0

		if(currentLevelBlocks.Count == 0) //Si aun no hay bloques creados, o sea, al inicar el juego la cantidad de bloques actuales(currentLevelBlocks) es igual a 0, el primer bloque de nivel...
		{
			block = Instantiate(allTheLevelBlocks[0]); //... sera el que este en la posicion 0 de allTheLevelBlocks(se instancia con Instantiate)...
			spawnPosition = levelStartPosition.position; //... y se indica que spawnPosition(de tipo Vector3, o sea, coordenadas) sera igual a las coordenadas de levelStartPosition(por .position, porque levelStartPosition es de tipo Transform POR SER UN GAME OBJECT, o sea, el punto anclaje en el que inician los bloques de nivel inicial. Se pasa como parametro en el editor en la variable levelStartPosition de este script
		}
		else //Si no es el primer bloque de nivel...
		{
			block = Instantiate(allTheLevelBlocks[randomIdx]); //... instanciara uno random de allTheLevelBlocks...
			spawnPosition = currentLevelBlocks[currentLevelBlocks.Count - 1].exitPoint.position; //... e indicara que el punto de anclaje para el bloque de nivel nuevo, sera la posicion del exit point(.exitPoint.position) del ultimo bloque de nivel de los bloques de nivel actuales que se pusieron(currentLevelBlocks[currentLevelBlocks.Count - 1]), - 1 se refiere al ultimo porque la numeracion empieza en 0, no en 1
		}

		block.transform.SetParent(this.transform, false); //Indica que todos los bloques que se generen seran hijos de este script el Level Manager y que todas las transformaciones que puedan tener del parent, no las tengan los bloques

		Vector3 correction = new Vector3(spawnPosition.x - block.startPoint.position.x, spawnPosition.y - block.startPoint.position.y, 0); //Guarda las que seran las coordenadas del start point del nuevo bloque de nivel al que se asigne esta variable, seran las del spawnPosition del ultimo bloque de nivel, o sea, su exit point

		block.transform.position = correction; //Asignacion de la variable correction como nueva posicion del startPoint del nuevo bloque de nivel por medio de transform.position, esto porque el tipo de dato de block, o sea, Level Block, esta asignado a un game object(un level block)
		currentLevelBlocks.Add(block); //Añade el nuevo bloque de nivel(block) a la lista de currentLevelBlocks

		//NOTA: En codigo, cuando se va a hacer referencia a la posicion de un game object se indica con tipo de dato Transform y de uno con un script en el, es por medio de transform.position; de este ultimo el tipo de dato de la variable que lo contiene es el nombre del script
	}

	public void RemoveLevelBlock() //Remover bloque de nivel
	{
		LevelBlock oldBlock = currentLevelBlocks[0]; //Variable que se refiere al bloque de nivel de los actuales, en la posicion 0. La varianle es de tipo Level Block porque se refiere a un objeto de la lista currentLevelBlocks la cual es de ese tipo
		currentLevelBlocks.Remove(oldBlock); //Se usa el metodo Remove() de currentLevelBlocks(por ser una List<>), para eliminar oldBlock de la lista
		Destroy(oldBlock.gameObject); //Y con Destroy() se elimina graficamente del juego
	}

	public void RemoveAllLevelBlocks() //Remover todos los bloques de nivel cuando muera el personaje o se reinicie la partida
	{
		while(currentLevelBlocks.Count > 0) //Mientras hayan bloques de nivel en currentLevelBlocks...
		{
			RemoveLevelBlock(); //... se ejecutara RemoveLevelBlock() hasta que no quede ninguno
		}
	}

	public void GenerateInitialLevelBlocks() //Generar los bloques de nivel iniciales al empezar el juego
	{
		for(int i = 0; i < 2; i++) //Indica que al iniciar el juego van a haber 2 bloques de nivel
		{
			AddLevelBlock();
		}
	}
}