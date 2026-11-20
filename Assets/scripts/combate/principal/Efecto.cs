

using System.Runtime.ConstrainedExecution;
using UnityEngine.InputSystem.Interactions;
using System.Collections.Generic;


public class Efecto
{
    public List<RasgoEfectoData> rasgosEfectos;
    public Personaje objetivo;

    public TipoDeDuplicado tipoDeDuplicado;
    public string id;
    bool esPositivo;
    bool esDisipable = true;
    public bool afectaEnTurnoGeneral = false;

    public bool disipado = false;
    public int duracion { get; protected set; }

    public virtual void DisiparEfecto() { }

    public virtual void LimpiarEfecto() { }

    public virtual void AlAplicarse(Personaje objetivo)
    {

        this.objetivo = objetivo;



    }



    public virtual void AlDuplicarse(Personaje objetivo, float potencia, float critico)
    {



    }
    public virtual void AntesDeImpacto(GolpeData golpe, Personaje objetivo) { }

    public virtual void InicioDeRonda(Personaje personaje) { }

    public virtual void EnTurnoPropio(Personaje personaje) { }

    public virtual void EnTurnoAliado(Personaje personaje) { }

    public virtual void EnTurnoEnemigo(Personaje personaje) { }

    public virtual EfectoData EnTurnoGeneral(Personaje personaje)
    {


        EfectoData efectoData = new EfectoData();
        foreach (RasgoEfectoData rasgo in rasgosEfectos)
        {
            if (rasgo.tiempoRasgo == TiempoRasgo.EnTurnoGeneral)
            {

                if (rasgo.stacks.Count > 0)
                {

                    rasgo.cooldownActual--;

                    if (rasgo.cooldownActual == 0)
                    {
                        efectoData.rasgos.Add(rasgo);
                        rasgo.ReducirDuracion();
                        rasgo.cooldownActual=rasgo.cooldown;

                    }




                }


            }
        }



        return null;
    }

    public virtual void TrasRecibirAtaque(Personaje personaje, ResultadoAtaque resultado) { }

    public virtual void TrasIniciarAtaque(Personaje personaje, AtaqueData ataque) { }

    public virtual void AlMorir() { }

    //efectos especiales

    public virtual void AntesDeMorir() { }


    public Efecto(params RasgoEfectoData[] rasgos)
    {
        this.rasgosEfectos = new List<RasgoEfectoData>(rasgos);


        foreach (RasgoEfectoData rasgo in rasgosEfectos)
        {
            if (rasgo.tiempoRasgo == TiempoRasgo.EnTurnoGeneral)
            {
                afectaEnTurnoGeneral = true;
            }



        }

    }


}
