using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GuerreroPrueba : Personaje
{
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

        vidaMaxima = 1000;
        vida = 1000;
        armadura = 30;
        speed = 50;

        probCritico = 10;
        probEvasion = 5;

        potencia = 1;
        probGolpe = 90;
        alteracionDaño = 1;



         ataquesInfo[0] = new AtaqueInfo(
            "Golpe divino",
            "Golpea al enemigo con el poder sagrado."
        );

        ataquesInfo[1] = new AtaqueInfo(
            "Curación",
            "Restaura una cantidad de vida a un aliado."
        );

        ataquesInfo[2] = new AtaqueInfo(
            "Luz sagrada",
            "Inflige daño sagrado a los enemigos."
        );

        ataquesInfo[3] = new AtaqueInfo(
            "Bendición",
            "Aumenta temporalmente las estadísticas de un aliado."
        );



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
