using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine.UI;



public class BarraVida : MonoBehaviour
{
    public Image BarraDeVida; 
    public GameObject Fondo;
    private float vidaActual; 
    private float vidaMaxima;
    public GameObject posVida;
    public Personaje personaje;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 posicion = posVida.transform.position;
        Fondo.transform.position = posicion;
        ModificarBarraVida();
    }

    // Update is called once per frame

    public void ModificarBarraVida()
    {
        vidaActual = personaje.vida;
        vidaMaxima = personaje.vidaMaxima;
        BarraDeVida.fillAmount =  (float)vidaActual / vidaMaxima;

    }
}
