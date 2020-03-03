using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float panSpeed = 40f;
    [SerializeField]
    private float panBorderThickness = 10f;
    [SerializeField]
    private Vector2 panClampLimit = new Vector2(100, 100);
    [SerializeField]
    private float scrollSpeed = 20f;
    [SerializeField]
    private float minY = 20f;
    [SerializeField]
    private float maxY = 100f;

    private void Update()
    {
        Vector3 pos = transform.position;

        if(Input.GetKey(KeyCode.W) || Input.mousePosition.y >= (Screen.height - panBorderThickness) || Input.GetKey(KeyCode.UpArrow))
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= panBorderThickness || Input.GetKey(KeyCode.DownArrow))
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= (Screen.width - panBorderThickness) || Input.GetKey(KeyCode.RightArrow))
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= panBorderThickness || Input.GetKey(KeyCode.LeftArrow))
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * Time.deltaTime * 100;
        
        pos.x = Mathf.Clamp(pos.x, -panClampLimit.x, panClampLimit.x);
        pos.z = Mathf.Clamp(pos.z, -panClampLimit.y, panClampLimit.y / 2);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }
}
