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
        speed = 20;

        probCritico = 10;
        probEvasion = 10;

        potencia = 1;
        probGolpe = 100;
        alteracionDaño = 1;

        //nombre="Caballero";
        clase="Guerrero";



         ataquesInfo[0] = new AtaqueInfo(
            "Tajo",
            "Golpea al enemigo con un tajo"
        );

        ataquesInfo[1] = new AtaqueInfo(
            "Robo de vida",
            "Golpea dos veces"
        );

        ataquesInfo[2] = new AtaqueInfo(
            "Particion de daño",
            "Se acerca y golpea con la espada"
        );

        ataquesInfo[3] = new AtaqueInfo(
            "Golpe mortal",
            "Hce 500 de daño."
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
        LanzarAtaque(1, new SelectorData(1, equipoEnemigo), CrearAtaque2);

    }

    public override void Ataque3()
    {
        LanzarAtaque(1, new SelectorData(1, Equipo.ambosEquipos), CrearAtaque1);

    }

    public override void Ataque4()
    {
        LanzarAtaque(3, new SelectorData(1, equipoEnemigo), CrearAtaque4);

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

        HerirEfecto herirEfecto = new HerirEfecto();
        golpe1.efectos.Add(herirEfecto);
        golpe1.aplicaEfecto=true;
        golpe1.penetracionArmadura=0f;
        AplicarEstadisticasAGolpe(golpe1);




        GolpeData golpe2 = new GolpeData
        (50, objetivos, TipoAtaque.Daño, TipoObjetivo.AreaEnemigos,TipoAnimacion.Ataque1_Golpe1);

        golpe2.penetracionArmadura=0f;
        AplicarEstadisticasAGolpe(golpe2);



        AtaqueData ataque = new AtaqueData(golpe1,golpe2);

        ataque.EstablecerObjetivoMelee(objetivos[0]);




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


        List<Personaje>unoMismo= new List<Personaje>();
        unoMismo.Add(this);
        
        GolpeData golpe2 = new GolpeData(50, unoMismo, TipoAtaque.Curacion,TipoObjetivo.Unitario,TipoAnimacion.Ataque1_Golpe1);

        AplicarEstadisticasAGolpe(golpe1);
        AplicarEstadisticasAGolpe(golpe2);

        AtaqueData ataque = new AtaqueData(golpe1,golpe2);
        ataque.EstablecerObjetivoMelee(objetivos[0]);


        // ejecutar ataque

        controlCombate.EmpezarAtaque(this, ataque);


    }


        private void CrearAtaque3(List<Equipo> ambosEquipos)

    {
        /*
        List<Personaje>objetivos= new List<Personaje>();

        objetivos.Add(ambosEquipos[0]);

        foreach (Personaje objetivo in objetivos)
        {
            Debug.Log("el seleccionado es: " + objetivo.nombre);
        }

        //  Golpe principal

        GolpeData golpe1 = new GolpeData
        (100, objetivos, TipoAtaque.Daño, TipoObjetivo.Unitario,TipoAnimacion.Ataque1_Golpe1);

        List<Personaje>aliado= new List<Personaje>();
        aliado.Add(AlgoritmosCombate.OrdenarPersonajesVidas(aliados)[0]);

        GolpeData golpe2 = new GolpeData
        (100, objetivos, TipoAtaque.Curacion, TipoObjetivo.Unitario,TipoAnimacion.Ataque1_Golpe1);

        golpe1.penetracionArmadura=0f;
        AplicarEstadisticasAGolpe(golpe1);

        AtaqueData ataque = new AtaqueData(golpe1);
        ataque.EstablecerObjetivoMelee(objetivos[0]);


        // ejecutar ataque

        controlCombate.EmpezarAtaque(this, ataque);

    */
    }


    
        private void CrearAtaque4(List<Personaje> objetivos)

    {
        foreach (Personaje objetivo in objetivos)
        {
            Debug.Log("el seleccionado es: " + objetivo.nombre);
        }

        //  Golpe principal

        GolpeData golpe1 = new GolpeData
        (500, objetivos, TipoAtaque.Daño, TipoObjetivo.Unitario,TipoAnimacion.Ataque1_Golpe1);

    
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
