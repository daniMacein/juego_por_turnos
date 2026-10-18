[System.Serializable]
public class AtaqueInfo
{
    public string nombre;
    public string descripcion;

    public AtaqueInfo(string nombre, string descripcion)
    {
        this.nombre = nombre;
        this.descripcion = descripcion;
    }
}