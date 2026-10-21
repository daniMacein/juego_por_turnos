using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System.Runtime.CompilerServices;
public class ControlCombate : MonoBehaviour
{


  #region Listas de personajes

  //todo: Listas organizadas de los personajes
  public List<Personaje> TodosPersonajes;

  public List<PosicionPersonaje> PersonajesOrdenados = new List<PosicionPersonaje>();
  public List<Personaje> equipoA = new List<Personaje>();
  public List<Personaje> equipoB = new List<Personaje>();

  public List<Personaje> principalesA = new List<Personaje>();
  public List<Personaje> principalesB = new List<Personaje>();

  public List<Personaje> personajesMuertos = new List<Personaje>();


  #endregion

  #region Variables
  //todo: VARIABLES

  //Guardar las ubicaciones de cada uno de los personajes
  public Transform[] UbicacionesA;
  public Transform[] UbicacionesB;


  // variables que se utilizan para llevar a cada personaje a su ubicacion y su orientacion
  private int indiceEquipoA = 0;
  private int indiceEquipoB = 0;
  private float orientacionA = 112;
  private float orientacionB = 224;


  //Referencias a scripts
  public static ControlCombate controlCombate;
  public SelectorPersonaje selectorPersonaje;

  public TimelineTurnos timeline;
  private InterfazCombate interfazCombate;


  //cosas del combate
  public int numeroRonda = 1;
  private bool partidaFinalizada = false;



  #endregion




  #region INICIAR
  //todo: INICIAR


  void Awake()
  {
    controlCombate = this;
  }


  void Start()
  {
    interfazCombate = InterfazCombate.interfazCombate;

    if (timeline == null)
    {
      timeline = FindFirstObjectByType<TimelineTurnos>();
    }

    //personaje1.Ataque1();
    CrearCombate();
  }
  //todo: Asignaciones oportunas al empezar
  void CrearCombate()
  {

    TodosPersonajes = new List<Personaje>(GetComponentsInChildren<Personaje>());
    AsignarEquipo();

    StartCoroutine(EjecutarCombate());


  }



  void AsignarEquipo()
  {
    foreach (Personaje personaje in TodosPersonajes)
    {
      if (personaje.equipo == Equipo.equipoA)
      {
        principalesA.Add(personaje);
        equipoA.Add(personaje);

        Transform ubicacionActual = UbicacionesA[indiceEquipoA].transform;
        personaje.transform.position = ubicacionActual.position;
        personaje.SetUbicacion(ubicacionActual);

        personaje.SetUbicacionAtacante(ObtenerUbicacionCombate(ubicacionActual));

        Quaternion rotacion = Quaternion.Euler(0f, orientacionA, 0f);
        personaje.transform.rotation = rotacion;



        indiceEquipoA++;

      }

      else
      {
        principalesB.Add(personaje);
        equipoB.Add(personaje);

        Transform ubicacionActual = UbicacionesB[indiceEquipoB].transform;
        personaje.transform.position = ubicacionActual.position;
        personaje.SetUbicacion(ubicacionActual);

        personaje.SetUbicacionAtacante(ObtenerUbicacionCombate(ubicacionActual));

        Quaternion rotacionB = Quaternion.Euler(0f, orientacionB, 0f);
        personaje.transform.rotation = rotacionB;


        indiceEquipoB++;
      }

      personaje.selector = selectorPersonaje;
      personaje.controlCombate = this;
    }

    //posición inicial: cada equipo mira al otro

  }

  //Devuelve la hija "ubicación de combate" (el punto frente a la ubicación donde salta el atacante).
  //OJO: GetComponentInChildren<Transform>() NO vale aquí porque primero devuelve el propio objeto,
  //así que UbicacionAtacante quedaba apuntando a la ubicación del personaje y se saltaba encima de él.
  Transform ObtenerUbicacionCombate(Transform ubicacion)
  {
    foreach (Transform hijo in ubicacion)
    {
      return hijo;
    }
    Debug.LogError("La ubicación " + ubicacion.name + " no tiene hija de combate, se usará la propia ubicación");
    return ubicacion;
  }



  #endregion


  #region  Rondas y Turnos

  //todo:Gestion de los turnos Y Rondas

  //Se gestiona que pasa en cada ronda, cada turno etc

  //**1) EJECUCION DEL COMBATE
  private IEnumerator EjecutarCombate()

  {

    while (true)

    {

      CrearPosicionEnPersonajes();
      OrdenarPersonajes();

      Debug.Log("Ronda " + numeroRonda + " comenzada");

      yield return new WaitForSeconds(2f);
      yield return EjecutarRonda();


      if (partidaFinalizada)
      {
        break;
      }

      numeroRonda++;
    }

    Debug.Log("combate terminado");
  }

  //** 2) LA RONDA
  private IEnumerator EjecutarRonda()
  {

    while (PersonajesOrdenados.Count > 0)
    {
      Personaje PersonajeActual = PersonajesOrdenados[0].personaje;
      Debug.Log("Turno de: " + PersonajeActual.nombre + "\n" + "vida: " + PersonajeActual.vida + "\n" + "posicion: "
      + PersonajesOrdenados[0].posicion + "\n");



      //EjecutarTurno(actual);
      yield return EjecutarTurno(PersonajeActual);


      ComprobarFinPartida();
      if (partidaFinalizada)
      {
        break;
      }

    }
  }


  //** 3) CADA TURNO
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

