using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base de datos central de alimentos.
/// Carga todos los FoodData del proyecto y los expone por key = alimentoID.
/// </summary>
public class BaseDeDatosComida : MonoBehaviour
{
    public static BaseDeDatosComida Instance;

    [Header("Catalogo completo de alimento")]
    [Tooltip("Arrastra aquí todos los ScriptableObjects FoodData que formen parte del juego.")]
    public FoodData[] catalogoComida;

    private Dictionary<string, FoodData> diccionarioComida = new Dictionary<string, FoodData>();

    private void Awake()
    {
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
            if (alimento == null || string.IsNullOrEmpty(alimento.alimentoID))
                continue;

            if (!diccionarioComida.ContainsKey(alimento.alimentoID))
            {
                diccionarioComida[alimento.alimentoID] = alimento;
            }
            else
            {
                Debug.LogWarning($"<color=red>[BaseDeDatosComida]</color> ID duplicado: {alimento.alimentoID}");
            }
        }

        Debug.Log($"<color=green>[BaseDeDatosComida]</color> Base de datos cargada con {diccionarioComida.Count} alimentos.");
    }

    public bool TryGetAlimentoPorID(string alimentoID, out FoodData alimento)
    {
        return diccionarioComida.TryGetValue(alimentoID, out alimento);
    }

    public FoodData ObtenerAlimentoPorID(string alimentoID)
    {
        if (diccionarioComida.TryGetValue(alimentoID, out FoodData alimento))
            return alimento;

        Debug.LogWarning($"<color=red>[BaseDeDatosComida]</color> ¡Alimento no encontrado en la base de datos! ID: {alimentoID}");
        return null;
    }
}