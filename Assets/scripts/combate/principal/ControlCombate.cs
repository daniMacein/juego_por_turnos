using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class ControlCombate : MonoBehaviour
{



  public Personaje personaje1;

  //public Personaje personaje2;

//  public List<Personaje> personajes = new List<Personaje>();


  #region  Posicion

 /*
  void Start()
  {

    //jugador1.Ataque1();
    //jugador2.Ataque1();
    List<int> posicionesIniciales = new List<int>();
    posicionesIniciales=ObtenerPosicionInicial(personajes);
    CrearPosicion(personajes,posicionesIniciales);



  }


  List<int> ObtenerPosicionInicial(List<Personaje> personajes)
  {
    List<int> posicionesIniciales = new List<int>();
    for (int i = 0; i < personajes.Count; i++)
    {
      posicionesIniciales.Add(UnityEngine.Random.Range(1, 101));
      print("posicion" + posicionesIniciales[i]);
    }
    return posicionesIniciales;
  }

//Sumamos las posicionesIniciales a la speed de cada jugador. Y  le asignamos su posicion correspondiente.

  void CrearPosicion(List<Personaje> personajes, List<int> posicionesIniciales)
  {
    //recorremos los personajes
    for (int i = 0; i < personajes.Count; i++)
    {
      posicionesIniciales[i] += personajes[i].speed; //obtenemos la velocidad del personaje

       List<int> posicionenviadas= new List<int>(); //creamos la lista con todas las posiciones que tendra
      while (posicionesIniciales[i]>100) //hacemos un bucle y añadimos cada 100 una nueva posicion 

      {
        posicionenviadas.Add(100);
        posicionesIniciales[i]-=100;
      }
       posicionenviadas.Add(posicionesIniciales[i]); //añadimos la menor de 100
       personajes[i].SetPosicion(posicionenviadas); //las enviamos
      
      print("posicion" +string.Join(", ", personajes[i].posicion) );
    }
  }

*/
 #endregion


 #region  combate


void Start()
  {

    personaje1.Ataque1();

  }


 public void EjecutarAtaque(Personaje atacante, AtaqueData ataque)
    {
        foreach (GolpeData golpe in ataque.golpes)
        {
            foreach (Personaje objetivo in golpe.objetivos)
            {
                objetivo.RecibirGolpe(golpe);
            }
        }
    }





 #endregion
}




