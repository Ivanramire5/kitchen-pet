using System.Collections.Generic;
using UnityEngine;
using TMPro; // Si vas a manejar textos para el total

public class ShopManager : MonoBehaviour
{
    // El Singleton: Una referencia global a este mismo script
    public static ShopManager Instancia;

    [Header("Generación de Catálogo")]
    //Acá vas a meter todos los alimentos que existan en el juego
    public List<FoodData> baseDeDatosComida;
    
    //El Prefab azul (el molde de la tarjeta)
    public GameObject productoPrefab;        
    
    // 3. El Panel de la UI que tiene el Grid Layout Group
    public Transform contenedorProductos;  

    [Header("Conexiones Externas")]
    // Referencia al script de tu camión para avisarle cuando hay una compra
    public TruckMovement camion; 

    // --- NUEVO: SECCIÓN DE ECONOMÍA Y UI ---
    [Header("Economía y UI")]
    public float dineroJugador = 100f; 
    [SerializeField]
    public TextMeshProUGUI textoDineroJugador; 
    [SerializeField]
    public TextMeshProUGUI textoTotalCarrito;  
    [Tooltip("El texto que va adentro de tu panel dineroStore")]
    public TextMeshProUGUI textoDineroStore;

    [Header("Datos del Carrito")]
    // La lista temporal donde se van acumulando los ítems
    public List<FoodData> carritoActual = new List<FoodData>();
    

    private float costoTotalCarrito = 0f;

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

        // --- NUEVO: ACTUALIZAR PANTALLA AL INICIO ---
        // Mostramos tu dinero actual apenas arranca el juego
        ActualizarTextosUI(); 
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

        // --- NUEVO: SUMAR COSTO Y MOSTRARLO EN PANTALLA ---
        // ATENCIÓN: Asegurate de que tu script FoodData tenga una variable 'public float precio;'
        costoTotalCarrito += alimentoAgregado.precioCompra; // Sumamos el precio del alimento al total del carrito
        ActualizarTextosUI(); // Refrescamos el texto de la UI

        Debug.Log($"Se agregó {alimentoAgregado.alimentoName} al carrito. Ahora hay {carritoActual.Count} ítems en el carrito. Total actual: ${costoTotalCarrito}");
    }

    /// <summary>
    /// Esta función la vas a conectar al botón de "Confirmar Compra" en tu UI.
    /// </summary>
    public void ConfirmarCompra()
    {
        if (carritoActual.Count == 0)
        {
            Debug.LogWarning("El carrito está vacío.");
            return;
        }

        if (dineroJugador < costoTotalCarrito)
        {
            Debug.LogWarning("No tenés suficiente dinero.");
            return; 
        }

        // Cobramos
        dineroJugador -= costoTotalCarrito;

        List<FoodData> alimentosParaElCamion = new List<FoodData>(carritoActual);

        if (camion != null)
        {
            camion.CargarPedido(alimentosParaElCamion);
            Debug.Log($"El camión arrancó con {alimentosParaElCamion.Count} paquetes.");
        }
        // ------------------------------------------------------------------

        carritoActual.Clear();
        costoTotalCarrito = 0f;
        ActualizarTextosUI(); 
    }
    // --- NUEVO: FUNCIÓN PARA MANEJAR LA UI ---
    /// <summary>
    /// Actualiza los números en los textos de tu interfaz (Canvas).
    /// </summary>
    private void ActualizarTextosUI()
    {
        // Verificamos que no estén vacíos en el Inspector para que Unity no tire error
        if (textoDineroJugador != null)
        {
            // El "F2" hace que el número se vea con 2 decimales (ej: $15.50)
            textoDineroJugador.text = "Billetera: $" + dineroJugador.ToString("F2");
        }
            
        if (textoTotalCarrito != null)
        {
            textoTotalCarrito.text = "A Pagar: $" + costoTotalCarrito.ToString("F2");
            textoDineroStore.text = "$" + dineroJugador.ToString("F2");
        }
    }
}