using UnityEngine;
using Unity.Cinemachine;

public class EstacionAderezos : MonoBehaviour
{
    [Header("Camaras")]
    public CinemachineCamera vcamAderezos;

    [Header("Sistemas")]
    public SistemaAderezos scriptDibujo;

    private bool enModoPreparacion = false;
    private bool jugadorCerca = false;

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            AlternarModoPreparacion();
        }
    }
    private void AlternarModoPreparacion()
    {
        enModoPreparacion = !enModoPreparacion;

        if (enModoPreparacion)
        {
            vcamAderezos.Priority = 20;
            scriptDibujo.enabled = true; 
            
            // --- ¡AQUÍ ESTÁ LA MAGIA! Liberamos y mostramos el cursor ---
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            vcamAderezos.Priority = 10;
            scriptDibujo.enabled = false; 
            
            // --- Bloqueamos y ocultamos el cursor para volver a jugar ---
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) jugadorCerca = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            // Si el jugador se aleja, forzamos la salida del modo preparación
            if (enModoPreparacion) AlternarModoPreparacion();
        }
    }
}
