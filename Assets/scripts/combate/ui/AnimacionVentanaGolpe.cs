using TMPro;
using UnityEngine;

public class AnimacionVentanaGolpe : MonoBehaviour
{

    public AnimationCurve opacidadCurva;
    private TextMeshProUGUI tmp;
    private float time =0;

    public AnimationCurve escalaCurva;

        public AnimationCurve alturaCurva;


    private Vector3 origen;

    private void Awake()
    {
        tmp= transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        origen= transform.position;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tmp.color= new Color(1,1,1,opacidadCurva.Evaluate(time));
        transform.localScale = Vector3.one * escalaCurva.Evaluate(time);
        transform.position= origen + new Vector3(0,1+ alturaCurva.Evaluate(time),0);
        time+=Time.deltaTime;
    }
}
