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

    private PetState petState;
    private MascotaMovimiento movimientoScript;

    void Start()
    {
        movimientoScript = GetComponent<MascotaMovimiento>();

        CambiarEstado(PetState.Moving);
    }

    public void CambiarEstado(PetState nuevoEstado)
    {
        petState = nuevoEstado;

        switch (petState)
        {
            case PetState.Moving:
                // ¡Prendemos el interruptor! Unity vuelve a ejecutar su Update
                movimientoScript.enabled = true; 
                break;

            case PetState.Pedido:
            case PetState.Eating:
            case PetState.Idle:

                movimientoScript.enabled = false; 
                break;
        }
    }
}
