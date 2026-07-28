using UnityEngine;
using Unity.Cinemachine;

public class PetStateMachine : MonoBehaviour
{
    public enum PetState
    {
        Idle,
        Moving,
        Watching,
        Pedido,
        Paciencia,
        Eating,
        Sleeping
    }

    private Transform jugador;
    public float velocidadGiro = 5f; 
    public PetState petState;
    public MascotaMovimiento movimientoScript;
    private GameObject mirarAlJugador;

    public PacienciaManager pacienciaScript;

    [SerializeField]
    public float tiempoParaPedir = 10f;
    private float temporizadorPedido;

    [Header("Sistema de Cámaras")]
    public CinemachineCamera camaraMostrador;

    [Header("Conexión con la Interfaz del Ticket")]
    public OrderDialogue controladorUI; 
    public Sprite miSpriteHamburguesa;
    
    [Header("Interacción del Jugador")]
    public GameObject textoAccionPedido; 
    private bool viendoTicket = false; 
    public bool jugadorEnZona = false;

    void Start()
    {
        movimientoScript = GetComponent<MascotaMovimiento>();

        // Nos aseguramos de que el texto empiece apagado al iniciar el juego
        if (textoAccionPedido != null) textoAccionPedido.SetActive(false);

        if (pacienciaScript != null)
        {
            pacienciaScript.enabled = false; 
        }

        mirarAlJugador = GameObject.FindGameObjectWithTag("Player");

        if(mirarAlJugador != null)
        {
            jugador = mirarAlJugador.transform;
        }
        
        CambiarEstado(PetState.Moving);
    }

    void Update()
    {
        switch (petState)
        {
            case PetState.Pedido:

            if (jugador != null)
            {
                Vector3 direccionAlJugador = transform.position - jugador.position;
                direccionAlJugador.y = 0; 

                if (direccionAlJugador != Vector3.zero)
                {
                    Quaternion rotacionDestino = Quaternion.LookRotation(direccionAlJugador);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDestino, Time.deltaTime * velocidadGiro);
                } 
            }
            
            

            // Esta es la logica que hace que el jugador pueda tomar el pedido de la mascota. Se divide en dos fases: la primera vez que presiona 'E' y la segunda vez.
            if (Input.GetKeyDown(KeyCode.E) && jugadorEnZona)
            {
                if (!viendoTicket)
                {
                    // PRIMER TOQUE DE 'E': El jugador decide atender al cliente
                    viendoTicket = true;
                    
                    if (textoAccionPedido != null) textoAccionPedido.SetActive(false); // Apagamos el aviso
                    
                    if (camaraMostrador != null) camaraMostrador.Priority = 20; // Hacemos Zoom al mostrador
                    
                    if (controladorUI != null) controladorUI.MostrarTicket(miSpriteHamburguesa, "Hamburguesa", "Punto: Bien cocida"); // Mostramos la UI
                }
                else
                {
                    // SEGUNDO TOQUE DE 'E': El jugador acepta el pedido y se va a cocinar
                    CambiarEstado(PetState.Paciencia); 
                }
            }
            
            break;
        }
    }

    public void CambiarEstado(PetState nuevoEstado)
    {
        petState = nuevoEstado;

        switch (petState)
        {
            case PetState.Moving:
                movimientoScript.enabled = true; 
                
                if (pacienciaScript != null) pacienciaScript.enabled = false; 
                break;

            case PetState.Pedido:
                movimientoScript.enabled = false; 
                viendoTicket = false; // Reiniciamos la variable
                
                if (jugadorEnZona && textoAccionPedido != null) 
                {
                    textoAccionPedido.SetActive(true);
                }
                else if (textoAccionPedido != null)
                {
                    textoAccionPedido.SetActive(false); // Por las dudas, lo mantenemos apagado
                }


                if (pacienciaScript != null) pacienciaScript.enabled = false; 
                temporizadorPedido = tiempoParaPedir;
                break;

            case PetState.Paciencia: 
                movimientoScript.enabled = false; 
                
                // Por seguridad, nos aseguramos de que el texto de acción quede apagado
                if (textoAccionPedido != null) textoAccionPedido.SetActive(false);

                // Regresamos la cámara
                if(camaraMostrador != null) camaraMostrador.Priority = 10;

                // Apagamos el Ticket Visualmente
                if (controladorUI != null) controladorUI.OcultarTicket();
                
                // Arranca la barra de paciencia
                if (pacienciaScript != null) pacienciaScript.enabled = true; 
                break;
        }
    }

    public void EntrarZonaMostrador()
    {
        jugadorEnZona = true;

        
        if (petState == PetState.Pedido && !viendoTicket && textoAccionPedido != null)
        {
            textoAccionPedido.SetActive(true);
        }
    }

    public void SalirZonaMostrador()
    {
        jugadorEnZona = false;

        if (textoAccionPedido != null)
        {
            textoAccionPedido.SetActive(false);
        }
    }
}