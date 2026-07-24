using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderDialogue : MonoBehaviour
{
    [Header("Contenedor Principal")]
    [Tooltip("Arrastra aquí el PanelTicket para poder prenderlo y apagarlo")]
    [SerializeField] private GameObject panelTicket; 

    [Header("Elementos del Ticket")]
    [Tooltip("Arrastra aquí el objeto IconoPedido")]
    [SerializeField] private Image iconoPedido; 
    
    [Tooltip("Arrastra aquí el objeto Pedido (Título/Nombre)")]
    [SerializeField] private TextMeshProUGUI textoTituloPedido; 
    
    [Tooltip("Arrastra aquí el objeto TextoPedido (Descripción/Detalles)")]
    [SerializeField] private TextMeshProUGUI textoDetallePedido; 

    private void Start()
    {
        // Por seguridad, nos aseguramos de que el ticket empiece oculto al iniciar el juego
        OcultarTicket();
    }

    /// <summary>
    /// Llama a esta función desde tu PetStateMachine pasándole los datos de la comida
    /// </summary>
    public void MostrarTicket(Sprite nuevoIcono, string titulo, string detalle)
    {
        // 1. Asignamos la información a la interfaz
        if (nuevoIcono != null) iconoPedido.sprite = nuevoIcono;
        textoTituloPedido.text = titulo;
        textoDetallePedido.text = detalle;

        // 2. Encendemos el panel para que aparezca en pantalla
        panelTicket.SetActive(true);
    }

    /// <summary>
    /// Llama a esta función cuando el jugador presione 'E' para aceptar el pedido
    /// </summary>
    public void OcultarTicket()
    {
        panelTicket.SetActive(false);
    }
}
