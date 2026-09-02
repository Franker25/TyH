using UnityEngine;
using System.Collections; 

public class Test : MonoBehaviour
{
    string nombre = "Franker";
    int edad = 25;
    int fechadenacimiento = 25102002;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(nombre);
        Debug.Log(edad);
        Debug.Log(fechadenacimiento);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Hola");
    }
}
