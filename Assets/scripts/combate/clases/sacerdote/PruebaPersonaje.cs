using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class PruebaPersonaje : Personaje
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

        vidaMaxima = 860;
        vida = 760;
        armadura = 30;
        speed = 50;

        probCritico = 8;
        probEvasion = 4;

        potencia = 1;
        probGolpe = 100;
        alteracionDaño = 1;



    }




    public override void Ataque1()
    {
        StartCoroutine(EsperarTarget());
        
    }

    
    IEnumerator EsperarTarget()
    {
        selector.Reset();

        // activar modo selección UI
        Debug.Log("Selecciona objetivo...");

        yield return new WaitUntil(() => selector.haySeleccion);

        Personaje objetivoTemporal = selector.seleccionado;

        CrearAtaque(objetivoTemporal);

        haElegidoAccion = true;

        selector.Reset();
    }


    public Personaje enemigo;


    

    private void CrearAtaque(Personaje objetivo)

    {
        Debug.Log("el seleccionado es" +objetivo.nombre);
        //  Golpe principal

        GolpeData golpe1 = new GolpeData(90, new List<Personaje> { objetivo }, TipoAtaque.Daño, TipoObjetivo.Unitario);

        //GolpeData golpe2 = new GolpeData(30,new List<Personaje> { enemigo }, TipoAtaque.Daño,TipoObjetivo.Unitario);

        golpe1 = AplicarEstadisticasAGolpe(golpe1);
        // golpe2=AplicarEstadisticasAGolpe(golpe2);

        AtaqueData ataque = new AtaqueData(golpe1);


        // ejecutar ataque
        
        controlCombate.EjecutarAtaque(this, ataque);

        Debug.Log("ataque creado");
    }
}
