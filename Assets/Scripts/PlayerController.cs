using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour //: indica que hereda de la clase MonoBehaviour
{
    //Todas las variables publicas se pueden modificar en Unity
    
    //VARIABLES DEL MOVIMIENTO DEL PERSONAJE
    public float jumpForce = 6f;
    public float walkSpeed = 2f; //VELOCIDAD DEL PLAYER, 2 metros por segundo

    Rigidbody2D rigidBody; //COMPONENTE Rigidbody2D DEL PLAYER, este componente es el que se rige por el motor de fisicas de Unity. Si no se escribe el nivel de visibilidad del codigo, lo interpretara como private, asi son Awake, Start y Update
    Animator animator; //COMPONENTE Animator DEL PLAYER
    Vector3 startPosition; //Guardara la posicion iniciar del personaje al juego, para que al reiniciar siempre empiece en el mismo lugar. Es Vector3 porque aunque sea un juego 2D tambien tiene profundidad en Z, y para esto tambien se tiene que indicar

    const string STATE_ALIVE = "isAlive"; //CONSTANTE QUE HACE REFERENCIA AL PARAMETRO BOOL DEL Animator. Las variables constantes (const), nunca cambiaran en toda la ejecucion del codigo
    const string STATE_ON_THE_GROUND = "isOnTheGround";

    //VARIABLES DE VIDA Y MANA
    [SerializeField] //Es para poder ver variable privadas en el editor
    private int healthPoints, manaPoints; //Variables privadas de vida y mana

    public const int INITIAL_HEALTH = 100, INITIAL_MANA = 15, MAX_HEALTH = 200, MAX_MANA = 30, MIN_HEALTH = 10, MIN_MANA = 0; //Constantes de vida iniciales, maximas y minimas

    //VARIABLES DEL SUPER SALTO
    public const int SUPER_JUMP_COST = 5;
    public const float SUPER_JUMP_FORCE = 1.5F;

    public LayerMask groundMask; //El tipo de variable LayerMask indica qué es palpable a nivel de suelo, con esto se sabra con que objetos se puede chocar
    public float raycastLongitude = 2.5f; //Longitud del raycast

    //1st
    void Awake() //Se ejecuta antes de iniciar el juego, frame 0, o sea, se encarga de "despertar" primero todos los elementos del player, por lo general aqui se declaran las variables privadas
    {
        rigidBody = GetComponent<Rigidbody2D>(); //Recuperacion del componente Rigidbody2D del player porque la variable que lo contiene (rigidBody) es private
        animator = GetComponent<Animator>(); //Recuperacion del componente Animator
    }

    //2nd
	void Start() //Se ejecuta al iniciar el juego; 1er frame, ya que antes Awake "desperto" todos los elementos
    {
        startPosition = this.transform.position; //Guarda la poscion del player al inicar el juego
	}

    public void StartGame() //Es publico para que se pueda acceder al el desde el Game Manager
    {
        //Estos dos SetBool(de STATE_ALIVE y STATE_ON_THE_GROUND) estan aqui por que si estuviesen en el Start de Unity provocarian un bug
		animator.SetBool(STATE_ALIVE, true); //Al iniciar el juego setear el animator el booleano STATE_ALIVE a true
		animator.SetBool(STATE_ON_THE_GROUND, false); //Este inicia en false porque el personaje aparece en el aire, al tocar el piso el codigo lo pasara a true

        healthPoints = INITIAL_HEALTH; //Iniciar el juego con determinada vida
        manaPoints = INITIAL_MANA;//Iniciar el juego con determinado mana

        Invoke("RestartPosition", 0.15f); //Invoke retrasa la invocacion de una funcion, esta se llama como string y se asigna el tiempo en el que se ejecutara despues de llamarla
    }

    void RestartPosition() //Funcion que sera llamada con retraso por medio de Invoke() para que la animacion de muerte no se vea
    {
        this.transform.position = startPosition; //Indica que al reiniciar el juego la posicion del player es la que se habia guardado anteriormente en Start() al iniciar la partida
        this.rigidBody.velocity = Vector2.zero; //Frena la caida del player para que al regresarlo a su posicion original no atraviese la plataforma

        GameObject mainCamera = GameObject.Find("Main Camera"); //Busca y almacena la Main Camera del juego para despues...
        mainCamera.GetComponent<CameraFollow>().ResetCameraPosition(); //... ontener su script Camera Follow y ejecutar la funcion ResetCameraPosition()
    }

    //3rd
	void Update() //Se ejecuta cada frame, o sea, cada frame checa si las funciones o variables cambian. Si el codigo de aqui es muy pesado los frames caeran
    {
        //FUNCION PARA ACTIVAR EL SALTO

        /*
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) //Ejecuta el codigo si cualquiera de las dos sentencias es verdadera, o sea, si se presiona espacio o clic izquierdo, en ambos Down indica que detectara la accion cuando la barra o boton se presionen, no al dejarlo de presionar(Up), || es o
        {
        }
        */

        if (Input.GetButtonDown("Jump")){ //Usa el input manager de Unity y ejecutara el codigo al detectar
            Jump(false); //false indica que usara el salto normal
        }
        if (Input.GetButtonDown("SuperJump")){
            Jump(true); //true indica que usara el super salto
        }

		animator.SetBool(STATE_ON_THE_GROUND, IsTouchigTheGround()); //Con esto indica que animacion hacer dependiendo de si esta en el suelo o no porque todo el tiempo esta checandolo ya que usa la funcion IsTouchigTheGround() como parametro



        Debug.DrawRay(this.transform.position, Vector2.down * raycastLongitude, Color.red); //Hacer visible el Raycast en color rojo, se borra antes de exportar el juego
	}

    //4th
    void FixedUpdate() //Update a ratio fijo, es un reloj, va a intervalos fijos y nunca se retrasa o adelanta y es llamado menos veces que el Update(), por esto, el codigo de los gameobjects que vaya aqui hara que sean mas fluidos, para evitar que una caida de frames los afecte fisicamente
    {
        //Walk();

        if(GameManager.sharedInstance.currentGameSate == GameState.inGame) //Si el estado del juego es inGame se ejecutara el codigo, lo sera cuando se presione enter porque el juego inicia en Menu
        {
            if(rigidBody.velocity.x < walkSpeed) //FUNCION PARA CORRER AUTOMATICAMENTE. Si la velocidad en x es menor al valor de la variable walkSpeed, ejecutara el codigo
            {
                rigidBody.velocity = new Vector2 //Velocidad del player (por rigidBody) es igual a nuevo Vector2 que sera el nuevo vector de velocidad, o sea, se crea el vector velocidad 2D
                (walkSpeed, //Velocidad en x
                rigidBody.velocity.y); //Velocidad en y
            }
        }
        else //Si no...
        {
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y); //... se detendra el movimiento del personaje. Al iniciar el juego el player cae al piso, por eso 0 es para que no avance y para y normal porque su velocidad es por la fisica
        }
    }

    //Jump() esta en Update() para aligerar su carga, ya que ejecutara Jump() solo cuando la condicion del if() que la contiene se cumpla, y Walk() esta en FixedUpdate() para que su movimiento sea mas fluido, o sea, por la misma naturaleza de FixedUpdate()

    //CAMINAR
    void Walk()
    {
        if (IsTouchigTheGround())
        {
            rigidBody.velocity = new Vector2(Input.GetAxis("Horizontal") * walkSpeed, rigidBody.velocity.y);

            if (Input.GetAxis("Horizontal") < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }

            if (Input.GetAxis("Horizontal") > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }

    //SALTAR
    void Jump(bool superJump){
        float jumpForceFactor = jumpForce; //Variable local debido al super salto, pero aun asi sustituye a jumpForce en el salto normal porque la variable es local

        //Si se cumple este if es solo para restar mana y aumentar el valor del jumpForceFactor y el siguiente if es el que realiza el salto
        if (superJump && manaPoints >= SUPER_JUMP_COST && IsTouchigTheGround()){ //Si superJump es true y los puntos de mana son mas o igual al costo del super salto y esta tocando el suelo...
            manaPoints -= SUPER_JUMP_COST; //... se resta el valor del super salto de los puntos de mana y...
            jumpForceFactor *= SUPER_JUMP_FORCE; //... el factor de fuerza de salto sera ahora el resultado de multiplicarse a si mismo por la fuerza del super salto
        }
        if (GameManager.sharedInstance.currentGameSate == GameState.inGame){ //Este if es el que realiza el salto en si
            if(IsTouchigTheGround()){
				GetComponent<AudioSource>().Play(); //Al saltar reproduce el sonido de salto por medio del componente AudioSource
                rigidBody.AddForce(Vector2.up * jumpForceFactor, ForceMode2D.Impulse); //Aplicar fuerza hacia arriba multiplicandolo por la fuerza de salto y el modo de la fuerza sera impulso; Impulse es que la fuerza sera repentina para que el personaje salte y caiga
            }
        }
    }

    bool IsTouchigTheGround() //FUNCION PARA DETECTAR SI EL PLAYER ESTA O NO EN EL SUELO. Traza un rayo invisible para detectar el suelo, si lo detecta es verdadero y regresara true, si no false
    {
        if(Physics2D.Raycast //Si al trazar un rayo con el motor de fisicas 2D...
        (this.transform.position, //... desde el centro del mismo player...
        Vector2.down, //... hacia abajo...
        raycastLongitude, //... de longitud tal...
        groundMask)) //... detecta el suelo...
        {
            //animator.enabled = true; //Habilitar la animacion al estar en el suelo
            //GameManager.sharedInstance.currentGameSate = GameState.inGame; //Indica que el estado del juego es inGame cuando personaje toca el piso por primera vez; se hace accediendo al currentGameSate por medio del GameManager y luego su instancia el sharedInstance, y con GameState.(que es el tipo de dato de currentGameState) se indica el estado del juego

            return true; //... devolvera true porque detecta el suelo
        }
        else
        {
            //animator.enabled = false; //Desabilitar la animacion al no estar en el suelo

            return false; //Si no, false porque no lo detecta
        }
    }

    public void Die(){
        float travelledDistance = GetTravelDistance(); //La distancia recorrida en la partida actual, sera la calculada por GetTravelDistance()
        float previousMaxDistance = PlayerPrefs.GetFloat("maxscore", 0f); //Con esto se indica que la distancia maxima previa se guardara en una variable de los PlayerPrefs a la cual se le llamara maxscore, de tipo float, y con 0f se indica que la distancia maxima previa antes de la primer partida es 0
        if (travelledDistance > previousMaxDistance){ //Si la distancia recorrida en la partida actual es mayor a la previa...
            PlayerPrefs.SetFloat("maxscore", travelledDistance); //... con SetFloat() asigna que el valor de la variable maxscore ahora sea el de travelledDistance(la distancia recorrida en la partida actual)
        }

        this.animator.SetBool(STATE_ALIVE, false); //Hacer la animacion de muerte
        GameManager.sharedInstance.GameOver(); //Indicar al Game Manager que es Game Over
    }

    //FUNCIONES DE RECOLECTAR VIDA Y MANA
    public void CollectHealth(int points){ //Recolectar vida
        this.healthPoints += points;
        if (this.healthPoints >= MAX_HEALTH){ //Limitador de vida recolectada. Si los puntos de vida superan la vida maxima...
            this.healthPoints = MAX_HEALTH; //... los puntos de vida quedaran hasta la vida maxima
        }
        if (this.healthPoints <= 0){
            Die();
        }
    }

    public void CollectMana(int points){ //Recolectar mana
        this.manaPoints += points;
        if (this.manaPoints >= MAX_MANA){
            this.manaPoints = MAX_MANA;
        }
    }

    public int GetHealth(){ //Devuelve los puntos de vida
        return healthPoints;
    }

    public int GetMana(){ //Devuelve los puntos de mana
        return manaPoints;
    }

    public float GetTravelDistance(){ //Funcion para calcular la distancia recorrida
        return this.transform.position.x - startPosition.x; //Regresa la posicion en la que esta el player, menos la posicion en la que empezo, ambos  en el eje x
    }
}