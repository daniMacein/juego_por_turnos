

using System.Runtime.ConstrainedExecution;
using UnityEngine.InputSystem.Interactions;

public abstract class Efecto
{
    public Personaje objetivo;
    public bool id { get; protected set; }
    bool esPositivo;
    bool esDisipable = true;
    public bool afectaEnTurnoGeneral = false;

    public bool disipado = false;
    public int duracion { get; protected set; }

    public virtual void DisiparEfecto() { }

    public virtual void AlAplicarse(Personaje objetivo, float potencia, float critico) { }
    public virtual void AntesDeImpacto(GolpeData golpe, Personaje objetivo) { }

    public virtual void InicioDeRonda(Personaje personaje) { }

    public virtual void EnTurnoPropio(Personaje personaje) { }

    public virtual void EnTurnoAliado(Personaje personaje) { }

    public virtual void EnTurnoEnemigo(Personaje personaje) { }

    public virtual EfectoData EnTurnoGeneral(Personaje personaje)
    {
        return null;
    }

    public virtual void TrasRecibirAtaque(Personaje personaje, ResultadoAtaque resultado) { }

    public virtual void TrasIniciarAtaque(Personaje personaje, AtaqueData ataque) { }

    public virtual void AlMorir() { }

    //efectos especiales

    public virtual void AntesDeMorir() { }

}
