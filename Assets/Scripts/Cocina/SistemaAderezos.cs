using UnityEngine;

/// <summary>
/// Clase que gestiona el sistema de aderezos en la cocina.
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
    
    // Usamos -1 para indicar que el jugador tiene las manos vacías
    private int indiceAderezoActual = -1; 
    private PoteAderezo poteFisicoActual = null;

    private bool ignorarClickEsteFrame = false;

    void Update()
    {
        // 1. DIBUJAR: Solo dibuja si tenemos un pote en la mano y hacemos Clic Izquierdo (0)
        if (indiceAderezoActual != -1 && Input.GetMouseButton(0)) 
        {
            DibujarAderezo();
        }

        // 2. SOLTAR POTE: Si haces Clic Derecho (1)
        if (Input.GetMouseButtonDown(1)) 
        {
            // Si lo acabamos de agarrar este frame, solo apagamos el escudo
            if (ignorarClickEsteFrame)
            {
                ignorarClickEsteFrame = false;
            }
            else // Si ya lo teníamos de antes, lo soltamos normalmente
            {
                SoltarPote();
            }
        }
    }

    // Esta función es llamada por los potes físicos cuando les haces clic
    public void SeleccionarPote(int indice, PoteAderezo poteFisico)
    {
        if (poteFisicoActual != null) poteFisicoActual.Soltar();

        indiceAderezoActual = indice;
        poteFisicoActual = poteFisico;
        poteFisicoActual.Agarrar(); 
        
        // Activamos el escudo para no soltarlo inmediatamente
        ignorarClickEsteFrame = true;
        
        Debug.Log("Agarraste el pote de: " + listaAderezos[indiceAderezoActual].nombre);
    }   

    public void SoltarPote() // Recuerda que esto ocurre al hacer Clic Derecho
    {
        if (poteFisicoActual != null)
        {
            poteFisicoActual.Soltar();
            poteFisicoActual = null;
        }
        
        indiceAderezoActual = -1;
        Debug.Log("Soltaste el pote. Manos vacías.");
    }

    private void DibujarAderezo()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Solo pintamos si golpeamos la comida (puedes agregar un Tag "Comida" para ser más preciso)
            Renderer rend = hit.transform.GetComponent<Renderer>();
            
            if (rend != null && rend.material.mainTexture != null)
            {
                Texture2D texturaBase = rend.material.mainTexture as Texture2D;

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
            }
        }
    }
}