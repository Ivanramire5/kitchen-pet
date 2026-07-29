
using UnityEngine;
/// <summary>
/// Clase que permite que un objeto 3D pueda ser agarrado y soltado por el jugador.
/// Se utiliza para los potes de aderezo en la cocina.
/// </summary>


public class ObjetoAgarrable : MonoBehaviour
{
    private bool estaAgarrado = false;
    private Collider[] misColliders;

    void Awake()
    {
        misColliders = GetComponentsInChildren<Collider>(true);
    }

    void Update()
    {
        if (estaAgarrado)
        {
            Camera camaraActiva = Camera.main;
            if (camaraActiva == null) camaraActiva = FindAnyObjectByType<Camera>();

            if (camaraActiva != null)
            {
                Ray ray = camaraActiva.ScreenPointToRay(Input.mousePosition);
                // 1.2 metros alejado de la cámara y un poco abajo a la derecha
                Vector3 puntoDestino = ray.GetPoint(1.2f);
                puntoDestino.y -= 0.15f; 
                
                transform.position = Vector3.Lerp(transform.position, puntoDestino, Time.deltaTime * 20f);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime * 15f);
            }
        }
    }

    public void Agarrar()
    {
        estaAgarrado = true;
        // Apagamos TODOS los colisionadores de la botella para no bloquear rayos
        foreach (Collider col in misColliders)
        {
            if (col != null) col.enabled = false;
        }
        Debug.Log("<color=yellow>[FISICA]</color> Pote agarrado. Colliders APAGADOS.");
    }

    public void Soltar()
    {
        estaAgarrado = false;

        bool apoyoExitoso = false;

        // 1. PRIMER INTENTO: Rayo vertical hacia abajo desde la botella (¡El más seguro para que no flote!)
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitDown, 50f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            if (hitDown.transform.CompareTag("Mesada"))
            {
                transform.position = hitDown.point + Vector3.up * 0.005f;
                transform.rotation = Quaternion.identity;
                apoyoExitoso = true;
                Debug.Log("<color=green>[APOYO]</color> Apoyado verticalmente en la mesada.");
            }
        }

        // 2. SEGUNDO INTENTO: Si no había mesada justo debajo, usamos el rayo de la cámara
        if (!apoyoExitoso)
        {
            Camera camaraActiva = Camera.main;
            if (camaraActiva == null) camaraActiva = FindAnyObjectByType<Camera>();
            Ray ray = camaraActiva.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitCam, 20f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                if (hitCam.transform.CompareTag("Mesada"))
                {
                    transform.position = hitCam.point + Vector3.up * 0.005f;
                    transform.rotation = Quaternion.identity;
                    apoyoExitoso = true;
                    Debug.Log("<color=green>[APOYO]</color> Apoyado con el puntero en la mesada.");
                }
            }
        }

        if (!apoyoExitoso)
        {
            Debug.LogWarning("<color=red>[APOYO]</color> No se encontró el tag 'Mesada' debajo. Revisa que la mesa tenga el Tag y un Collider no-trigger.");
        }

        // Reactivamos los colisionadores para poder agarrarlo después
        foreach (Collider col in misColliders)
        {
            if (col != null) col.enabled = true;
        }
    }
}