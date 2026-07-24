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

    void Start()
    {
        movimientoScript = GetComponent<MascotaMovimiento>();

        if (pacienciaScript != null)
        {
            pacienciaScript.enabled = false; 
        }
        else
        {
            Debug.LogWarning("No se encontró el componente PacienciaManager en el objeto.");
        }

        mirarAlJugador = GameObject.FindGameObjectWithTag("Player");

        if(mirarAlJugador != null)
        {
            jugador = mirarAlJugador.transform;
        }
        else
        {
            Debug.LogWarning("No se encontró el objeto con la etiqueta 'Player'. Asegúrate de que exista en la escena.");
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
                //Calculamos la dirección (Destino - Origen)
                Vector3 direccionAlJugador = transform.position - jugador.position;
                
                direccionAlJugador.y = 0; 

                // Evitamos un error matemático si la mascota está exactamente en el centro del jugador
                if (direccionAlJugador != Vector3.zero)
                {
                    Quaternion rotacionDestino = Quaternion.LookRotation(direccionAlJugador);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDestino, Time.deltaTime * velocidadGiro);
                } 
            }
            
            // Reloj de pedido original
            temporizadorPedido -= Time.deltaTime;

            // ¡EL CAMBIO CLAVE!: Pasamos a paciencia si el jugador presiona 'E' O si el tiempo se acaba
            if (Input.GetKeyDown(KeyCode.E) || temporizadorPedido <= 0)
            {
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
                
                if (pacienciaScript != null)
                {
                    pacienciaScript.enabled = false; 
                }
                break;

            case PetState.Pedido:
                movimientoScript.enabled = false; 

                if(camaraMostrador != null)
                {
                    camaraMostrador.Priority = 20;
                }

                // Encendemos el Ticket Visualmente
                if (controladorUI != null)
                {
                    controladorUI.MostrarTicket(miSpriteHamburguesa, "Hamburguesa", "Punto: Bien cocida");
                }

                if (pacienciaScript != null) pacienciaScript.enabled = false; 
                
                temporizadorPedido = tiempoParaPedir; 
                break;

            case PetState.Paciencia: 
                movimientoScript.enabled = false; 

                if(camaraMostrador != null)
                {
                    camaraMostrador.Priority = 10;
                }

                // Apagamos el Ticket Visualmente
                if (controladorUI != null)
                {
                    controladorUI.OcultarTicket();
                }
                
                if (pacienciaScript != null)
                {
                    pacienciaScript.enabled = true; // ¡Aquí arranca tu barra de paciencia!
                }
                break;
        }
    }
}