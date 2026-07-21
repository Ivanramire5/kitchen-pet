using UnityEngine;

/// <summary>
/// Esta clase se encarga de la logica de los waypoints, 
/// que son los puntos que definiran por donde se mueve la mascota
/// </summary>
public class Waypoints : MonoBehaviour
{
    public static Transform[] waypoints;

    void Awake()
    {
        waypoints = new Transform[transform.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = transform.GetChild(i);
        }
    }
}
