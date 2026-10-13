using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class GuerreroPrueba : Personaje
{


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

        vidaMaxima = 1000;
        vida = 1000;
        armadura = 30;
        speed = 200;

        probCritico = 10;
        probEvasion = 25;

        potencia = 1;
        probGolpe = 90;
        alteracionDaño = 1;

        //nombre="Caballero";
        clase="Guerrero";



         ataquesInfo[0] = new AtaqueInfo(
            "Tajo",
            "Golpea al enemigo con un tajo"
        );

        ataquesInfo[1] = new AtaqueInfo(
            "Doble tajada",
            "Golpea dos veces"
        );

        ataquesInfo[2] = new AtaqueInfo(
            "Golpe cercano",
            "Se acerca y golpea con la espada"
        );

        ataquesInfo[3] = new AtaqueInfo(
            "Arrodillarse",
            "Nada."
        );



    }

    public override IEnumerator AnimarAtaque(GolpeData golpeData, List<Personaje> objetivosFinales)
    {

        switch(golpeData.tipoAnimacion)
{
    case TipoAnimacion.Ataque1_Golpe1:
        animator.SetTrigger("TajoVertical");
         yield return new WaitForSeconds(0.7f);
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
        LanzarAtaque(2, new SelectorData(1, equipoEnemigo), CrearAtaque3);

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

        GolpeData golpe2 = new GolpeData(30, objetivos, TipoAtaque.Daño,TipoObjetivo.AreaEnemigos,TipoAnimacion.Ataque1_Golpe1);

        AplicarEstadisticasAGolpe(golpe1);
        AplicarEstadisticasAGolpe(golpe2);

        AtaqueData ataque = new AtaqueData(golpe1,golpe2);


        // ejecutar ataque

        controlCombate.EmpezarAtaque(this, ataque);


    }


        private void CrearAtaque3(List<Personaje> objetivos)

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
        ataque.EstablecerObjetivoMelee(objetivos[0]);


        // ejecutar ataque

        controlCombate.EmpezarAtaque(this, ataque);


    }

/*
    public override void AtaqueRecibido(ResultadoAtaque resultadoAtaque)
    {
        base.AtaqueRecibido(resultadoAtaque);

        animator.SetTrigger("RecibirDano");
    }*/



}
