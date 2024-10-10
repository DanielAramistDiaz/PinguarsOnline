using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CounDown : MonoBehaviour
{
    public float countdownTime = 3600f;  // Tiempo en segundos para la cuenta atrás (ejemplo: 1 hora = 3600 segundos)
    public TextMeshProUGUI countdownText;           // Referencia al componente UI Text para mostrar el tiempo

    private float currentTime;

    void Start()
    {
        currentTime = countdownTime;  // Inicializamos el tiempo actual con el valor de inicio
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;  // Restamos el tiempo que pasa en cada frame

            // Calculamos horas, minutos y segundos
            int hours = Mathf.FloorToInt(currentTime / 3600);
            int minutes = Mathf.FloorToInt((currentTime % 3600) / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);

            // Actualizamos el texto con el formato HH:MM:SS
            countdownText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        }
        else
        {
            countdownText.text = "Time's Up!";  // Mensaje cuando el tiempo se acaba
        }
    }

}
