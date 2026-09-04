using UnityEngine;

public class TerminalTienda : MonoBehaviour
{
    [Header("Interfaz a controlar")]
    // Arrastrá acá el GameObject de tu Canvas principal (o el Panel general)
    public GameObject panelTiendaUI;

    [Tooltip("Arrastrá acá el objeto padre que contiene la Billetera y el A Pagar del exterior")]
    public GameObject hudExterior;

    [Header("Control del Jugador")]
    [Tooltip("Arrastrá acá el script que controla el movimiento de la cámara (ej: FirstPersonController, MouseLook, etc)")]
    public MonoBehaviour scriptCamaraJugador;

    [Header("Estado")]
    // Para saber si la tienda está abierta o no
    private bool tiendaAbierta = false;
    // Para saber si el jugador está lo suficientemente cerca
    private bool jugadorCerca = false;

    

    void Start()
    {
        // Por seguridad, nos aseguramos de que la tienda empiece apagada
        if (panelTiendaUI != null)
        {
            panelTiendaUI.SetActive(false);
        }
    }

    void Update()
    {
        
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            AlternarTienda();
        }
        
    }

    // Usamos los Triggers (Colisionadores invisibles) para detectar al jugador
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            Debug.Log("Jugador acaba de entrar a la zona de la tienda. Presiona [E] para abrir/cerrar.");
        }
    }        

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;

            // Y por las dudas, si se aleja con la tienda abierta, la cerrás.
            if (tiendaAbierta)
            {
                AlternarTienda();
            }
        }
    }

    private void AlternarTienda()
    {
        tiendaAbierta = !tiendaAbierta;

        if (panelTiendaUI != null)
        {
            panelTiendaUI.SetActive(tiendaAbierta);
        }

        Cursor.visible = tiendaAbierta;
        Cursor.lockState = tiendaAbierta ? CursorLockMode.None : CursorLockMode.Locked;

        // --- NUEVO: Apagamos o prendemos el script de la cámara ---
        if (scriptCamaraJugador != null)
        {
            // Si la tienda está abierta (true), desactivamos la cámara (false). Por eso usamos el "!" (negación).
            scriptCamaraJugador.enabled = !tiendaAbierta;
        }
        if (hudExterior != null)
        {
            hudExterior.SetActive(!tiendaAbierta);
        }
    }
}