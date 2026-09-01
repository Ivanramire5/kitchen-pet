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
        carritoActual.Add(alimentoAgregado);

        Debug.Log($"Se agregó {alimentoAgregado.alimentoName} al carrito. Ahora hay {carritoActual.Count} ítems en el carrito.");
    }

    /// <summary>
    /// Esta función la vas a conectar al botón de "Confirmar Compra" en tu UI.
    /// </summary>
    public void ConfirmarCompra()
    {
        // TAREA PARA VOS:
        // 1. Primero, hacé un 'if' para verificar que la lista 'carritoActual' no esté vacía (Count > 0). ¡No queremos llamar al camión por nada!
        if (carritoActual.Count == 0)
        {
            Debug.Log("El carrito está vacío. No se puede confirmar la compra.");
            return;
        }
        // 2. EXTRAER LOS IDs: 
        List<string> idsParaElCamion = new List<string>();

        //Recorremos cada alimento en el carrito y extraemos su ID
        foreach(FoodData alimento in carritoActual)
        {
            idsParaElCamion.Add(alimento.alimentoID);
        }

        if (camion == null)
        {
            Debug.LogError("[SHOP] No hay referencia al TruckMovement en el ShopManager.");
            return;
        }

        camion.CargarPedido(idsParaElCamion);
        Debug.Log("Pedido enviado al camion con " + idsParaElCamion.Count + " ítems.");

        // 3. Limpiar el carrito después de confirmar la compra
        carritoActual.Clear();
        Debug.Log("Carrito limpiado después de la compra.");
    }
}