    PersonajesOrdenados.RemoveAt(0);
    yield return ComprobarMuerte();
    OrdenarPersonajes();

  }



  #endregion
  #region Metodos gestion turnos
  //todo: METODOS PARA LA GESTION DE TURNOS




  //todo: Efectos
  private IEnumerator EjecutarEfectosGenerales()
  {


    foreach (Personaje personaje in TodosPersonajes)
    {
      yield return EfectosTurnoGeneral(personaje);
    }

    yield return new WaitForSeconds(2f);



  }


  private IEnumerator EfectosTurnoGeneral(Personaje personaje)
  {
    foreach (Efecto efecto in personaje.efectosActivos)
    {
      if (efecto.afectaEnTurnoGeneral){
        EfectoData efectoData = efecto.EnTurnoGeneral(personaje);
        
        yield return EjecutarEfecto(personaje,efectoData);
      }


    }

    yield return new WaitForSeconds(0.1f); 
  }


  private IEnumerator EjecutarEfecto(Personaje personaje, EfectoData efectoData)
  {
    ResultadoGolpe resultado = personaje.RecibirEfecto(efectoData);

    GestorUICombate.gestorUICombate.MostrarGolpe(resultado);
    yield return new WaitForSeconds(0.1f);
  }







  //todo:OBTENER LA POSICION DE LOS PERSONAJES

  //** Metodos para iniciar y crear la lista con las posiciones
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




      Personaje personaje = TodosPersonajes[i];
      int posicionInicial = posicionesIniciales[i];
      int velocidadPersonaje = TodosPersonajes[i].speed;
      int posicion = 0;


      do
      {
        posicion = posicionInicial + velocidadPersonaje;
        PosicionPersonaje personajePosicion = new PosicionPersonaje(personaje, posicionInicial, posicion);
        PersonajesOrdenados.Add(personajePosicion);

        posicionInicial -= 100;

      } while (posicion > 100);



    }


  }


  //** Metodo para reordenar la lista (ya creada)

  void OrdenarPersonajes()
  {

    //  ORDENAR de mayor a menor
    PersonajesOrdenados.Sort((a, b) => b.posicion.CompareTo(a.posicion));

    foreach (PosicionPersonaje posicionPersonaje in PersonajesOrdenados)
    {



      print(posicionPersonaje.personaje.nombre + " posicion " + string.Join(", ", posicionPersonaje.posicion));

      if (timeline != null)
      {
        timeline.MostrarOrden(PersonajesOrdenados);
      }


    }


  }


  //todo: Muerte


  private IEnumerator ComprobarMuerte()
  {

    // Se recorre una copia porque ControlMuerte elimina elementos de la lista original.
    foreach (Personaje personaje in new List<Personaje>(TodosPersonajes))
    {
      if (personaje.EstaVivo() == false)
      {
        ControlMuerte(personaje);
        
        yield return personaje.AnimarMuerte();

        personaje.gameObject.SetActive(false);
        
      }

    }

  }


  void ControlMuerte(Personaje personaje)
  {
    personajesMuertos.Add(personaje);

    // Se elimina al personaje muerto de todas las listas de combate donde esté.
    TodosPersonajes.RemoveAll(p => p == personaje);

    equipoA.RemoveAll(p => p == personaje);
    equipoB.RemoveAll(p => p == personaje);

    principalesA.RemoveAll(p => p == personaje);
    principalesB.RemoveAll(p => p == personaje);

    // PersonajesOrdenados contiene PosicionPersonaje (numero + personaje) y
    // un personaje puede aparecer varias veces: se quitan todas sus entradas.
    PersonajesOrdenados.RemoveAll(pp => pp != null && pp.personaje == personaje);
  }



  void ComprobarFinPartida()
  {

    if (equipoA.Count == 0 && equipoB.Count == 0)
    {
      Debug.Log("Se acabo la partida");
      Debug.Log("EMPATE");
      partidaFinalizada = true;
    }

    if (equipoA.Count == 0)
    {
      Debug.Log("Se acabo la partida");
      Debug.Log("Gango el equipo B");
      partidaFinalizada = true;
    }

    if (equipoB.Count == 0)
    {
      Debug.Log("Se acabo la partida");
      Debug.Log("Gango el equipo A");
      partidaFinalizada = true;
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


  //todo: METODO PRINCIAPL: EJECUTAR ATAQUE
  public IEnumerator EjecutarAtaque(Personaje atacante, AtaqueData ataque)
  {
    // Se crea el diccionario
    Dictionary<Personaje, ResultadoAtaque> resultados = new Dictionary<Personaje, ResultadoAtaque>();


    //** Movimiento de camara
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


     //** Desplazamiento a objetivo
    //Si ataca a melee, se desplaza hacia el objetivo
    if (ataque.seDesplaza)
    {
      yield return atacante.DesplazarseAObjetivo(ataque.meleeObjetivo);
    }

     //** RECORRER CADA GOLPE
    foreach (GolpeData golpe in ataque.golpes)
    {

      List<Personaje> objetivosFinales = ObtenerObjetivos(atacante, golpe);


      // se parara aqui hasta que haga la animacion completa de ese golpe especifico
      yield return atacante.AnimarAtaque(golpe, objetivosFinales);


      if (golpe.estadoGolpe == EstadoGolpe.Fallado)
      {
        Debug.Log("Golpe fallado vaya :(");

        ResultadoGolpe evasion = new ResultadoGolpe(atacante, 0, 0, EstadoGolpe.Fallado, TipoObjetivo.Nulo, TipoAtaque.Nulo);


        GestorUICombate.gestorUICombate.MostrarGolpe(evasion);

      }

      else
      {
        //** RECORRER CADA OBJETIVO DEL GOLPE
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
      //** ESPERA TRAS EJECUTAR GOLPE
      //espera tras desencadenar un golpe
      yield return new WaitForSeconds(2f);
    }
     //** SE ACABA: RECORRER CADA GOLPE


    //Por ahora aqui: el personaje vuelve a su posicion si se ha acercado
    if (ataque.seDesplaza)
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




