using System.Collections;
using UnityEngine;

public class Estructuras : MonoBehaviour
{
    int edad = 23; // Esto es una funcion global, entra tanto en el void Start y Update. Si esta dentro de los void serian unicos a esos voids
    string nombre = "Franco";

    bool verdad = false;

    void Start()
    {
        if (verdad == false)
        {
            edad = 24;
        }
        else // Sino ejecutamos esto
        {
            edad = 30;
        }
            
        Debug.Log("Edad N1:" + edad); // El + une a la edad, no la está sumando

        if (edad == 24) // SI esto se Cumple. Cuando comparamos es doble igual
        {
            edad = 25;
            // Se ejecuta esto
        }

        Debug.Log("Edad N2:" + edad);
    }
}