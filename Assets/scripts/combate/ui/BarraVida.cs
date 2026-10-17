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

    private Camera camara;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Vector3 posicion = posVida.transform.position;
        //Fondo.transform.position = posicion;
        ModificarBarraVida();
          camara = Camera.main;

         
    }

    // LateUpdate se ejecuta después de Update (y después de que la cámara se mueva)
   

    public void ModificarBarraVida()
    {
        vidaActual = personaje.vida;
        vidaMaxima = personaje.vidaMaxima;
        BarraDeVida.fillAmount =  (float)vidaActual / vidaMaxima;

    }




    void LateUpdate()
{
     //transform.LookAt(camara.transform);
    OrientarHaciaCamara();

      
}

    public void OrientarHaciaCamara()
    {
        if (camara == null) return;

        // Billboard clásico: mirar hacia la cámara manteniendo el "up" vertical
        transform.LookAt(transform.position + camara.transform.rotation * Vector3.forward,
                         camara.transform.rotation * Vector3.up);
    }
}
