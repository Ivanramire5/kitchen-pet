using UnityEngine;
/// <summary>
/// Clase que representa los datos de un alimento en el juego. Se utiliza para almacenar información relevante sobre cada alimento, como su ID, nombre, icono, prefab 3D, propiedades de cocina y economía.
/// </summary>
[CreateAssetMenu(fileName = "Nuevo Alimento", menuName = "Cocina/Alimentos de Base de Datos")]
public class FoodData : ScriptableObject
{
    [Header("Información del Alimento")]
    [Tooltip("ID del alimento, debe ser único y que sera usado por el camion, las recetas, etc.")]
    public string alimentoID;
    public string alimentoName;
    public Sprite iconoUI;

    [Header("Fisicas y 3d del alimento")]
    public GameObject prefab3D;

    [Header("Propiedades de Cocina")]
    public bool sePuedeCocinar = false;
    public float tiempoCoccion = 5f;
    public bool sePuedeCortar = false;
    
    [Header("Economía (Configurable en Inspector)")]
    [Tooltip("Cuánto le cuesta al jugador comprar este ingrediente")]
    [Min(0)] 
    public int precioCompra;

    [Tooltip("Cuánto oro da este ingrediente al ser vendido o entregado")]
    [Min(0)]
    public int precioVenta;

    [Tooltip("Ganancia de experiencia o puntaje al procesar este ítem")]
    [Range(0, 100)] 
    public int puntosDeReputacion;

}
