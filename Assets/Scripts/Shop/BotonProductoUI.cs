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

    // --- FUNCIONES QUE VAS A PROGRAMAR ---

    /// <summary>
    /// Esta función será llamada desde afuera por el ShopManager al crear el botón.
    /// </summary>
    public void ConfigurarBoton(FoodData datosRecibidos)
    {
        // 1. Guardar 'datosRecibidos'
        miAlimento = datosRecibidos;
        
        // 2. Cambiar el .text de 'textoNombre' por el nombre del alimento
        textoNombre.text = miAlimento.alimentoName;
        
        // 3. Cambiar el .text de 'textoPrecio' por el precio del alimento (convertido a string)
        textoPrecio.text = "$" + miAlimento.precioCompra.ToString();
        
        // 4. Cambiar el .sprite de 'iconoVisual' por el icono del alimento
        iconoVisual.sprite = miAlimento.iconoUI;
    }

    /// <summary>
    /// Esta función se ejecutará cuando el jugador haga clic en el botón físico de Unity.
    /// </summary>
    public void AlPresionarBoton()
    {
        
        Debug.Log("¡Hiciste clic en el producto!");
        ShopManager.Instancia.AgregarAlCarrito(miAlimento);
    }
}