using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using System.Collections;
using System;
using System.Runtime.ConstrainedExecution;
public abstract class Personaje : MonoBehaviour
{



    #region Interfaz

    public string nombre;
    public string clase;
    public string descripcion;

    public AtaqueInfo[] ataquesInfo = new AtaqueInfo[4];

    [Header("Icono para el timeline de turnos (asignar en el inspector)")]
    public Sprite icono;

    protected BarraVida barraVida;

    #endregion



    #region Estadisticas

    public bool estaVivo = true;
    public float vidaMaxima { get; protected set; }
    public float vida { get; protected set; } //vida actual
    public float porcentajeVida { get; protected set; } //vida actual en porcentaje
    public float armadura { get; protected set; }
    public int speed { get; protected set; } //velocidad
    public List<int> posicion { get; protected set; } = new List<int>(); //posicion en la ronda actual 

    //*------------------------------------------
    public int probCritico { get; protected set; } //prob de que sea critico el ataque
    public int probEvasion { get; protected set; } //prob de evadir el ataque
    public int probGolpe { get; protected set; } //probabilidad de asestar el ataque
                                                 //*------------------------------------------
    public float potencia { get; protected set; }
    public float penetracionArmadura { get; protected set; } = 1; // Ignora parte de la armadura
    public float alteracionDaño { get; protected set; } //porcentaje que implica cuanto por ciento te entra de mas o de menos en el daño.

    #endregion


    #region Variables


    //Referencias
    protected Animator animator;
    public SelectorPersonaje selector;
    public ControlCombate controlCombate;
    private ControlPersonajeCombate controlPersonajeCombate;


    //Equipo
    public Equipo equipo;
    public Equipo equipoEnemigo;


    //Cosas de ataques
    public AtaqueData ataqueElegido;
    public bool haTerminadoElAtaque;


    //ubicacion
    public Transform UbicacionPropia { get; protected set; }
    public Transform UbicacionAtacante { get; protected set; }

    //efectos
    public List<Efecto> efectosActivos = new List<Efecto>();


    #endregion


    #region Start


    void Start()
    {
        controlPersonajeCombate = GetComponent<ControlPersonajeCombate>();

        if (equipo == Equipo.equipoA)
        {
            equipoEnemigo = Equipo.equipoB;
        }
        else
        {
            equipoEnemigo = Equipo.equipoA;
        }

        animator = GetComponent<Animator>();
        porcentajeVida = (vida / vidaMaxima) * 100;
        barraVida = GetComponentInChildren<BarraVida>();
    }

    #endregion


    #region Ataques

    public abstract void Ataque1();

    public abstract void Ataque2();
    public abstract void Ataque3();
    public abstract void Ataque4();


    //corrutina de selección de objetivos en marcha (para poder cancelarla)
    private Coroutine esperaActiva;
    private int ataqueEnCurso = -1;



    //todo: Metodos generales para lanzar ataques
    //Lanza la espera de objetivos de un ataque.
    //Si ya hay un ataque esperando: el mismo boton lo cancela, otro boton lo sustituye.
    protected void LanzarAtaque(int indiceAtaque, SelectorData datos, Action<List<Personaje>> alCompletar)
    {
        if (esperaActiva != null)
        {
            //mismo boton pulsado otra vez: cancelar
            if (ataqueEnCurso == indiceAtaque)
            {
                CancelarEspera();
                return;
            }

            //otro ataque: el anterior deja de esperarse
            CancelarEspera();
        }

        ataqueEnCurso = indiceAtaque;
        esperaActiva = StartCoroutine(EsperarObjetivos(datos, alCompletar));
    }

    //Cancela la seleccion en marcha (tambien valdra para un futuro boton de cancelar)
    public void CancelarEspera()
    {

        if (esperaActiva != null)
        {
            StopCoroutine(esperaActiva);
            esperaActiva = null;
        }

        ataqueEnCurso = -1;
        selector.Reset();
        Debug.Log("Selección cancelada");
    }


