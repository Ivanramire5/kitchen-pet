using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PuertaInteraccion : MonoBehaviour
{   
    [SerializeField] 
    public float anguloApertura = 90f; // Ángulo de apertura de la puerta
    [SerializeField]
    public float velocidadApertura = 2f; // Velocidad de apertura de la puerta
    private bool puertaAbierta = false; // Estado de la puerta
    private Quaternion rotacionDeCerrado; // Rotación de la puerta cuando se cierra
    private Quaternion rotacionDeAbierto; // Rotación de la puerta cuando se abre
    private Coroutine currentCoroutine;

    void Start()
    {
        rotacionDeCerrado = transform.rotation;
        rotacionDeAbierto = Quaternion.Euler(transform.eulerAngles + new Vector3(0, anguloApertura, 0));
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(AnimarPuerta());
        }
    }
    private IEnumerator AnimarPuerta()
    {
        Quaternion targetRotation = puertaAbierta ? rotacionDeCerrado : rotacionDeAbierto;
        puertaAbierta = !puertaAbierta;

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * velocidadApertura);
            yield return null;
        }

        transform.rotation = targetRotation; // Asegura que la rotación final sea exacta
    }
}
