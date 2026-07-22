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
    public PetState petState;
    public MascotaMovimiento movimientoScript;
    private GameObject mirarAlJugador;

    public PacienciaManager pacienciaScript;

    [SerializeField]
    public float tiempoParaPedir = 10f;
    private float temporizadorPedido;

    void Start()
    {
        movimientoScript = GetComponent<MascotaMovimiento>();

        if (pacienciaScript != null)
        {
            pacienciaScript.enabled = false; 
        }
        else
        {
            Debug.LogWarning("No se encontró el componente PacienciaManager en el objeto.");
        }

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
            temporizadorPedido -= Time.deltaTime;

            Debug.Log("Reloj de Pedido: " + temporizadorPedido);

            if (temporizadorPedido <= 0)
            {
                // ¡Pasamos al estado de esperar la comida!
                CambiarEstado(PetState.Paciencia); 
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
                
                // El blindaje: "Si encontré el script al inicio, apágalo."
                if (pacienciaScript != null)
                {
                    pacienciaScript.enabled = false; 
                }
                break;

            case PetState.Pedido:
                movimientoScript.enabled = false; 
                if (pacienciaScript != null) pacienciaScript.enabled = false; 
                
                temporizadorPedido = tiempoParaPedir; 
                break;

            case PetState.Paciencia: 
                movimientoScript.enabled = false; 
                
                // Lo mismo aquí al encenderlo
                if (pacienciaScript != null)
                {
                    pacienciaScript.enabled = true; 
                }
                break;
        }
    }
}
