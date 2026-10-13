using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.UIElements;
public class ControlCombate : MonoBehaviour
{



  public Personaje personaje1;

  //public Personaje personaje2;

  //  public List<Personaje> personajes = new List<Personaje>();

 public Transform []posicionesA;
  public Transform []posicionesB;

  void Start()
  {
    interfazCombate= InterfazCombate.interfazCombate;
    if (timeline == null)
    {
      timeline = GetComponent<TimelineTurnos>();
    }
    if (timeline == null)
    {
      timeline = FindObjectOfType<TimelineTurnos>();
    }
    if (timeline == null)
    {
      timeline = gameObject.AddComponent<TimelineTurnos>();
    }
    //personaje1.Ataque1();
    CrearCombate();
  }

  #region  ControlCombate

  public static ControlCombate controlCombate;
  void Awake()
    {
      controlCombate=this;
    }

  //todo: Listas organizadas de los personajes
  public List<Personaje> TodosPersonajes;

  public List<PosicionPersonaje> PersonajesOrdenados = new List<PosicionPersonaje>();
  public List<Personaje> equipoA = new List<Personaje>();
  public List<Personaje> equipoB = new List<Personaje>();

  public List<Personaje> principalesA = new List<Personaje>();
  public List<Personaje> principalesB = new List<Personaje>();

  //todo: Iniciar el combate
  public SelectorPersonaje selectorPersonaje;

  //banda superior con el orden de turnos (se busca solo si no se asigna)
  public TimelineTurnos timeline;


  



private int indiceEquipoA=0;
private int indiceEquipoB=0;

  void AsignarEquipo()
  {
    foreach (Personaje personaje in TodosPersonajes)
    {
      if (personaje.equipo == Equipo.equipoA)
      {
        principalesA.Add(personaje);
        equipoA.Add(personaje);

        Transform ubicacionActual=posicionesA[indiceEquipoA].transform;
        personaje.transform.position=ubicacionActual.position;
        personaje.SetUbicacion(ubicacionActual);

        Transform ubiSec= ubicacionActual.GetComponentInChildren<Transform>();
        personaje.SetUbicacionAtacante(ubiSec);
        


      indiceEquipoA++;
  
      }
      
      else
      {
        principalesB.Add(personaje);
        equipoB.Add(personaje);

        Transform ubicacionActual=posicionesB[indiceEquipoB].transform;
        personaje.transform.position=ubicacionActual.position;
        personaje.SetUbicacion(ubicacionActual);

        Transform ubiSec= ubicacionActual.GetComponentInChildren<Transform>();
        personaje.SetUbicacionAtacante(ubiSec);


      indiceEquipoB++;
      }

      personaje.selector = selectorPersonaje;
      personaje.controlCombate = this;
    }
  }

  //todo: Posicion y orden

  List<int> ObtenerPosicionInicial()
  {
    List<int> posicionesIniciales = new List<int>();
    for (int i = 0; i < TodosPersonajes.Count; i++)
    {
      posicionesIniciales.Add(UnityEngine.Random.Range(1, 101));
    }
    return posicionesIniciales;
  }


  void CrearPosicionEnPersonajes()
  {
     List<int> posicionesIniciales = ObtenerPosicionInicial();
     PersonajesOrdenados.Clear();
   
    //recorremos los personajes
    for (int i = 0; i < TodosPersonajes.Count; i++)
    {
   
    

      
      Personaje personaje= TodosPersonajes[i];
      int posicionInicial=posicionesIniciales[i];
      int velocidadPersonaje=TodosPersonajes[i].speed;
      int posicion=0;


      do
      {
         posicion= posicionInicial+velocidadPersonaje;
        PosicionPersonaje personajePosicion = new PosicionPersonaje(personaje,posicionInicial,posicion);
         PersonajesOrdenados.Add(personajePosicion);

        posicionInicial-=100;
        
      } while(posicion>100);

     

    }


  }


  void OrdenarPersonajes()
  {

    //  ORDENAR de mayor a menor
    PersonajesOrdenados.Sort((a, b) => b.posicion.CompareTo(a.posicion));

      foreach( PosicionPersonaje posicionPersonaje in PersonajesOrdenados){
       
        

         print( posicionPersonaje.personaje.nombre+" posicion " +string.Join(", ",posicionPersonaje.posicion) );


    }
  }






  //todo: Gestionar Combate

  void CrearCombate()
  {

    TodosPersonajes = new List<Personaje>(GetComponentsInChildren<Personaje>());
    AsignarEquipo();


    StartCoroutine(EjecutarCombate());


  }


  public int numeroRonda = 1;
  private IEnumerator EjecutarCombate()

  {

    while (numeroRonda < 3)

    {
     
      CrearPosicionEnPersonajes();
      OrdenarPersonajes();
      if (timeline != null)
      {
        timeline.MostrarOrden(PersonajesOrdenados);
      }
      Debug.Log("Ronda " + numeroRonda + " comenzada");

      yield return new WaitForSeconds(2f);
      yield return EjecutarRonda();
      numeroRonda++;
    }

    Debug.Log("combate terminado");
  }


  private IEnumerator EjecutarRonda()
  {

    while (PersonajesOrdenados.Count > 0)
    {
      Personaje PersonajeActual = PersonajesOrdenados[0].personaje;
      Debug.Log("Turno de: " + PersonajeActual.nombre + "\n" + "vida: " + PersonajeActual.vida + "\n" + "posicion: "
      + PersonajesOrdenados[0].posicion + "\n");



      //EjecutarTurno(actual);
      yield return EjecutarTurno(PersonajeActual);
      // después del turno, reordenas por si algo ha cambiado
      //Reordenar();
      PersonajesOrdenados.RemoveAt(0);
      if (timeline != null)
      {
        timeline.AvanzarTurno();
      }
    }
  }

