using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tempo : MonoBehaviour
{

    private float segundos, minutos, horas;
    public Text horarioD;



    // Start is called before the first frame update
    void Start()
    {

        segundos = minutos = horas = 0;

    }

    void AtualizaHorario()
    {
        segundos += Time.deltaTime;

        if (segundos >= 60)
        {
            minutos++;
            segundos = 0;
        }

        if (minutos >= 60)
        {
            horas++;
            minutos = 0;
            segundos = 0;
        }

        if (horas == 12)
        {
            horas = 0;
            minutos = 0;
            segundos = 0;
        }

        horarioD.text = horas.ToString() + ":" + minutos.ToString() + ":" + segundos.ToString("0");
    }

    // Update is called once per frame
    void Update()
    {
        AtualizaHorario();
    }

}