    //Espera hasta que el selector tenga todos los objetivos que pedian los datos
    //y entonces crea el ataque con ellos
    IEnumerator EsperarObjetivos(SelectorData datos, Action<List<Personaje>> alCompletar)
    {
        // activar modo selección UI
        selector.EmpezarSeleccion(datos);
        Debug.Log("Selecciona " + datos.numeroObjetivos + " objetivo(s)...");

        yield return new WaitUntil(() => selector.seleccionCompleta);

        //copiar los objetivos antes de resetear el selector
        List<Personaje> objetivos = new List<Personaje>(selector.objetivos);

        //la espera ha terminado bien: liberamos el estado antes de crear el ataque
        esperaActiva = null;
        ataqueEnCurso = -1;

        selector.Reset();
        InterfazCombate.interfazCombate.ResetBotonesPersonaje();

        alCompletar(objetivos);
    }


    #endregion





    #region Animacion y ubi


    //TODO: ANIMACION DE INFLIGIR
    public virtual IEnumerator AnimarAtaque(GolpeData golpeData, List<Personaje> objetivosFinales)

    {
        //aqui miras cual es el golpe, porque tendra un identificador dentro de golpe y haces la animacion
        //correspondiente

        yield return new WaitForSeconds(1f);



    }

    //TODO: ANIM DESPLAZAMIENTO

    public virtual IEnumerator DesplazarseAObjetivo(Personaje personaje)
    {
        //yield WaitForSeconds(1f);
        animator.SetTrigger("Salto");

        yield return new WaitForSeconds(0.25f);

        yield return controlPersonajeCombate.MoverseAobjetivo(personaje.UbicacionAtacante, personaje.UbicacionPropia);
        animator.SetTrigger("Aterrizar");
        yield return new WaitForSeconds(1f);
        //yield return  controlPersonajeCombate.MoverseAobjetivo(personaje.UbicacionPropia,personaje.UbicacionAtacante);
    }

    public virtual IEnumerator DesplazarseAOrigen()
    {

        yield return controlPersonajeCombate.VolverAOrigen();
    }

    //TODO: ANIMACION DE RECIBIR 

    protected virtual void AnimacionRecibirGolpe(ResultadoGolpe resultadoGolpe)
    {

        if (resultadoGolpe.tipoAtaque == TipoAtaque.Daño)
        {
            animator.SetTrigger("RecibirDano");
            //Debug.Log("ATACADOOOOOOOOOOOOOOOOOOOOO");
            Debug.Log(nombre + " golpeado con " + resultadoGolpe.dañoFinal.ToString("F0") + "p de vida");
        }
    }


     //TODO: ANIMACION DE RECIBIR 

     public virtual IEnumerator AnimarMuerte()
    {
          animator.SetTrigger("Morir");
         yield return new WaitForSeconds(2f);

    }


    #endregion





    #region gets/set

    public void SetPosicion(List<int> posiciones)
    {
        posicion.Clear();
        for (int i = 0; i < posiciones.Count; i++)

        {
            this.posicion.Add(posiciones[i]);
        }
    }

    void SetVida(float vidaRecibida)
    {

        if ((vida + vidaRecibida) > vidaMaxima)

        {
            vida = vidaMaxima;
        }
        else if ((vida + vidaRecibida) <= 0)
        {
            vida = 0;
        }
        else
        {
            vida += vidaRecibida;
        }

        porcentajeVida = (vida / vidaMaxima) * 100;
        barraVida.ModificarBarraVida();

    }

    public void SetUbicacion(Transform ubi)
    {
        this.UbicacionPropia = ubi;
    }

    public void SetUbicacionAtacante(Transform ubi)
    {
        this.UbicacionAtacante = ubi;
    }



    #endregion
    #region Muerte

    public bool EstaVivo()
    {

        if (vida <= 0)
        {
            estaVivo = false;
            //HARÁ TODO LO QUE TENGA QUE HACER
            Debug.Log("ME HE MUERTO " + nombre);
            return false;

        }
        Debug.Log("Sigo vivo" + nombre);
        return true;


    }


