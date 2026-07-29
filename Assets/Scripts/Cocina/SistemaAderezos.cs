
using UnityEngine;

/// <summary>
/// Estructura que representa un aderezo con su nombre, color y tamaño de pincel.
/// </summary>

[System.Serializable]
public struct Aderezo
{
    public string nombre;
    public Color colorAderezo;
    [Range(1, 10)] 
    public int tamañoPincel;
}

public class SistemaAderezos : MonoBehaviour
{
    [Header("Configuración de Aderezos")]
    public Aderezo[] listaAderezos;
    
    [Header("Cursores")]
    [Tooltip("Arrastra aquí tu textura PNG de la manito")]
    public Texture2D cursorManito;
    
    private int indiceAderezoActual = -1; 
    private PoteAderezo poteFisicoActual = null;

    void Update()
    {
        ActualizarFormaCursor();

        // 1. DIBUJAR: Mantener Clic Izquierdo (0)
        if (indiceAderezoActual != -1 && Input.GetMouseButton(0)) 
        {
            DibujarAderezo();
        }

        // 2. AGARRAR: Clic Derecho (1) solo si tenemos las manos vacías
        if (Input.GetMouseButtonDown(1) && indiceAderezoActual == -1) 
        {
            IntentarAgarrarPote();
        }

        // 3. SOLTAR: Presionar la tecla 'F' solo si tenemos un pote en la mano
        if (Input.GetKeyDown(KeyCode.F) && indiceAderezoActual != -1)
        {
            Debug.Log("<color=cyan>[DEBUG SOLTAR]</color> Soltaste el pote presionado 'F'.");
            SoltarPote();
        }
    }

    private void ActualizarFormaCursor()
    {
        if (indiceAderezoActual != -1)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.GetComponent<PoteAderezo>() != null && cursorManito != null)
            {
                Cursor.SetCursor(cursorManito, Vector2.zero, CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    private void IntentarAgarrarPote()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            Debug.Log("<color=yellow>[DEBUG AGARRE]</color> El Clic Derecho golpeó a: <b>" + hit.transform.name + "</b>");

            PoteAderezo poteTocado = hit.transform.GetComponent<PoteAderezo>();
            
            if (poteTocado != null)
            {
                SeleccionarPote(poteTocado.indiceAderezo, poteTocado);
            }
            else
            {
                Debug.LogWarning("<color=red>[DEBUG AGARRE]</color> El objeto " + hit.transform.name + " no es un PoteAderezo.");
            }
        }
    }

    public void SeleccionarPote(int indice, PoteAderezo poteFisico)
    {
        if (poteFisicoActual != null) poteFisicoActual.Soltar();

        indiceAderezoActual = indice;
        poteFisicoActual = poteFisico;
        poteFisicoActual.Agarrar(); 
        
        Debug.Log("Agarraste el pote de: " + listaAderezos[indiceAderezoActual].nombre + ". ¡Presiona F para soltar!");
    }

    public void SoltarPote()
    {
        if (poteFisicoActual != null)
        {
            poteFisicoActual.Soltar();
            poteFisicoActual = null;
        }
        
        indiceAderezoActual = -1;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Debug.Log("Manos vacías.");
    }

    private void DibujarAderezo()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            // Ya no ignoramos silenciosamente: si golpea un pote, es porque su Collider sigue prendido
            if (hit.transform.GetComponent<PoteAderezo>() != null)
            {
                Debug.LogWarning("<color=red>[DEBUG DIBUJO ERROR]</color> Estás apuntando a " + hit.transform.name + ". ¡Su Collider no se apagó al agarrarlo!");
                return;
            }

            Debug.Log("<color=orange>[DEBUG DIBUJO]</color> El rayo golpea a: <b>" + hit.transform.name + "</b>");

            Renderer rend = hit.transform.GetComponent<Renderer>();
            
            if (rend != null && rend.material.mainTexture != null)
            {
                Texture2D texturaBase = rend.material.mainTexture as Texture2D;

                if (!texturaBase.isReadable)
                {
                    Debug.LogError("<color=red>[DEBUG DIBUJO ERROR]</color> La textura de " + hit.transform.name + " NO tiene marcado 'Read/Write Enabled' en Project.");
                    return;
                }

                Vector2 pixelUV = hit.textureCoord;
                pixelUV.x *= texturaBase.width;
                pixelUV.y *= texturaBase.height;

                Aderezo aderezoActual = listaAderezos[indiceAderezoActual];

                for (int x = -aderezoActual.tamañoPincel; x <= aderezoActual.tamañoPincel; x++)
                {
                    for (int y = -aderezoActual.tamañoPincel; y <= aderezoActual.tamañoPincel; y++)
                    {
                        texturaBase.SetPixel((int)pixelUV.x + x, (int)pixelUV.y + y, aderezoActual.colorAderezo);
                    }
                }

                texturaBase.Apply();
                Debug.Log("<color=green>[DEBUG DIBUJO]</color> ¡Píxeles pintados sobre " + hit.transform.name + "!");
            }
        }
    }
}