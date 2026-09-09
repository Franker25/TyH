using System.Collections;
using UnityEngine;

public class Simulacro : MonoBehaviour
{
    private float waitTime = 1.00f;
    private float segundos = 0.00f;
    private float chau = 10.00f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        while (segundos < 0)
        {
            segundos++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        segundos += Time.deltaTime;
            if (segundos > waitTime);
        {
                Debug.Log("El segundos es: " + segundos);
            if (chau > segundos);
                segundos = 0;
                Debug.Log("Chau se activo");
        }
    }
}
