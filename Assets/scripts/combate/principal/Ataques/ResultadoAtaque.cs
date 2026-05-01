using System.Collections.Generic;

public class ResultadoAtaque 
{
    public float dañoTotal;
    public float armaduraTotal;

     public Personaje objetivo;

    public List<ResultadoGolpe> golpes = new List<ResultadoGolpe>();




//! Esto simplemente es el Tostring, NO HACE NADAS
    public override string ToString()
{
    string resultado = "";
     resultado += "AtaqueData"+ "\n";
    resultado += "Objetivo: " + objetivo.nombre + "\n";
    resultado += "Daño total: " + dañoTotal + "\n";
    resultado += "Armadura reducida: " + armaduraTotal + "\n";
    resultado += "Número de golpes: " + golpes.Count + "\n";

    resultado += "Detalle de golpes:\n";

    for (int i = 0; i < golpes.Count; i++)
    {
        ResultadoGolpe g = golpes[i];

        resultado += "- Golpe " + i + ": ";
        resultado += "objetivo=" + g.tipoObjetivo + ", ";
        //resultado += "tipo=" + g.tipoataque + ", ";
        resultado += "Daño=" + g.dañoFinal + ", ";
        resultado += "Armadura=" + g.armaduraReducida + "\n";
    }

    return resultado;
}
}
