

public abstract class Efecto
{

public bool id{ get; protected set; } 
   bool esPositivo;
   bool esDisipable=true;

    public bool disipado=false;
   public int duracion;

    public virtual void DisiparEfecto() { }

        public virtual void AlAplicarse(Personaje objetivo) { }
    public virtual void AntesDeImpacto(GolpeData golpe, Personaje objetivo) { }

    public virtual void InicioDeRonda(Personaje personaje) { }

    public virtual void EnTurnoPropio(Personaje personaje) { }

    public virtual void EnTurnoAliado(Personaje personaje) { }

    public virtual void EnTurnoEnemigo(Personaje personaje) { }

    public virtual void EnTurnoGeneral(Personaje personaje) { }

    public virtual void TrasRecibirAtaque(Personaje personaje, ResultadoAtaque resultado) { }

     public virtual void TrasIniciarAtaque(Personaje personaje, AtaqueData ataque) { }

     public virtual void AlMorir(){}

}
