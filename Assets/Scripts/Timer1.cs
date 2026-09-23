using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    float tiempo = 0;
    private int segundos = 0;
    private bool terminar = false;
    int duracion = 10;
    float segundosrestantes = 10;
    int[] segundospares = new int[5]; // los [] se marcan como array, la parte de new int con el 5, marca que va a hacer la "tabla de array 5 veces
    int posicionarray = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //LogicaTimer();
        LogicaTimer();
        MostrarParesWhile();
    }

    void LogicaTimer()
    {

        if (!terminar) // esto significa que es falso
            tiempo += Time.deltaTime; // el Delta Time actualiza el tiempo, es una constante

        if (tiempo >= 1)
        {
            segundos++;
            tiempo = 0;

            if (segundos >= duracion)
            {
                terminar = true;
                Debug.Log("Terminado");
            }

            if (segundos % 2 == 0) // El % funciona para marcar el modulo, lo cual permite marcar el 2 que el 0 son iguales
            {
                Debug.Log("Segundos: " + segundos + "- PAR");
                GuardarNumeroPar(); // Creamos un void nuevo, para llamar cuando ocurre lo anterior
            }
            else
            {
                Debug.Log("Segundos: " + segundos);
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
    void GuardarNumeroPar()
    {
        segundospares[posicionarray] = segundos; //Marcamos la posicion array con los segundos, y le sumamos ++ para que continue la tabla llamando el array de segundospares
        posicionarray++;
    }
    void MostrarPares()
    {
        for (int i = 0; i < segundospares.Length; i++) // marca que si es mayor que el largo del array de los segundos pares termina
        {
            Debug.Log("Par: " + segundospares[i]); // aca tenemos en cuenta cada array de los segundos pares
        }
    }
    void MostrarParesWhile()
    {
        int index = 0;
        while (index < 5)
        {
            if (segundospares[index] >= 5) 
                Debug.Log("Par: " + segundospares[index]);
            index++;
        }
    }
}
