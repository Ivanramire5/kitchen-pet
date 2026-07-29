using UnityEngine;

/// <summary>
/// Script que gestiona el agarre de objetos por parte del jugador.
/// </summary>
public class AgarreManager : MonoBehaviour
{

    public ObjetoAgarrable objetoEnMano; 
    
    // Nuestro confiable escudo contra clics dobles accidentales
    private float cooldown = 0f;

    void Update()
    {
        if (cooldown > 0) cooldown -= Time.deltaTime;

        // Si tenemos algo en la mano y hacemos Clic Derecho, lo soltamos
        if (Input.GetMouseButtonDown(1) && objetoEnMano != null && cooldown <= 0)
        {
            objetoEnMano.Soltar();
            objetoEnMano = null;
            cooldown = 0.2f; // Activamos el escudo
        }
    }

    // Esta función la llamarán los objetos cuando les hagas clic derecho
    public void IntentarAgarrar(ObjetoAgarrable nuevoObjeto)
    {
        if (objetoEnMano == null && cooldown <= 0)
        {
            objetoEnMano = nuevoObjeto;
            objetoEnMano.Agarrar();
            cooldown = 0.2f; // Activamos el escudo
        }
    }
}
