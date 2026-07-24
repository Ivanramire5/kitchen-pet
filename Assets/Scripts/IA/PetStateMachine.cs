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
    public GameObject textoAccionPedido; // <-- AQUÍ ARRASTRARÁS TU TEXTO DE "PRESIONAR E"
    private bool viendoTicket = false;   // <-- Variable interna para saber en qué fase estamos

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
            
            temporizadorPedido -= Time.deltaTime;

            // LÓGICA DE INTERACCIÓN DE 2 PASOS
            if (Input.GetKeyDown(KeyCode.E))
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
            else if (temporizadorPedido <= 0)
            {
                // Si el tiempo se acaba en cualquiera de las dos fases, pasa a paciencia automáticamente
                CambiarEstado(PetState.Paciencia);
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
                
                // ENCENDEMOS EL TEXTO DE ACCIÓN (pero NO la cámara ni el ticket todavía)
                if (textoAccionPedido != null) textoAccionPedido.SetActive(true);

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
}