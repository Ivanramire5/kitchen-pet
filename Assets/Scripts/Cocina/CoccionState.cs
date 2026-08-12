using UnityEngine;

/// <summary>
/// Maquina de estados que se encarga de la coccion de los alimentos
/// </summary>
public class CoccionState : MonoBehaviour
{
    public enum EstadoComida
    {
        Vacia,
        Cruda,
        Cocida,
        Quemada
    }
    public EstadoComida estadoActual = EstadoComida.Vacia;

    [Header("Tiempos de coccion")]
    public float tiempoParaCocinarse = 20f; //Tiempo de prueba unicamente
    public float tiempoParaQuemarse = 20f; //Lo mismo. Esto es una prueba. Despues se cambia
    private float temporizador = 0f;
    
    [Header("Sprites de los alimentos")]
    public SpriteRenderer spriteComida;
    public Sprite spriteCrudo;
    public Sprite spriteCocido;
    public Sprite spriteQuemado;

    //El temporizador avanza solo si hay comida cruda o cocida en el sarten.
    void Update()
    {
        if(estadoActual == EstadoComida.Cruda || estadoActual == EstadoComida.Cocida)
        {
            temporizador += Time.deltaTime;

            if(temporizador >= tiempoParaQuemarse && estadoActual != EstadoComida.Quemada)
            {
                CambiarEstado(EstadoComida.Quemada);
            }
            else if(temporizador >= tiempoParaCocinarse && estadoActual == EstadoComida.Cruda)
            {
                CambiarEstado(EstadoComida.Cocida);
            }
        }
    }

    //Esta funcion interactua con la plancha cuando el jugador tiene un ingrediente crudo en la mano
    public void PonerComida(FoodData datosComida)
{
    tiempoParaCocinarse = datosComida.tiempoCoccion;
    tiempoParaQuemarse = datosComida.tiempoCoccion * 2;
    temporizador = 0f;
    CambiarEstado(EstadoComida.Cruda);
}

    private void CambiarEstado(EstadoComida nuevoEstado)
    {
        estadoActual = nuevoEstado;

        switch (estadoActual)
        {
            
            case EstadoComida.Cruda:
                spriteComida.sprite = spriteCrudo;
                break;
            case EstadoComida.Cocida:
                temporizador = 0f;
                Debug.Log("<color=cyan>[CoccionState]</color> ¡La comida se ha cocinado!");
                spriteComida.sprite = spriteCocido;
                break;
            case EstadoComida.Quemada:
                Debug.Log("<color=red>[CoccionState]</color> ¡La comida se ha quemado!");
                spriteComida.sprite = spriteQuemado;
                break;
            case EstadoComida.Vacia:
                spriteComida.sprite = null;
                break;
        }
    }

    // Este sensor se dispara cuando un objeto físico cae sobre la plancha
    private void OnTriggerEnter(Collider other)
    {
        
        if (estadoActual != EstadoComida.Vacia) return;

        IngredienteFisico ingrediente = other.GetComponentInParent<IngredienteFisico>();

        
        if (ingrediente != null && ingrediente.datos != null && ingrediente.datos.sePuedeCocinar)
        {
            Debug.Log($"<color=cyan>[PLANCHA]</color> Empezando a cocinar: {ingrediente.datos.alimentoName}");

            
            PonerComida(ingrediente.datos);

            Destroy(ingrediente.gameObject);
        }
    }
}
