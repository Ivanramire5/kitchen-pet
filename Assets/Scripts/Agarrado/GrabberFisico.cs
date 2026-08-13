using UnityEngine;
// <summary>
// Este script permite al jugador agarrar y lanzar objetos físicos con Rigidbody.
// Se recomienda usarlo junto con el script ItemFisico.cs en los objetos que se puedan agarrar.
/// </summary>
public class GrabberFisico : MonoBehaviour
{
    [Header("Referencias de la Cámara")]
    [Tooltip("Un GameObject vacío HIJO de tu cámara principal")]
    public Transform holdPoint;
    public float alcanceRaycast = 4f;

    [Header("Filtro de Objetos")]
    [Tooltip("Selecciona aquí la Layer 'Agarrables' para ignorar mesas, paredes y suelo")]
    public LayerMask capaAgarrable = ~0; // Por defecto detecta todo (~0), pero puedes filtrarlo en el Inspector

    [Header("Fuerzas de Física (Estilo HL2)")]
    public float fuerzaAtraccion = 20f;
    public float amortiguacion = 8f; // Evita que el objeto orbite como un péndulo
    [SerializeField]
    public float fuerzaLanzamiento;
    [SerializeField]
    public float fuerzaDeObjetoSoltado;

    public float distanciaMaximaRuptura = 3f; // Si se traba detrás de un muro y se aleja mucho, se suelta

    private ItemFisico itemAgarrado;
    private float dragOriginal;
    private float angularDragOriginal;

    void Update()
    {
        // CLIC IZQUIERDO: Agarrar o Soltar
        if (Input.GetMouseButtonDown(0))
        {
            if (itemAgarrado == null)
                IntentarAgarrar();
            else
                Soltar();
        }

        // CLIC DERECHO: Lanzar el objeto (Gravity Gun)
        if (Input.GetMouseButtonDown(1) && itemAgarrado != null)
        {
            Lanzar();
        }
    }

    // LAS FÍSICAS DE RIGIDBODY SIEMPRE SE CALCULAN EN FIXEDUPDATE
    void FixedUpdate()
    {
        if (itemAgarrado != null && holdPoint != null)
        {
            MoverObjetoConFisicas();
        }
    }

    private void IntentarAgarrar()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        
        // Lanzamos el rayo usando la LayerMask para no detectar la mesa por error
        if (Physics.Raycast(ray, out RaycastHit hit, alcanceRaycast, capaAgarrable, QueryTriggerInteraction.Ignore))
        {
            // Buscamos el script ItemFisico en el objeto tocado o en su padre
            ItemFisico itemEncontrado = hit.collider.GetComponentInParent<ItemFisico>();

            if (itemEncontrado != null && itemEncontrado.rb != null && !itemEncontrado.rb.isKinematic)
            {
                itemAgarrado = itemEncontrado;

                // 1. Ajustamos la posición del HoldPoint a la distancia personalizada de este ítem
                holdPoint.localPosition = new Vector3(0, 0, itemAgarrado.distanciaDeFlotacion);

                // 2. Apagamos la gravedad mientras lo sostenemos
                itemAgarrado.rb.useGravity = false;

                // 3. Guardamos su resistencia del aire original y le aumentamos la amortiguación
                dragOriginal = itemAgarrado.rb.linearDamping;        // En Unity < 6 usar: .drag
                angularDragOriginal = itemAgarrado.rb.angularDamping; // En Unity < 6 usar: .angularDrag
                
                itemAgarrado.rb.linearDamping = amortiguacion;
                itemAgarrado.rb.angularDamping = amortiguacion;

                Debug.Log("<color=green>[GRABBER]</color> Agarraste: " + itemAgarrado.name);
            }
        }
    }

    private void MoverObjetoConFisicas()
    {
        // Calculamos el vector desde donde está el ítem hasta donde queremos que flote (HoldPoint)
        Vector3 direccionHaciaDestino = holdPoint.position - itemAgarrado.rb.position;
        float distancia = direccionHaciaDestino.magnitude;

        // Si el jugador se movió rápido y el ítem se trabó detrás de una pared, se soltará solo
        if (distancia > distanciaMaximaRuptura)
        {
            Debug.LogWarning("<color=yellow>[GRABBER]</color> El objeto se atascó o alejó demasiado. Soltando.");
            Soltar();
            return;
        }

        // APLICAMOS VELOCIDAD LINEAL: El objeto viaja hacia el nodo pero colisiona real con el entorno
        itemAgarrado.rb.linearVelocity = direccionHaciaDestino * fuerzaAtraccion; // En Unity < 6 usar: .velocity
    }

    private void Soltar()
    {
        if (itemAgarrado == null) return;

        // Restauramos su gravedad y físicas originales
        itemAgarrado.rb.useGravity = true;
        itemAgarrado.rb.linearDamping = dragOriginal;
        itemAgarrado.rb.angularDamping = angularDragOriginal;

        Debug.Log("<color=cyan>[GRABBER]</color> Soltaste: " + itemAgarrado.name);
        itemAgarrado = null;
    }

    private void Lanzar()
    {
        if (itemAgarrado == null) return;

        bool permitiLanzar = itemAgarrado.sePuedeLanzar;
        Rigidbody rbTemporal = itemAgarrado.rb;

        // Lo soltamos primero para restaurar su gravedad
        Soltar();

        // Si el ítem lo permite, le damos un impulso instantáneo hacia el frente de la cámara
        if (permitiLanzar && rbTemporal != null)
        {
            rbTemporal.AddForce(Camera.main.transform.forward * fuerzaLanzamiento, ForceMode.Impulse);
            Debug.Log("<color=orange>[GRABBER]</color> ¡Lanzado!");
        }
    }
}