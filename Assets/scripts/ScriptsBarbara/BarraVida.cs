using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine.UI;



public class BarraVida : MonoBehaviour
{
    public Image BarraDeVida; 
    public int vidaActual; 
    public int vidaMaxima;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vidaActual = 1000;
        vidaMaxima = 1000;
    }

    // Update is called once per frame
    void Update()
    {
        BarraDeVida.fillAmount =  (float)vidaActual / vidaMaxima;
    }
}