    #endregion
    #region Formulas
    //*formulas

    //todo: Formulas de utilidad
    //Formulas que sirve para calcular cosas de forma mas cómoda


    //Le metes un numero y te devuelve true o false. 
    //Si le metes 50, tendra un 50% de devolverte true. Un 100% siempre te dara true. 0 siempre false
    public bool ProbabilidadAcertada(int porcentaje)
    {
        porcentaje = Mathf.Clamp(porcentaje, 0, 100);

        float prob = porcentaje / 100f;


        return UnityEngine.Random.value < prob;
    }

    //todo: Formulas de estadisticas

    private float DañoReducidoPorArmadura(float daño, float penetracion)
    {
        penetracion = 1 - penetracion;
        return daño * (100 / (100 + (armadura * penetracion)));
    }

    private float CalcularCritico()
    {
        float excesoCritico = 0f;
        if (probCritico > 100)
        {
            excesoCritico = probCritico - 100;
            excesoCritico = excesoCritico * 0.5f;

        }
        int rango = (int)excesoCritico + 150;
        if (rango > 200)
        {
            rango = 200;
        }
        float critico = UnityEngine.Random.Range(rango, 210) / 100f;
        return critico;

    }


    private float DesgasteArmadura(float dañoArmadura)
    {
        float armaduraAntigua = armadura;
        armadura = armadura - ((dañoArmadura * armadura / 50) / 100);

        if (armadura < 0)
        {
            armadura = 0;
        }
        return armaduraAntigua - armadura;
    }

    #endregion
    #region Infligir ataques

    //todo: Metodos para infligir 
    //(utilizados para infligir ataques y efectos)


    // Aplicar a tu daño todo lo que hay que aplicar
    //**1) Metodo que se le aplica a cada golpe
    public void AplicarEstadisticasAGolpe(GolpeData golpe)
    {

        // 1. Comprobar si acierta
        if (ProbabilidadAcertada(probGolpe))
        {
            golpe.vida = golpe.vida * potencia;
            golpe.armadura = golpe.armadura * potencia;

            if (probGolpe > 100)
            {
                golpe.probGolpeExtra = probGolpe - 100;
            }

            // 2. Comprobar si hay crítico
            if (ProbabilidadAcertada(probCritico))
            {
                float critico = CalcularCritico();
                golpe.vida = golpe.vida * critico;

                if (golpe.esPositivo)
                {
                    golpe.tipoAtaque = TipoAtaque.Curacion;
                    golpe.estadoGolpe = EstadoGolpe.Critico;
                }
                else
                {
                    golpe.armadura = golpe.armadura * critico;
                    golpe.tipoAtaque = TipoAtaque.Daño;
                    golpe.estadoGolpe = EstadoGolpe.Critico;
                }
            }
        }
        else
        {
            // 3. Si falla
            golpe.vida = 0;
            golpe.estadoGolpe = EstadoGolpe.Fallado;
        }
    }

    //**2) acabar ataque
    //Indico que he terminado un ataque
    public virtual void TerminarAtaque(AtaqueData ataqueData)
    {
        haTerminadoElAtaque = true;
    }





    #endregion
    #region Recibir ataques


    //todo: METODO GENERAL DE RECIBIR GOLPE

