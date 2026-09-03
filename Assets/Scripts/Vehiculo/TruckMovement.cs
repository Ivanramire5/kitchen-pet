using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Máquina de estados del funcionamiento del camión.
/// Se encarga de la entrega de los pedidos y de su movimiento.
/// </summary>
public class TruckMovement : MonoBehaviour
{
    public enum TruckState
    {
        EsperandoPedido, // NUEVO: El camión espera oculto a que lo llamen
        YendoEntrega,
        Entregandose,
        Yendose,
        Finalizado
    }

    [Header("Estado Actual")]
    // Empezamos en estado de espera para que no arranque solo al darle Play
    public TruckState estadoActual = TruckState.EsperandoPedido;

    [Header("Nodos/Puntos de ruta")]
    [Tooltip("Punto donde el camión frena para hacer la entrega")]
    public Transform puntoEntrega;
    [Tooltip("Punto hacia donde se va el camión tras entregar")]
    public Transform puntoSalida;

    [Header("Configuración del vehículo")]
    public float velocidad = 6f;
    public float velocidadRotacion = 5f;
    public float velocidadEntrega = 4f;

    [Header("Eventos")]
    public UnityEvent AlLlegarAEntrega;
    public UnityEvent AlTerminarEntrega;
    private float temporizadorEntrega = 0f;

    [Header("Reparto de paquetes")]
    public GameObject prefabCajaReparto;
    public Transform puntoDescarga;

    
    
    public List<FoodData> pedidoActual = new List<FoodData>();

    private Vector3 posicionInicial;


    /// <summary>
    /// Esta función es llamada desde el ShopManager al confirmar la compra
    /// </summary>
    void Start()
    {
        // Guardamos la posición exacta donde pusiste el camión al arrancar el juego
        posicionInicial = transform.position;
    }
    /// <summary>
    /// Esta función es llamada desde el ShopManager al confirmar la compra
    /// </summary>
    public void CargarPedido(List<FoodData> alimentosRecibidos)
    {
        if (alimentosRecibidos == null || alimentosRecibidos.Count == 0)
        {
            Debug.LogWarning("[TRUCK] El pedido recibido está vacío.");
            pedidoActual.Clear();
            return;
        }

        pedidoActual = new List<FoodData>(alimentosRecibidos);
        Debug.Log($"[TRUCK] Pedido cargado con {pedidoActual.Count} elementos. ¡El camión arranca!");

        estadoActual = TruckState.YendoEntrega;
    }

    void Update()
    {
        switch(estadoActual)
        {
            case TruckState.EsperandoPedido:
                // El camión no hace nada hasta que el ShopManager llame a CargarPedido()
                break;

            case TruckState.YendoEntrega:
                MoverHaciaNodo(puntoEntrega.position);

                if(Vector3.Distance(transform.position, puntoEntrega.position) < 0.3f)
                {
                    LlegarAPuntoDeEntrega();
                }
                break;

            case TruckState.Entregandose:
                temporizadorEntrega -= Time.deltaTime;
                if (temporizadorEntrega <= 0f)
                {
                    TerminarEntregaYSalir();
                }
                break;

            case TruckState.Yendose:
                MoverHaciaNodo(puntoSalida.position);
                if(Vector3.Distance(transform.position, puntoSalida.position) < 0.1f)
                {
                    // --- NUEVA LÓGICA DE RECICLAJE ---
                    // 1. Lo teletransportamos de vuelta a su escondite original
                    transform.position = posicionInicial;
                    
                    // 2. Lo ponemos a dormir hasta la próxima compra
                    estadoActual = TruckState.EsperandoPedido;
                    
                    // 3. Limpiamos su caja
                    pedidoActual.Clear();
                    
                    Debug.Log("<color=green>[DELIVERY]</color> El camión volvió a la base y está listo para otra orden.");
                    // ---------------------------------
                }
                break;
        }
    }

    private void MoverHaciaNodo(Vector3 destino)
    {
        transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
    }

    private void LlegarAPuntoDeEntrega()
    {
        estadoActual = TruckState.Entregandose;
        temporizadorEntrega = velocidadEntrega;
        
        Debug.Log("<color=cyan>[DELIVERY]</color> ¡Camión llegó! Descargando pedido");
        
        if (prefabCajaReparto != null && puntoDescarga != null)
        {
            GameObject nuevaCaja = Instantiate(prefabCajaReparto, puntoDescarga.position, puntoDescarga.rotation);
            
            // Inyectamos la lista de modelos 3D a la caja
            CajaReparto scriptCaja = nuevaCaja.GetComponent<CajaReparto>();
            if (scriptCaja != null)
            {
                scriptCaja.CargarPedido(pedidoActual);
            }
        }
        AlLlegarAEntrega?.Invoke();
    }

    private void TerminarEntregaYSalir()
    {
        estadoActual = TruckState.Yendose;
        Debug.Log("<color=orange>[DELIVERY]</color> Comida entregada. El camión se retira.");
        
        AlTerminarEntrega?.Invoke();
    }

    void OnDrawGizmos()
    {
        if (puntoEntrega != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(puntoEntrega.position, 0.5f);
            Gizmos.DrawLine(transform.position, puntoEntrega.position);
        }

        if (puntoEntrega != null && puntoSalida != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(puntoSalida.position, 0.5f);
            Gizmos.DrawLine(puntoEntrega.position, puntoSalida.position);
        }
    }
}