using UnityEngine;
using System.Collections.Generic;
public class PruebaPersonaje : Personaje
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        
        vidaMaxima=860;
        vida=760;
        armadura=30;
        speed=50;
        
        probCritico=8;
        probEvasion=4;

        potencia=1;
        probGolpe=100;
        alteracionDaño=1;

        

    }

  



    public override void Ataque1()
    {
      
        CrearAtaque();
    }


    public Personaje enemigo;


 public ControlCombate controlCombate;

    private void CrearAtaque()

    {
        
     //  Golpe principal
        
        GolpeData golpe1 = new GolpeData(2000,new List<Personaje> { enemigo }, TipoAtaque.Curacion,TipoObjetivo.Unitario);
        
        //GolpeData golpe2 = new GolpeData(30,new List<Personaje> { enemigo }, TipoAtaque.Daño,TipoObjetivo.Unitario);
          
        golpe1=AplicarEstadisticasAGolpe(golpe1);
       // golpe2=AplicarEstadisticasAGolpe(golpe2);

        AtaqueData ataque = new AtaqueData(golpe1);

        
        // ejecutar ataque
        controlCombate.EjecutarAtaque(this.GetComponent<Personaje>(), ataque);
    }
}
