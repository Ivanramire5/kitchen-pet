using UnityEngine;
public class PetStateMachine : MonoBehaviour
{
    
    public enum PetState
    {
        Idle,
        Moving,
        Watching,
        Pedido,
        Paciencia,
        Eating,
        Sleeping
    }

    private Transform jugador;
    public float velocidadGiro = 5f; 
    private PetState petState;
    private MascotaMovimiento movimientoScript;
    private GameObject mirarAlJugador;

    void Start()
    {
        movimientoScript = GetComponent<MascotaMovimiento>();

        CambiarEstado(PetState.Moving);
        mirarAlJugador = GameObject.FindGameObjectWithTag("Player");

        if(mirarAlJugador != null)
        {
            jugador = mirarAlJugador.transform;
        }
        else
        {
            Debug.LogWarning("No se encontró el objeto con la etiqueta 'Player'. Asegúrate de que exista en la escena.");
        }
        CambiarEstado(PetState.Moving);
    }

    void Update()
    {
        switch (petState)
        {
            case PetState.Pedido:

            if (jugador != null)
            {
                //Calculamos la dirección (Destino - Origen)
                Vector3 direccionAlJugador = transform.position - jugador.position;
                
                direccionAlJugador.y = 0; 

                // Evitamos un error matemático si la mascota está exactamente en el centro del jugador
                if (direccionAlJugador != Vector3.zero)
                {

                    Quaternion rotacionDestino = Quaternion.LookRotation(direccionAlJugador);

                    transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDestino, Time.deltaTime * velocidadGiro);
                }
            }
            break;
        }
    }

    public void CambiarEstado(PetState nuevoEstado)
    {
        petState = nuevoEstado;

        switch (petState)
        {

            case PetState.Moving:
                movimientoScript.enabled = true; 
                break;

            case PetState.Pedido:

            case PetState.Idle:
                movimientoScript.enabled = false; 
                break;
        }
    }
}
