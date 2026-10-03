using TMPro;
using UnityEngine;

public class VentanaDanoGenerador : MonoBehaviour
{
    public static VentanaDanoGenerador ventanaDanoGenerador;
    public GameObject prefab;

    private void Awake()
    {
        ventanaDanoGenerador = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            MostrarNumGolpe(Vector3.one, UnityEngine.Random.Range(0, 1000).ToString(), Color.yellow);
        }
    }

    public void MostrarNumGolpe(Vector3 posicion, string texto, Color color)
    {
        var ventana = Instantiate(prefab, posicion, Quaternion.identity);
        var temp = ventana.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        temp.text = texto;

        temp.faceColor = color;

        Destroy(ventana, 1f);
    }
}