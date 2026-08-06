using System.Collections.Generic;
using UnityEngine;

public class BaseDeDatosComida : MonoBehaviour
{
    public static BaseDeDatosComida Instance;

    [Header("Catalogo completo de alimento")]
    [Tooltip("Arrastra aquí todos los ScriptableObjects creados en la carpeta Datos/Comida")]
    public FoodData[] catalogoComida;

    //Usamos un diccionario para acceder a los alimentos por su ID sin sacrificar rendimientro
    private Dictionary<string, FoodData> diccionarioComida = new Dictionary<string, FoodData>();

    void Awake()
    {
        //Usamos un Singleton para tener un acceso rapido desde cualquier script
        if (Instance == null)
        {
            Instance = this;
            InicializarDiccionario();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InicializarDiccionario()
    {
        diccionarioComida.Clear();
        
        foreach (FoodData alimento in catalogoComida)
        {
            if (alimento != null && !string.IsNullOrEmpty(alimento.alimentoID))
            {
                if (!diccionarioComida.ContainsKey(alimento.alimentoID))
                {
                    diccionarioComida.Add(alimento.alimentoID, alimento);
                }
                else
                {
                    Debug.LogWarning($"<color=red>[BaseDeDatosComida]</color> ¡ID duplicado en la base de datos!: " + alimento.alimentoID);
                }
            }
        }
        Debug.Log($"<color=green>[BaseDeDatosComida]</color> Base de datos cargada exitosamente con {diccionarioComida.Count} ítems.");
    }
    //Cualquier script llama a esta funcion para pedir informacion de un alimento por su ID
    public FoodData ObtenerAlimentoPorID(string alimentoID)
    {
        if (diccionarioComida.TryGetValue(alimentoID, out FoodData alimento))
        {
            return alimento;
        }
        else
        {
            Debug.LogWarning($"<color=red>[BaseDeDatosComida]</color> ¡Alimento no encontrado en la base de datos! ID: " + alimentoID);
            return null;
        }
    }
}
