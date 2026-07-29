using UnityEngine;
/// <summary>
/// Script que representa un objeto que puede ser agarrado por el jugador.
/// </summary>
public class ObjetoAgarrable : MonoBehaviour
{
    private Vector3 posicionOriginal;
    private Quaternion rotacionOriginal;
    private bool estaAgarrado = false;
    private Collider miCollider;
    
    // Busca automáticamente a la mano maestra
    private AgarreManager mano; 

    void Start()
    {
        posicionOriginal = transform.position;
        rotacionOriginal = transform.rotation;
        miCollider = GetComponent<Collider>();
        mano = FindAnyObjectByType<AgarreManager>(); // Se conecta solo al Cerebro
    }

    private void OnMouseOver()
    {
        // Si le hacemos Clic Derecho, le pide a la mano que lo agarre
        if (Input.GetMouseButtonDown(1) && !estaAgarrado)
        {
            if (mano != null) mano.IntentarAgarrar(this);
        }
    }

    void Update()
    {
        // La lógica de seguir al mouse
        if (estaAgarrado)
        {
            Plane plano = new Plane(Vector3.up, posicionOriginal + (Vector3.up * 0.5f));
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (plano.Raycast(ray, out float distancia))
            {
                transform.position = Vector3.Lerp(transform.position, ray.GetPoint(distancia), Time.deltaTime * 15f);
            }
        }
    }

    public void Agarrar()
    {
        estaAgarrado = true;
        
        // APAGAMOS EL COLLIDER DEL POTE Y DE TODOS SUS HIJOS (por si tiene tapa o etiquetas en 3D)
        Collider[] todosLosColliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in todosLosColliders)
        {
            col.enabled = false;
        }
    }

    public void Soltar()
    {
        estaAgarrado = false;
        transform.position = posicionOriginal;
        transform.rotation = rotacionOriginal;
        
        // VOLVEMOS A ENCENDER LOS COLLIDERS PARA PODER AGARRARLO DESPUÉS EN LA MESA
        Collider[] todosLosColliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in todosLosColliders)
        {
            col.enabled = true;
        }
    }
}
