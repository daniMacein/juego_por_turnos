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


//**Pruebas
void Start()
  {

    personaje1.Ataque1();

  }


//**Pruebas
public List<Personaje> personajesPrincipalesEnemigos=new List<Personaje>();

//**FUNCIONES DE CONTROLAR ATAQUES

//Obtener los objetivos de un GolpeData 
//Si es area, se le añaden los objetivos correspondientes
List<Personaje> ObtenerObjetivos(Personaje atacante, GolpeData golpe)
{
    switch (golpe.tipoObjetivo)
    {
        case TipoObjetivo.AreaTodosEnemigos:
            return  new List<Personaje>(personajesPrincipalesEnemigos);

        case TipoObjetivo.Unitario:
            return golpe.objetivos;

        default:
            return new List<Personaje>();
    }
}


//Ejecutar un ataque realizado.

//Recorres los golpes, por cada golpe mandas un golpe a cada personaje que lo iba a recibir
//Mientras recopilas la informacion que has hecho en Golpe resultado, y todos esos los recopidas
//En un ataqueresultado por cada personaje dañado. Para luego mandarle esta informacion + decirles
//que han recibido un 

public void EjecutarAtaque(Personaje atacante, AtaqueData ataque)
{
    Dictionary<Personaje, ResultadoAtaque> resultados = new Dictionary<Personaje, ResultadoAtaque>();
    

    foreach (GolpeData golpe in ataque.golpes)
    {

      List<Personaje> objetivosFinales = ObtenerObjetivos(atacante, golpe);
      
      if (golpe.estadoGolpe==EstadoGolpe.Fallado)
      {
        Debug.Log("Golpe fallado vaya :(");
        continue;
      }

        foreach (Personaje objetivo in objetivosFinales)
        {
            // Crear resultado si no existe
            if (!resultados.ContainsKey(objetivo))
            {
                resultados[objetivo] = new ResultadoAtaque();
                resultados[objetivo].objetivo = objetivo;
            }

            // Recibir golpe y devolver el resultado del golpe
            
            ResultadoGolpe res = objetivo.RecibirGolpe(golpe);

            // Guardar info del resultadogolpe en resultadoataque
            resultados[objetivo].golpes.Add(res);

            // Solo sumar si ha sido golpeado (ejemplo básico)
            if (res.estadoGolpe == EstadoGolpe.Golpeado || res.estadoGolpe == EstadoGolpe.Critico)
            {
                resultados[objetivo].dañoTotal += res.dañoFinal;
                resultados[objetivo].armaduraTotal+=res.armaduraReducida;
            }
        }
    }

    // Avisar que ha recibido un ataque, a las personas afectadas (mas pasar la info)
    foreach (var kvp in resultados)
    {
        kvp.Key.AtaqueRecibido(kvp.Value);
    }
}





 #endregion
}




