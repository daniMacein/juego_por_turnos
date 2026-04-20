using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class ControlCombate : MonoBehaviour
{
  // Start is called once before the first execution of Update after the MonoBehaviour is created


  public Personaje personaje1;

  public Personaje personaje2;

  public List<Personaje> personajes = new List<Personaje>();
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


  void CrearPosicion(List<Personaje> personajes, List<int> posicionesIniciales)
  {

    for (int i = 0; i < personajes.Count; i++)
    {
      posicionesIniciales[i] += personajes[i].speed;
      personajes[i].SetPosicion(posicionesIniciales[i]);
      print("posicion" + personajes[i].posicion);
    }
  }

}




