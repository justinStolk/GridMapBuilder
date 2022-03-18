using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPanner : MonoBehaviour
{
    public bool panCamera = true;
    public float mouseSensitivity = 0.01f;

    [SerializeField] private int panSpeed;

    private Camera camera;
    private int minSize = 5;
    private int maxSize = 15;

    private Vector3 lastPosition;

    private void Start()
    {
        camera = GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!panCamera)
        {
            return;
        }
        if (Input.GetMouseButtonDown(2))
        {
            lastPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 delta = Input.mousePosition - lastPosition;
            transform.Translate(delta.x * mouseSensitivity * Time.deltaTime, delta.y * mouseSensitivity * Time.deltaTime, 0);
            lastPosition = Input.mousePosition;
        }
        //Vector2 panDirection = Vector2.zero;
        //if (Input.GetMouseButton(2))
        //{
        //    panDirection = (camera.transform.position - Input.mousePosition).normalized;
        //}
        //Vector2 panDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //transform.Translate(panDirection * Time.deltaTime * panSpeed * (camera.orthographicSize/minSize));
        int zoomValue = -(int)Input.mouseScrollDelta.y;
        float zoom = camera.orthographicSize + zoomValue;
        if (zoom < minSize)
            zoom = minSize;
        if (zoom > maxSize)
            zoom = maxSize;
        camera.orthographicSize = zoom;
    }
}
