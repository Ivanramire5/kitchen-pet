using UnityEngine;

/// <summary>
/// Estructura que representa un aderezo con su nombre, color y tamaño de pincel.
/// Se utiliza en el SistemaAderezos para dibujar sobre la comida.
/// </summary>

[System.Serializable]
public struct Aderezo
{
    public string nombre;
    public Color colorAderezo;
    [Range(1, 15)] 
    public int tamañoPincel;
}

public class SistemaAderezos : MonoBehaviour
{
    [Header("Configuración de Aderezos")]
    public Aderezo[] listaAderezos;
    
    [Header("Cursores")]
    public Texture2D cursorManito;
    
    private int indiceAderezoActual = -1; 
    private PoteAderezo poteFisicoActual = null;

    private Vector2 ultimoPixelUV = -Vector2.one;
    private Renderer ultimoRendererPintado = null;

    void Update()
    {
        ActualizarFormaCursor();

        // 1. DIBUJAR: Clic Izquierdo (0) presionado SOLO si tenemos una botella en mano
        if (indiceAderezoActual != -1 && Input.GetMouseButton(0)) 
        {
            DibujarAderezoLibre();
        }
        else
        {
            ultimoPixelUV = -Vector2.one;
            ultimoRendererPintado = null;
        }

        // 2. AGARRAR: Clic Derecho (1) SOLO si tenemos las manos vacías
        if (Input.GetMouseButtonDown(1) && indiceAderezoActual == -1) 
        {
            IntentarAgarrarPote();
        }

        // 3. SOLTAR: Tecla 'F' SOLO si tenemos una botella en mano
        if (Input.GetKeyDown(KeyCode.F) && indiceAderezoActual != -1)
        {
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
            if ((hit.transform.CompareTag("Aderezos") || hit.transform.GetComponentInParent<PoteAderezo>() != null) && cursorManito != null)
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
            PoteAderezo poteTocado = hit.transform.GetComponent<PoteAderezo>();
            if (poteTocado == null) poteTocado = hit.transform.GetComponentInParent<PoteAderezo>();
            if (poteTocado == null) poteTocado = hit.transform.GetComponentInChildren<PoteAderezo>();
            
            if (poteTocado != null)
            {
                SeleccionarPote(poteTocado.indiceAderezo, poteTocado);
            }
        }
    }

    public void SeleccionarPote(int indice, PoteAderezo poteFisico)
    {
        if (poteFisicoActual != null) poteFisicoActual.Soltar();

        indiceAderezoActual = indice;
        poteFisicoActual = poteFisico;
        poteFisicoActual.Agarrar(); 
        
        Debug.Log("<color=green>[SISTEMA]</color> Agarraste: " + listaAderezos[indiceAderezoActual].nombre + " | ¡Presiona F para soltar!");
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
        Debug.Log("<color=cyan>[SISTEMA]</color> Pote soltado. Manos vacías.");
    }

    private void DibujarAderezoLibre()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            // Ignorar la botella de salsa por si el rayo la roza
            if (hit.transform.CompareTag("Aderezos") || hit.transform.GetComponentInParent<PoteAderezo>() != null) return;

            Renderer rend = hit.transform.GetComponent<Renderer>();
            
            if (rend != null && rend.material.mainTexture != null)
            {
                // Verificamos si el colisionador es compatible con coordenadas UV
                if (!(hit.collider is MeshCollider))
                {
                    Debug.LogWarning("<color=magenta>[DEBUG DIBUJO]</color> Estás intentando pintar sobre '" + hit.transform.name + "' pero no tiene un Mesh Collider. ¡El SetPixel necesita un MeshCollider para calcular las UVs!");
                    return;
                }

                Texture2D texturaBase = rend.material.mainTexture as Texture2D;

                if (texturaBase.name != "TexturaClonada_Cocina")
                {
                    Texture2D texturaEditable = new Texture2D(texturaBase.width, texturaBase.height, TextureFormat.RGBA32, false);
                    texturaEditable.name = "TexturaClonada_Cocina";
                    
                    RenderTexture rt = RenderTexture.GetTemporary(texturaBase.width, texturaBase.height);
                    Graphics.Blit(texturaBase, rt);
                    RenderTexture actualActiva = RenderTexture.active;
                    RenderTexture.active = rt;
                    texturaEditable.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
                    texturaEditable.Apply();
                    RenderTexture.active = actualActiva;
                    RenderTexture.ReleaseTemporary(rt);

                    rend.material.mainTexture = texturaEditable;
                    texturaBase = texturaEditable;
                }

                Vector2 pixelUVActual = hit.textureCoord;
                pixelUVActual.x *= texturaBase.width;
                pixelUVActual.y *= texturaBase.height;

                Aderezo aderezoActual = listaAderezos[indiceAderezoActual];

                // Primer clic: Pintamos el punto inicial
                if (ultimoPixelUV == -Vector2.one || ultimoRendererPintado != rend)
                {
                    PintarCirculo(texturaBase, (int)pixelUVActual.x, (int)pixelUVActual.y, aderezoActual.tamañoPincel, aderezoActual.colorAderezo);
                }
                else
                {
                    // Interpolación lineal fluida (3x densidad para no dejar huecos al mover el mouse rápido)
                    float distancia = Vector2.Distance(ultimoPixelUV, pixelUVActual);
                    int pasos = Mathf.CeilToInt(distancia * 3f);

                    for (int i = 0; i <= pasos; i++)
                    {
                        float t = pasos == 0 ? 0f : (float)i / pasos;
                        Vector2 puntoIntermedio = Vector2.Lerp(ultimoPixelUV, pixelUVActual, t);
                        PintarCirculo(texturaBase, (int)puntoIntermedio.x, (int)puntoIntermedio.y, aderezoActual.tamañoPincel, aderezoActual.colorAderezo);
                    }
                }

                ultimoPixelUV = pixelUVActual;
                ultimoRendererPintado = rend;

                texturaBase.Apply();
            }
            else
            {
                Debug.LogWarning("<color=magenta>[DEBUG DIBUJO]</color> El rayo golpea a '" + hit.transform.name + "', pero no tiene Renderer o textura principal.");
            }
        }
    }

    private void PintarCirculo(Texture2D textura, int centroX, int centroY, int radio, Color color)
    {
        for (int x = -radio; x <= radio; x++)
        {
            for (int y = -radio; y <= radio; y++)
            {
                if (x * x + y * y <= radio * radio)
                {
                    textura.SetPixel(centroX + x, centroY + y, color);
                }
            }
        }
    }
}