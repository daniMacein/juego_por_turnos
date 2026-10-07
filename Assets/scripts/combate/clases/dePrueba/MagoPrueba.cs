using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class MagoPrueba : Personaje
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
        probGolpe = 100;
        alteracionDaño = 1;

        nombre="Azulio ";
        clase="Mago";



         ataquesInfo[0] = new AtaqueInfo(
            "Bola",
            "Golpea al enemigo"
        );

        ataquesInfo[1] = new AtaqueInfo(
            "Doble descarga",
            "Golpea dos veces"
        );

        ataquesInfo[2] = new AtaqueInfo(
            "Explosion de fuego",
            "Inflige daño en area."
        );

        ataquesInfo[3] = new AtaqueInfo(
            "Aumentar poder",
            "Nada"
        );



    }

    public override IEnumerator AnimarAtaque(GolpeData golpeData, List<Personaje> objetivosFinales)
    {
        
        switch(golpeData.tipoAnimacion)
{
    case TipoAnimacion.Ataque1_Golpe1:
        
        AnimarPersonajes perso= GetComponentInChildren<AnimarPersonajes>();
        StartCoroutine(perso.AnimacionAtaqueCerca());
        break;

    case TipoAnimacion.Magia_Fuego:
        Debug.Log("Anim fuego");
         yield return new WaitForSeconds(1f);
        break;
}
    }



 
 public override void Ataque1()
    {
        //Tajo: 1 objetivo del equipo enemigo
        LanzarAtaque(0, new SelectorData(1, equipoEnemigo), CrearAtaque1);

    }

    public override void Ataque2()
    {
        //Doble tajada: 2 objetivos del equipo enemigo
        LanzarAtaque(1, new SelectorData(2, equipoEnemigo), CrearAtaque2);

    }

    public override void Ataque3()
    {
        LanzarAtaque(2, new SelectorData(1, equipoEnemigo), CrearAtaque1);

    }

    public override void Ataque4()
    {
        LanzarAtaque(3, new SelectorData(1, equipoEnemigo), CrearAtaque1);

    }





    private void CrearAtaque1(List<Personaje> objetivos)

    {
        foreach (Personaje objetivo in objetivos)
        {
            Debug.Log("el seleccionado es: " + objetivo.nombre);
        }

        //  Golpe principal

        GolpeData golpe1 = new GolpeData
        (100, objetivos, TipoAtaque.Daño, TipoObjetivo.Unitario,TipoAnimacion.Ataque1_Golpe1);

        golpe1.penetracionArmadura=0f;
        AplicarEstadisticasAGolpe(golpe1);

        AtaqueData ataque = new AtaqueData(golpe1);


        // ejecutar ataque

        controlCombate.EmpezarAtaque(this, ataque);


    }



    private void CrearAtaque2(List<Personaje> objetivos)

    {
        foreach (Personaje objetivo in objetivos)
        {
            Debug.Log("el seleccionado es: " + objetivo.nombre);
        }

        //  Golpe principal

        GolpeData golpe1 = new GolpeData
        (100, objetivos, TipoAtaque.Daño, TipoObjetivo.Unitario,TipoAnimacion.Ataque1_Golpe1);

        GolpeData golpe2 = new GolpeData(30, objetivos, TipoAtaque.Daño,TipoObjetivo.AreaTodos,TipoAnimacion.Ataque1_Golpe1);

        AplicarEstadisticasAGolpe(golpe1);
        AplicarEstadisticasAGolpe(golpe2);

        AtaqueData ataque = new AtaqueData(golpe1,golpe2);


        // ejecutar ataque

        controlCombate.EmpezarAtaque(this, ataque);


    }



  
}
