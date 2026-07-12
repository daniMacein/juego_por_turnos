using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class PruebaPersonaje : Personaje
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

        vidaMaxima = 1000;
        vida = 1000;
        armadura = 30;
        speed = 50;

        probCritico = 100;
        probEvasion = 5;

        potencia = 1;
        probGolpe = 100;
        alteracionDaño = 1;



    }

    public override IEnumerator AnimarAtaque(GolpeData golpeData, List<Personaje> objetivosFinales)
    {
        
        switch(golpeData.tipoAnimacion)
{
    case TipoAnimacion.Ataque1_Golpe1:
        
         yield return new WaitForSeconds(2f);
        break;

    case TipoAnimacion.Magia_Fuego:
        Debug.Log("Anim fuego");
         yield return new WaitForSeconds(1f);
        break;
}
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

        

        selector.Reset();
    }


    public Personaje enemigo;


    

    private void CrearAtaque(Personaje objetivo)

    {
        Debug.Log("el seleccionado es: " +objetivo.nombre);
        //  Golpe principal

        GolpeData golpe1 = new GolpeData
        (100, new List<Personaje> { objetivo }, TipoAtaque.Daño, TipoObjetivo.Unitario,TipoAnimacion.Ataque1_Golpe1);

       // GolpeData golpe2 = new GolpeData(30,new List<Personaje> { objetivo }, TipoAtaque.Daño,TipoObjetivo.AreaTodos,TipoAnimacion.Ataque1_Golpe1);
        golpe1.penetracionArmadura=0f;
        AplicarEstadisticasAGolpe(golpe1);
        // golpe2=AplicarEstadisticasAGolpe(golpe2);

        AtaqueData ataque = new AtaqueData(golpe1);
        

        // ejecutar ataque
        
        controlCombate.EmpezarAtaque(this, ataque);

       
    }


  
}
