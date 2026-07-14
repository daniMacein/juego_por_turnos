
public class PosicionPersonaje 
{
    public int posicion;
    public Personaje personaje;


    private int posicionInicial;
    private int numeroRepeticion=0;

    public void SetPosicionInicial(int posicionInicial)
    {
        this.posicionInicial=posicionInicial;
    }


        public void SetnumeroRepeticion(int numeroRepeticion)
    {
        this.numeroRepeticion=numeroRepeticion;
    }



    public void SetPosicion()
    {
        posicion=posicionInicial+personaje.speed+(-100*numeroRepeticion);
    }

    public bool EsPosicionMayorACien()
    {
        if (posicion > 100)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public PosicionPersonaje(Personaje personaje, int posicionInicial, int numeroRepeticion )
    {
       this.personaje=personaje;

       SetPosicionInicial(posicionInicial);
       SetnumeroRepeticion(numeroRepeticion);
       SetPosicion();
    }
}