    public ResultadoGolpe RecibirGolpe(GolpeData golpe)
    {
        // recuerda: daño es negativo

        //** Declaracion de variables
        float vidaRecibida = 0;
        float armaduraGastada = 0;
        bool golpeAcertado = true;
        //estado propio de ESTE objetivo: no se escribe en el golpe compartido,
        //o el primer objetivo que evada contaminaría el resultado de los demás
        EstadoGolpe estadoFinal = golpe.estadoGolpe;

        //** Si es daño:
        if (golpe.esPositivo == false)
        {

            if (ProbabilidadAcertada(probEvasion - golpe.probGolpeExtra))
            {
                estadoFinal = EstadoGolpe.Evadido;
                golpeAcertado = false;
            }

            else
            {
                vidaRecibida = RecibirDaño(golpe.vida, golpe.penetracionArmadura);
                SetVida(vidaRecibida);
                armaduraGastada = GastarArmadura(golpe.armadura);
            }

        }
        else
        //** Si es Curacion:
        {
            SetVida(golpe.vida);
            vidaRecibida = golpe.vida;
        }

        //** aplicar los efectos
        if (golpe.aplicaEfecto && golpeAcertado)
        {
            foreach (Efecto efecto in golpe.efectos)
            {
                AplicarEfecto(efecto);
            }

        }

        //** Recopilar el resultado
        ResultadoGolpe resultado = new ResultadoGolpe
        (this, vidaRecibida, armaduraGastada, estadoFinal, golpe.tipoObjetivo, golpe.tipoAtaque);
        AnimacionRecibirGolpe(resultado);

        return resultado;

    }

    //todo: METODOS QUE COMPLEMENTAN RECIBIR
    //aplicar todo lo que hay que aplicar al recibir daño
    private float RecibirDaño(float daño, float penetracion)

    {

        float nuevoDaño = daño * alteracionDaño;

        nuevoDaño = DañoReducidoPorArmadura(nuevoDaño, penetracion);

        return nuevoDaño;
    }


    //** armadura Rota
    private float GastarArmadura(float desgaste)
    {
        return DesgasteArmadura(desgaste * alteracionDaño);
    }



    //todo: POSTATAQUE

    //Despues de haber recibido un ataque completo. Se indica que el ataque ha sido finalizado/ejecutado
    //y se consigue cuantos daño y armadura total ha sido quitada.
    public virtual void AtaqueRecibido(ResultadoAtaque resultadoAtaque)

    {

        Debug.Log(nombre + ": vida actual: " + vida.ToString("F0") + "/" + vidaMaxima + "\n" + "armadura:" + armadura.ToString("F0"));

    }







    #endregion
    #region Efectos


    //todo: EFECTOS



    public ResultadoGolpe RecibirEfecto(EfectoData efecto)
    {
        // recuerda: daño es negativo
        float vidaRecibida = 0;
        float armaduraGastada = 0;
        //si es daño
        if (efecto.esPositivo == false)
        {

            vidaRecibida = RecibirDaño(efecto.vida, efecto.penetracionArmadura);
            SetVida(vidaRecibida);
            armaduraGastada = GastarArmadura(efecto.armadura);

        }
        else
        {
            SetVida(efecto.vida);
            vidaRecibida = efecto.vida;
        }

        ResultadoGolpe resultado = new ResultadoGolpe
        (this, vidaRecibida, armaduraGastada, efecto.estadoGolpe, efecto.tipoObjetivo, efecto.tipoAtaque);
        AnimacionRecibirGolpe(resultado);

        return resultado;

    }



    public void AplicarEfecto(Efecto efecto)
    {
        efectosActivos.Add(efecto);
        //efecto.AlAplicarse(this, potencia, critico);
    }


    public void EfectosInicioDeRonda()
    {
        foreach (Efecto efecto in efectosActivos)
        {
            efecto.InicioDeRonda(this);
        }
    }



    public void LimpiarEfectos()
    {
        for (int i = efectosActivos.Count - 1; i >= 0; i--)
        {
            if (efectosActivos[i].disipado)
            {
                efectosActivos.RemoveAt(i);
            }
        }
    }


    #endregion

    //! tostring
    public override string ToString()
    {
        string resultado = "";
        resultado += "Personaje" + "\n";
        resultado += "nombre: " + nombre + "\n";
        resultado += "vida actual: " + vida + "\n";
        resultado += "armadura: " + armadura + "\n";

        resultado += "equipo: " + equipo + "\n";
        resultado += "posicion: " + string.Join(", ", posicion) + "\n";


        return resultado;
    }
}

