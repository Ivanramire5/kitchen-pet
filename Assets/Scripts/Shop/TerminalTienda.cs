using UnityEngine;

public class TerminalTienda : MonoBehaviour
{
    [Header("Interfaz a controlar")]
    // Arrastrá acá el GameObject de tu Canvas principal (o el Panel general)
    public GameObject panelTiendaUI;

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
        // 1. Invertimos el estado (si estaba true pasa a false, y viceversa)
        tiendaAbierta = !tiendaAbierta;

        // 2. Prendemos o apagamos el Canvas visualmente
        if (panelTiendaUI != null)
        {
            panelTiendaUI.SetActive(tiendaAbierta);
        }

        // 3. ¡EL DETALLE CLAVE DEL MOUSE!
        // Mientras la tienda está abierta, el cursor queda visible y desbloqueado.
        Cursor.visible = tiendaAbierta;
        Cursor.lockState = tiendaAbierta ? CursorLockMode.None : CursorLockMode.Locked;
    }
}