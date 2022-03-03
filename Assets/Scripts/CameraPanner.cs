using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPanner : MonoBehaviour
{

    [SerializeField] private int panSpeed;

    private Camera camera;
    private int minSize = 5;
    private int maxSize = 15;

    private void Start()
    {
        camera = GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 panDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        transform.Translate(panDirection * Time.deltaTime * panSpeed);
        int zoomValue = -(int)Input.mouseScrollDelta.y;
        float zoom = camera.orthographicSize + zoomValue;
        if (zoom < minSize)
            zoom = minSize;
        if (zoom > maxSize)
            zoom = maxSize;
        camera.orthographicSize = zoom;
    }
}
