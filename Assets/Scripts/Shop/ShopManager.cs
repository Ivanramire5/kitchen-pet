using System.Collections.Generic;
using UnityEngine;
using TMPro; // Si vas a manejar textos para el total

public class ShopManager : MonoBehaviour
{
    // El Singleton: Una referencia global a este mismo script
    public static ShopManager Instancia;

    [Header("Conexiones Externas")]
    // Referencia al script de tu camión para avisarle cuando hay una compra
    public TruckMovement camion; 

    [Header("Datos del Carrito")]
    // La lista temporal donde se van acumulando los ítems
    public List<FoodData> carritoActual = new List<FoodData>();
    
    //public TextMeshProUGUI textoPrecioTotal;
    //private float precioTotal = 0f;

    private void Awake()
    {
        // Configuramos el Singleton al arrancar el juego
        if (Instancia == null) 
        {
            Instancia = this;
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Esta función la va a llamar cada BotonProductoUI cuando le hagan clic.
    /// </summary>
    public void AgregarAlCarrito(FoodData alimentoAgregado)
    {
        // TAREA PARA VOS:
        // 1. Agregá el 'alimentoAgregado' a la lista 'carritoActual' usando .Add()
        // 2. (Opcional) Sumá el precio del alimento a un contador de precio total
        // 3. Meté un Debug.Log que diga "Agregaste un [nombre del alimento]" para probarlo
    }

    /// <summary>
    /// Esta función la vas a conectar al botón de "Confirmar Compra" en tu UI.
    /// </summary>
    public void ConfirmarCompra()
    {
        // TAREA PARA VOS:
        // 1. Primero, hacé un 'if' para verificar que la lista 'carritoActual' no esté vacía (Count > 0). ¡No queremos llamar al camión por nada!
        
        // 2. EXTRAER LOS IDs: 
        // Acordate que tu función camion.CargarPedido() probablemente pida una List<string> (los IDs), pero tu carrito es de tipo List<FoodData>.
        // Vas a tener que crear una List<string> temporal acá adentro, hacer un foreach que recorra 'carritoActual', y meter los IDs en esa lista de strings.

        // 3. Llamá a tu camión: camion.CargarPedido(listaDeStrings);
        
        // 4. Vaciá el carrito actual con carritoActual.Clear() para dejar la terminal lista para el próximo cliente.
    }
}