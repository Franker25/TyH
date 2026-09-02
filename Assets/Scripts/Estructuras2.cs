using System.Collections;
using UnityEngine;

public class Estructuras2 : MonoBehaviour
{
    int edad = 28;

    void Start()
    {
        Debug.Log("Primer edad:" + edad);
        for (int i = 0; i < 2; i++) // El int le pone una letra (que no importa cual sea) que da igual a 0, le pone el ; que le permite seguir, marcando que si el I es meor a 2 el for funciones, El I++ hace que se loope. Cuando pasa a ser mayor que dos, frena
        {
            edad++;
            Debug.Log("Pasada por For:" + edad);
        }

        while (edad == 28)
        {
            Debug.Log("Hola");
        }
        Debug.Log("Chau");
    }

    // Update is called once per frame
    void Update()
    {

    }
}