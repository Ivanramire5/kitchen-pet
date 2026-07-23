using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PacienciaManager : MonoBehaviour
{
    public Image pacienciaBar;

    [SerializeField]
    public float pacienciaAmount = 100f;
    [SerializeField]
    public float velocidadDeCaida = 10f;

    public PetStateMachine cerebro;
    void Start()
    {
        
    }

    
    void Update()
    {
        DecrasePaciencia(velocidadDeCaida * Time.deltaTime);
        if (pacienciaAmount <= 0)
        {
            if (cerebro != null)
            {
                cerebro.CambiarEstado(PetStateMachine.PetState.Idle);
                Debug.Log("Game Over");
                this.enabled = false;
            }
            else
            {
                Debug.LogError("¡Falta asignar la variable 'cerebro' en el Inspector de PacienciaManager!");
            }
        }

    
    }

    public void DecrasePaciencia(float paciencia)
    {
        pacienciaAmount -= paciencia;
        
        // El blindaje: "Solo actualiza la imagen si de verdad hay una asignada"
        if (pacienciaBar != null)
        {
            pacienciaBar.fillAmount = pacienciaAmount / 100f;
        }
    }

    public void Paciencia(float patienceAmount)
    {
        pacienciaAmount += patienceAmount;
        pacienciaAmount = Mathf.Clamp(pacienciaAmount, 0, 100);

        pacienciaBar.fillAmount = pacienciaAmount / 100f;
    }
}
