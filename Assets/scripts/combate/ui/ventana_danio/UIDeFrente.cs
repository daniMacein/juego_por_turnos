using UnityEngine;

public class UIDeFrente : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Camera cam;

    private void Awake()
    {
        cam= Camera.main;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward= cam.transform.forward;
    }
}
