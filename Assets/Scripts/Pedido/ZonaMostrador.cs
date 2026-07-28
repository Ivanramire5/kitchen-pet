using UnityEngine;

public class ZonaMostrador : MonoBehaviour
{
    [Tooltip("Arrastra aquí a la mascota que está siendo atendida")]
    public PetStateMachine mascotaEnMostrador;


    //Cuando la mascota entra al mostrador se puede tomar su pedido
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && mascotaEnMostrador != null)
        {

            mascotaEnMostrador.EntrarZonaMostrador();
        }
    }

    //Cuando la mascota sale del mostrador ya no se puede tomar su pedido
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && mascotaEnMostrador != null)
        {

            mascotaEnMostrador.SalirZonaMostrador();
        }
    }
}
