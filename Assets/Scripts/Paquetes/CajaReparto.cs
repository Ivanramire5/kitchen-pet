using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Script encargado del contenido de las cajas
/// </summary>
public class CajaReparto : MonoBehaviour
{
    [Header("Contenido de la caja")]

    //Aca guardamos todos los IDs de los items
    public List<string> idsContenido = new List<string>();

    public void CargarPedido(List<string> nuevosItems)
    {
        idsContenido = nuevosItems;
        Debug.Log("<color=yellow>[CAJA]</color> La caja fue cargada con" + idsContenido.Count + "items.");
    }
    public void AbrirCaja()
    {
        Debug.Log("<color=green>[CAJA]</color> Abriendo caja de entrega...");

        foreach (string idItem in idsContenido)
        {
            //Aca se consulta la base de datos
            //Se busca el item en una base de datos

            Debug.Log("ID del item extraido" + idItem);
        }
        //Cuando se abre la caja esta desaparece dando mejor rendimiento
        //Se le pueden colocar animaciones
        Destroy(gameObject);
    }
}
