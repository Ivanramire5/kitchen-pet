using System.Collections.Generic;
using UnityEngine;
using TMPro; // Si vas a manejar textos para el total

public class ShopManager : MonoBehaviour
{
    // El Singleton: Una referencia global a este mismo script
    public static ShopManager Instancia;

    [Header("Generación de Catálogo")]
    // 1. Acá vas a meter todos los alimentos que existan en el juego
    public List<FoodData> baseDeDatosComida;
    
    // 2. El Prefab azul (el molde de la tarjeta)
    public GameObject productoPrefab;        
    
    // 3. El Panel de la UI que tiene el Grid Layout Group
    public Transform contenedorProductos;  

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

    private void Start()
    {
        // Apenas arranca la escena, armamos los botones del catálogo
        GenerarCatalogo();
    }

    /// <summary>
    /// Crea una tarjeta visual por cada alimento en la base de datos.
    /// </summary>
    public void GenerarCatalogo()
    {
        // Recorremos cada alimento en nuestra base de datos
        foreach (FoodData alimento in baseDeDatosComida)
        {
            // 1. Clonamos el Prefab y lo metemos adentro del contenedor (el Grid Layout Group lo acomoda)
            GameObject nuevaTarjeta = Instantiate(productoPrefab, contenedorProductos);

            // 2. Buscamos el script individual que está pegado en esa tarjeta clonada
            BotonProductoUI scriptTarjeta = nuevaTarjeta.GetComponent<BotonProductoUI>();

            // 3. Le pasamos los datos del alimento actual para que configure sus textos e imagen
            scriptTarjeta.ConfigurarBoton(alimento);
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
        // 1. Verificar que el carrito no esté vacío
        if (carritoActual.Count == 0)
        {
            Debug.Log("El carrito está vacío. No se puede confirmar la compra.");
            return;
        }

        // 2. EXTRAER LOS IDs: 
        List<string> idsParaElCamion = new List<string>();

        // Recorremos cada alimento en el carrito y extraemos su ID
        foreach(FoodData alimento in carritoActual)
        {
            idsParaElCamion.Add(alimento.alimentoID);
        }

        // Verificamos que el camión esté asignado en el Inspector
        if (camion == null)
        {
            Debug.LogError("[SHOP] No hay referencia al TruckMovement en el ShopManager.");
            return;
        }

        // Enviamos la lista de IDs al camión
        camion.CargarPedido(idsParaElCamion);
        Debug.Log("Pedido enviado al camión con " + idsParaElCamion.Count + " ítems.");

        // 3. Limpiar el carrito después de confirmar la compra
        carritoActual.Clear();
        Debug.Log("Carrito limpiado después de la compra.");
    }
}