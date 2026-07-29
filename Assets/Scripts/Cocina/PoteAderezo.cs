using UnityEngine;

/// <summary>
/// Codigo para poder elegir los potes de aderezo y dibujar con ellos en la comida.
/// </summary>


public class PoteAderezo : MonoBehaviour
{
    [Tooltip("0 = Ketchup, 1 = Mostaza, 2 = Mayonesa, etc.")]
    public int indiceAderezo;

    private ObjetoAgarrable miAgarrable;

    void Awake()
    {
        miAgarrable = GetComponent<ObjetoAgarrable>();
        if (miAgarrable == null) miAgarrable = GetComponentInParent<ObjetoAgarrable>();
    }

    public void Agarrar()
    {
        if (miAgarrable != null) miAgarrable.Agarrar();
    }

    public void Soltar()
    {
        if (miAgarrable != null) miAgarrable.Soltar();
    }
}