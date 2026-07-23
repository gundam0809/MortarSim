using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;

    private bool isCamera1Active = true;

    void Start()
    {
        camera1.gameObject.SetActive(true);
        camera2.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Q))
        {
            isCamera1Active = !isCamera1Active;
            camera1.gameObject.SetActive(isCamera1Active);
            camera2.gameObject.SetActive(!isCamera1Active);
        }
    }
}