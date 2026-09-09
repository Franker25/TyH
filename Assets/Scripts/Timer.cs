using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    float tiempo = 0;
    private int segundos = 0;
    private bool terminar = false;
    int duracion = 10;
    float segundosrestantes = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //LogicaTimer();
        LogicaCuentaRegresiva();
    }

    void LogicaTimer()
    {

        if (!terminar) // esto significa que es falso
            tiempo += Time.deltaTime; // el Delta Time actualiza el tiempo, es una constante

        if (tiempo >= 1)
        {
            segundos++;
            Debug.Log("Segundos: " + segundos);
            tiempo = 0;

            if (segundos >= duracion)
            {
                terminar = true;
                Debug.Log("Terminado");
            }
        }

    }
    void LogicaCuentaRegresiva()
    {
        if (!terminar) // esto significa que es falso
            tiempo += Time.deltaTime; // el Delta Time actualiza el tiempo, es una constante

        if (tiempo >= 1)
        {
            segundosrestantes--;
            Debug.Log("Tiempo restante: " + segundosrestantes);
            tiempo = 0;

            if (segundosrestantes <= 0)
            {
                terminar = true;
                Debug.Log("Terminado");
            }
        }
    }
}
