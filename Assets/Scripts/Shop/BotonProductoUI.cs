using UnityEngine;
using UnityEngine.UI; 
using TMPro;        

public class BotonProductoUI : MonoBehaviour
{
    [Header("Referencias Visuales (Hijos)")]
    // Estos son los "cables" que conectaremos en el Inspector
    public Image iconoVisual;
    public TextMeshProUGUI textoNombre;
    public TextMeshProUGUI textoPrecio;

    [Header("Datos Internos")]
    // Acá el botón va a guardar la memoria de quién es
    private FoodData miAlimento; 

    /// <summary>
    /// Esta función será llamada desde afuera por el ShopManager al crear el botón.
    /// </summary>
    public void ConfigurarBoton(FoodData datosRecibidos)
    {
        // 1. Guardar 'datosRecibidos' en la memoria del botón
        miAlimento = datosRecibidos;
        
        // 2. Cambiar los textos y la imagen de forma SEGURA (con salvavidas)
        if (textoNombre != null) 
        {
            textoNombre.text = miAlimento.alimentoName;
        }

        if (textoPrecio != null) 
        {
            textoPrecio.text = "$" + miAlimento.precioCompra.ToString("F2");
        }
        
        if (iconoVisual != null) 
        {
            iconoVisual.sprite = miAlimento.iconoUI;
        }
    }

    /// <summary>
    /// Esta función se ejecutará cuando el jugador haga clic en el botón físico de Unity.
    /// </summary>
    public void AlPresionarBoton()
    {
        // Un salvavidas final por si el botón perdió la memoria
        if (miAlimento == null)
        {
            Debug.LogError("¡Error! Este botón está vacío y no sabe qué alimento es. Revisá tu Prefab.");
            return;
        }

        Debug.Log($"¡Hiciste clic en {miAlimento.alimentoName}!");
        ShopManager.Instancia.AgregarAlCarrito(miAlimento);
    }
}