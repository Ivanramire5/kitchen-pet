using System.Collections.Generic;
using UnityEngine;

public class CajaReparto : MonoBehaviour
{
    [Header("Contenido de la caja")]
    public bool cajaAbierta = false;
    public List<FoodData> contenidoCaja = new List<FoodData>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !cajaAbierta)
        {
            AbrirCaja();
        }
    }

    public void CargarPedido(List<string> idsPedido)
    {
        contenidoCaja.Clear();

        if (idsPedido == null)
        {
            Debug.LogWarning("[CAJA] La lista de IDs es nula.");
            return;
        }

        if (BaseDeDatosComida.Instance == null)
        {
            Debug.LogError("[CAJA] No hay BaseDeDatosComida en la escena.");
            return;
        }

        foreach (string idItem in idsPedido)
        {
            if (string.IsNullOrEmpty(idItem))
                continue;

            if (BaseDeDatosComida.Instance.TryGetAlimentoPorID(idItem, out FoodData alimento))
            {
                contenidoCaja.Add(alimento);
                Debug.Log("[CAJA] Encontrado: " + alimento.alimentoName + " | ID: " + alimento.alimentoID);
            }
            else
            {
                Debug.LogWarning("[CAJA] No se encontró el alimento con ID: " + idItem);
            }
        }
    }

    public void AbrirCaja()
    {
        if (cajaAbierta)
            return;

        if (contenidoCaja == null || contenidoCaja.Count == 0)
        {
            Debug.LogWarning("[CAJA] La caja está vacía.");
            return;
        }

        cajaAbierta = true;

        foreach (FoodData alimento in contenidoCaja)
        {
            if (alimento == null)
                continue;

            if (alimento.prefab3D != null)
            {
                Instantiate(alimento.prefab3D, transform.position + Vector3.up * 0.8f, Quaternion.identity);
                Debug.Log("[CAJA] Spawn: " + alimento.alimentoName);
            }
            else
            {
                Debug.LogWarning("[CAJA] Este alimento no tiene prefab3D: " + alimento.alimentoName);
            }
        }

        Destroy(gameObject);
    }
}