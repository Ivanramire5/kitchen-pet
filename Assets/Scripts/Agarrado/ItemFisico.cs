using UnityEngine;
/// <summary>
/// Este script debe ir en los objetos que se puedan agarrar y lanzar. Se recomienda usarlo junto con el script GrabberFisico.cs en el jugador.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class ItemFisico : MonoBehaviour
{
    [Header("Ajustes de Flotación")]
    [Tooltip("A qué distancia de la cámara debe quedar flotando este objeto en metros")]
    public float distanciaDeFlotacion = 1.5f;

    [Tooltip("¿Se puede lanzar con Clic Derecho?")]
    public bool sePuedeLanzar = true;

    [HideInInspector]
    public Rigidbody rb;

    void Awake()
    {
        // Guardamos la referencia al Rigidbody automáticamente al iniciar
        rb = GetComponent<Rigidbody>();
    }
}