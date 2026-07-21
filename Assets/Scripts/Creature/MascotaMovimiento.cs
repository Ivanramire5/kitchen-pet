using UnityEngine;

public class MascotaMovimiento : MonoBehaviour
{
    public float movimientoSpeed = 5f; // Velocidad de movimiento de la mascota

    private Transform target; // Referencia al objetivo al que la mascota seguirá
    private int waypointIndex = 0; // Índice del waypoint actual
    private PetStateMachine petStateMachine;

    void Start()
    {
        target = Waypoints.waypoints[0]; // Inicializa el objetivo con el primer waypoint
        petStateMachine = GetComponent<PetStateMachine>();
        if (petStateMachine == null)
        {
            Debug.LogWarning("PetStateMachine component not found on Mascota.");
        }
    }

    void Update()
    {
        Vector3 direction = target.position - transform.position; // Calcula la dirección hacia el objetivo
        transform.Translate(direction.normalized * movimientoSpeed * Time.deltaTime, Space.World); // Mueve la mascota hacia el objetivo

        if(Vector3.Distance(transform.position, target.position) <= 2f) // Verifica si la mascota ha llegado al objetivo
        {
            GetNextWaypoint(); // Obtiene el siguiente waypoint
        }
    }

    void GetNextWaypoint()
    {
        if(waypointIndex >= Waypoints.waypoints.Length - 1) // Verifica si se ha llegado al último waypoint
        {
            if (petStateMachine != null)
            {
                petStateMachine.CambiarEstado(PetStateMachine.PetState.Pedido); // Cambia el estado de la mascota a Pedido
            }
            return; // Sale del método para evitar errores de índice fuera de rango
        }
        else
        {
            waypointIndex++; // Incrementa el índice del waypoint
        }
        target = Waypoints.waypoints[waypointIndex]; // Actualiza el objetivo al siguiente waypoint
    }
}