  private InterfazCombate interfazCombate;

  private IEnumerator EjecutarTurno(Personaje p)
  {


    interfazCombate.MostrarMenuPersonaje(p);

    p.haTerminadoElAtaque = false;

    yield return new WaitUntil(() => p.haTerminadoElAtaque == true);

    p.haTerminadoElAtaque = false;
    /*
     else
     {
         IA.Ejecutar(p);
     }*/

     EjecutarEfectosGenerales();
  }



  private IEnumerator EjecutarEfectosGenerales()
  {


       foreach (Personaje personaje in TodosPersonajes)
    {
      EfectosTurnoGeneral(personaje);
    }

     yield return new WaitForSeconds(2f);
    


  }


        public void EfectosTurnoGeneral(Personaje personaje)
    {
        foreach (Efecto efecto in personaje.efectosActivos)
        {
            efecto.EnTurnoGeneral(personaje);
        }
    }



  #endregion


  #region  combate



  //**FUNCIONES DE CONTROLAR ATAQUES

  //Obtener los objetivos de un GolpeData 
  //Si es area, se le añaden los objetivos correspondientes
  List<Personaje> ObtenerObjetivos(Personaje atacante, GolpeData golpe)
  {
    switch (golpe.tipoObjetivo)
    {
      case TipoObjetivo.AreaTodos:
        return new List<Personaje>(TodosPersonajes);
        

      case TipoObjetivo.Unitario:
        return golpe.objetivos;

        case TipoObjetivo.AreaEnemigos:
        if (atacante.equipo == Equipo.equipoA)
        {
           return new List<Personaje>(equipoB);
        }
        else
        {
          return new List<Personaje>(equipoA);
        }

      default:
        return new List<Personaje>();
    }
  }


  //Ejecutar un ataque realizado.

  //Recorres los golpes, por cada golpe mandas un golpe a cada personaje que lo iba a recibir
  //Mientras recopilas la informacion que has hecho en Golpe resultado, y todos esos los recopidas
  //En un ataqueresultado por cada personaje dañado. Para luego mandarle esta informacion + decirles
  //que han recibido un 

  public void EmpezarAtaque(Personaje atacante, AtaqueData ataque)
  {
    StartCoroutine(EjecutarAtaque(atacante, ataque));
  }

  public IEnumerator EjecutarAtaque(Personaje atacante, AtaqueData ataque)
  {
    Dictionary<Personaje, ResultadoAtaque> resultados = new Dictionary<Personaje, ResultadoAtaque>();

    //empezar movimiento de camara: encuadra al atacante y sus objetivos
    if (CamaraCombate.camaraCombate != null)
    {
      List<Personaje> objetivosCamara = new List<Personaje>();
      foreach (GolpeData golpeCamara in ataque.golpes)
      {
        foreach (Personaje p in ObtenerObjetivos(atacante, golpeCamara))
        {
          if (p != null && !objetivosCamara.Contains(p))
          {
            objetivosCamara.Add(p);
          }
        }
      }
      CamaraCombate.camaraCombate.moviminetoAtaque(atacante, objetivosCamara);
    }

    //Si ataca a melee, se desplaza hacia el objetivo
    if(ataque.seDesplaza)
    {
      yield return atacante.DesplazarseAObjetivo(ataque.meleeObjetivo);
    }
    

    foreach (GolpeData golpe in ataque.golpes)
    {

      List<Personaje> objetivosFinales = ObtenerObjetivos(atacante, golpe);


      // se parara aqui hasta que haga la animacion completa de ese golpe especifico
      
      yield return atacante.AnimarAtaque(golpe, objetivosFinales);

      if (golpe.estadoGolpe == EstadoGolpe.Fallado)
      {
        Debug.Log("Golpe fallado vaya :(");

        ResultadoGolpe evasion= new ResultadoGolpe(atacante, 0, 0,EstadoGolpe.Fallado,TipoObjetivo.Nulo,TipoAtaque.Nulo);


        GestorUICombate.gestorUICombate.MostrarGolpe(evasion);
        
      }

      else
      {
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
        if (res.estadoGolpe == EstadoGolpe.Normal || res.estadoGolpe == EstadoGolpe.Critico)
        {
          resultados[objetivo].dañoTotal += res.dañoFinal;
          resultados[objetivo].armaduraTotal += res.armaduraReducida;
        }

        //?AQUI PROGRAMARAS CON "RES", que es el resultado golpe, que se vea en pantalla "golpeado con tal"
        GestorUICombate.gestorUICombate.MostrarGolpe(res);
       
      }

      if (CamaraCombate.camaraCombate != null)
      {
        CamaraCombate.camaraCombate.movimientoGolpe();
      }
      }
      
      //espera tras desencadenar un golpe
      yield return new WaitForSeconds(2f);
    }


    //Por ahora aqui: el personaje vuelve a su posicion si se ha acercado
    if(ataque.seDesplaza)
    {
      yield return atacante.DesplazarseAOrigen();
    }


    //camara: vuelve a su sitio mientras se muestran las reacciones al golpe
    if (CamaraCombate.camaraCombate != null)
    {
      CamaraCombate.camaraCombate.restaurarOriginal();
    }

    // Avisar que ha recibido un ataque, a las personas afectadas (mas pasar la info)
    foreach (var kvp in resultados)
    {
      kvp.Key.AtaqueRecibido(kvp.Value);
      //yield return new WaitForSeconds(2f);
    }
    atacante.TerminarAtaque(ataque);

  }





  #endregion
}




