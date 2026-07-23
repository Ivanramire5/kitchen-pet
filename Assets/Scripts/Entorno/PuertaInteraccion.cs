using System.Collections;
using TMPro; // Cambia a 'using UnityEngine.UI;' si no usas TextMeshPro
using UnityEngine;

public class PuertaInteraccion : MonoBehaviour
{   
    [Header("Configuración de Animación")]
    [SerializeField] public float anguloApertura = 90f;
    [SerializeField] public float velocidadApertura = 2f;
    
    [Header("Configuración de UI e Interacción")]
    public GameObject mensajeUI; // Arrastra tu TextoInteraccion del Canvas
    public string textoAbrir = "[E] Abrir Puerta";
    public string textoCerrar = "[E] Cerrar Puerta";

    private bool puertaAbierta = false;
    private bool jugadorCerca = false;
    private Quaternion rotacionDeCerrado;
    private Quaternion rotacionDeAbierto;
    private Coroutine currentCoroutine;
    private TextMeshProUGUI textoComponente;

    void Start()
    {
        rotacionDeCerrado = transform.rotation;
        rotacionDeAbierto = Quaternion.Euler(transform.eulerAngles + new Vector3(0, anguloApertura, 0));

        if (mensajeUI != null)
        {
            textoComponente = mensajeUI.GetComponent<TextMeshProUGUI>();
            mensajeUI.SetActive(false); // Ocultamos el cartel al iniciar
        }
    }

    void Update()
    {
        // Solo responde a la tecla 'E' si el jugador está dentro del Trigger
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(AnimarPuerta());
        }
    }

    private IEnumerator AnimarPuerta()
    {
        Quaternion targetRotation = puertaAbierta ? rotacionDeCerrado : rotacionDeAbierto;
        puertaAbierta = !puertaAbierta;

        // Actualizamos el texto dinámicamente según si la puerta se está abriendo o cerrando
        ActualizarTextoUI();

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * velocidadApertura);
            yield return null;
        }

        transform.rotation = targetRotation;
    }

    // --- DETECCIÓN DE JUGADOR Y CONTROL DE UI ---

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            if (mensajeUI != null)
            {
                ActualizarTextoUI();
                mensajeUI.SetActive(true); // Mostramos el cartel
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            if (mensajeUI != null)
            {
                mensajeUI.SetActive(false); // Ocultamos el cartel al alejarnos
            }
        }
    }

    private void ActualizarTextoUI()
    {
        if (textoComponente != null)
        {
            // Muestra "Cerrar" si la puerta ya está abierta, o "Abrir" si está cerrada
            textoComponente.text = puertaAbierta ? textoCerrar : textoAbrir;
        }
    }
}