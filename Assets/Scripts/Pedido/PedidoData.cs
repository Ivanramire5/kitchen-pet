using UnityEngine;

[CreateAssetMenu(fileName = "NuevoPedido", menuName = "Restaurante/Pedido Data")]
public class PedidoData : ScriptableObject
{
    [Header("Información Básica")]
    public string idPedido; // Ej: "combo_hamburguesa"
    public string nombreMostrado; // Ej: "Hamburguesa"
    
    [Header("Diálogo / UI")]
    public Sprite iconoComida; // La imagen que se verá en el bocadillo sobre la cabeza
    [TextArea(2, 4)]
    public string textoDialogo; // Ej: "¡Hola! Quisiera una hamburguesa bien cargada."

    [Header("Parámetros del Juego")]
    public float tiempoPacienciaBase = 15f; // Cuántos segundos esperará por este plato
    public int recompensaMonedas = 50;
}
