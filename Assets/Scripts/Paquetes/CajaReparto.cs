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

    public void CargarPedido(List<FoodData> alimentosPedido)
    {
        contenidoCaja.Clear();

        if (alimentosPedido == null)
        {
            Debug.LogWarning("[CAJA] La lista de alimentos es nula.");
            return;
        }

        foreach (FoodData alimento in alimentosPedido)
        {
            if (alimento == null)
                continue;

            contenidoCaja.Add(alimento);
            Debug.Log("[CAJA] Encontrado: " + alimento.alimentoName + " | ID: " + alimento.alimentoID);
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