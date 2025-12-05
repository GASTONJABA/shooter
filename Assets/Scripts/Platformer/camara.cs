using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camara : MonoBehaviour
{    // Asigna aquí a tu personaje (arrastra el GameObject)
    public Transform target;

    // Ajusta X (lateral), Y (altura), y Z (distancia detrás)
    // Para Crash, X debería ser 0 o un valor muy pequeño.
    public Vector3 offset = new Vector3(0f, 3f, -7f);
    // Start is called before the first frame update
    void LateUpdate()
    {
        // La cámara se mueve a la misma posición del personaje más el offset
        transform.position = target.position + offset;

        // Opcional: Asegura que la cámara siempre mire al personaje
        transform.LookAt(target);
    }
}
