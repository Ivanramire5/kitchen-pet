using UnityEngine;

public class InteractionText : MonoBehaviour
{
    [Header("UI de Interacción")]
    [Tooltip("Arrastra aquí tu texto de 'Presiona E' del Canvas")]
    public GameObject textoInteraccion; 

    // Usamos un contador por si juntas mucho dos mesas, 
    // para que el texto no parpadee ni se apague por error al salir de una y entrar a otra.
    private int objetosCercanos = 0; 

    void Start()
    {
        // Nos aseguramos de que el texto empiece apagado
        if (textoInteraccion != null) textoInteraccion.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si chocamos con cualquier cosa que tenga el tag "Interactuable"
        if (other.CompareTag("Interactuable"))
        {
            objetosCercanos++;
            if (textoInteraccion != null) textoInteraccion.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactuable"))
        {
            objetosCercanos--;
            
            // Solo apagamos el texto si ya no hay NINGÚN objeto cerca
            if (objetosCercanos <= 0)
            {
                objetosCercanos = 0;
                if (textoInteraccion != null) textoInteraccion.SetActive(false);
            }
        }
    }
}
