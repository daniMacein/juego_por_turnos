
public class PosicionPersonaje
{

    public Personaje personaje;


    private int posicionInicial;
    public int posicion;



    public void SetPosicionInicial(int posicionInicial)
    {
        this.posicionInicial = posicionInicial;
    }



    public void CalcularPosicion()
    {
        posicion = posicionInicial + personaje.speed;
    }



    public PosicionPersonaje(Personaje personaje, int posicionInicial, int posicion)
    {
        this.personaje = personaje;

        SetPosicionInicial(posicionInicial);
        this.posicion = posicion;
    }
}