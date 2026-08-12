using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Maquina de estados del funcionamiento del camion
/// Se encarga de la entrega de los pedidos y del movimiento del mismo
/// </summary>
public class TruckMovement : MonoBehaviour
{
    public enum TruckState
    {
        YendoEntrega,
        Entregandose,
        Yendose,
        Finalizado
    }

    [Header("Estado Actual")]
    public TruckState estadoActual = TruckState.YendoEntrega;

    [Header("Nodos/Puntos de ruta")]
    [Tooltip("Este punto es en donde el camion frena y hace tu entrega")]
    public Transform puntoEntrega;
    [Tooltip("Este punto es el que indica hacia donde se va el camion")]
    public Transform puntoSalida;

    [Header("Configuracion del vehiculo")]
    public float velocidad = 6f;
    public float velocidadRotacion = 5f;
    public float velocidadEntrega = 4f;

    [Header("Eventos")]
    [Header("Aquí puedes arrastrar funciones para que aparezca la caja de comida cuando el camión llegue")]
    public UnityEvent AlLlegarAEntrega;
    public UnityEvent AlTerminarEntrega;
    public float temporizadorEntrega = 0f;

    [Header("Reparto de paquetes")]
    public GameObject prefabCajaReparto;
    public Transform puntoDescarga;
    public List<string> pedidoActual = new List<string>
    {
        "panchito_id",
        "kebab_id",
    };


    void Update()
    {
        switch(estadoActual)
        {
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
                    estadoActual = TruckState.Finalizado;
                    Debug.Log("<color=green>[DELIVERY]</color> El camión terminó su recorrido.");
                    // Aquí puedes destruirlo, desactivarlo o regresarlo al inicio (Pool)
                    Destroy(gameObject);
                }
                break;
            case TruckState.Finalizado:

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
            
            // 2. LE INYECTAMOS LOS DATOS DE NUESTRA BASE DE DATOS / PEDIDO
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
    //GIZMOS: Dibuja las líneas de la calle en la escena de Unity
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
