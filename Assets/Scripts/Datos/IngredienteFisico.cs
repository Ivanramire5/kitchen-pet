using UnityEngine;
/// <summary>
/// Script corto que sirve unicamente para que la plancha sepa que objeto se esta cocinando
/// </summary>
public class IngredienteFisico : MonoBehaviour
{
    [Header("Base de Datos")]
    [Tooltip("Arrastra aquí el archivo .asset (ej: ing_salchicha) que le corresponde a este modelo")]
    public FoodData datos;
}
