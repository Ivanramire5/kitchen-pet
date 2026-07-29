using UnityEngine;

/// <summary>
/// Codigo para poder elegir los potes de aderezo y dibujar con ellos en la comida.
/// </summary>
public class PoteAderezo : MonoBehaviour
{
    public int indiceAderezo; // Este índice lo asignamos en el Inspector para cada pote
    public SistemaAderezos sistemaPrincipal;
    
    private Vector3 posicionOriginal;
    private Quaternion rotacionOriginal;
    private bool estaAgarrado = false;

    void Start()
    {
        // Guardamos su posición original para saber a dónde devolverlo
        posicionOriginal = transform.position;
        rotacionOriginal = transform.rotation;
    }

    private void OnMouseOver()
    {
        // Preguntamos si presionó el clic derecho (1)
        if (Input.GetMouseButtonDown(1))
        {
            if (sistemaPrincipal != null && sistemaPrincipal.enabled && !estaAgarrado)
            {
                sistemaPrincipal.SeleccionarPote(indiceAderezo, this);
            }
        }
    }

    void Update()
    {
        if (estaAgarrado)
        {
            // Magia matemática: Creamos un plano invisible un poquito más alto que la mesa
            Plane planoMesa = new Plane(Vector3.up, posicionOriginal + (Vector3.up * 0.5f));
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            // Disparamos un rayo desde el mouse hacia ese plano
            if (planoMesa.Raycast(ray, out float distancia))
            {
                // Movemos el modelo 3D del pote suavemente hacia el puntero
                Vector3 puntoDestino = ray.GetPoint(distancia);
                transform.position = Vector3.Lerp(transform.position, puntoDestino, Time.deltaTime * 15f);
            }
        }
    }

    // Estas funciones son llamadas por el Sistema Principal
    public void Agarrar()
    {
        estaAgarrado = true;
    }

    public void Soltar()
    {
        estaAgarrado = false;
        // Lo devolvemos a su lugar y rotación exactos en la mesa
        transform.position = posicionOriginal;
        transform.rotation = rotacionOriginal;
    }
}