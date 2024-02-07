using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float camSpeed;
    public float minX;
    public float minY;
    public float maxX;
    public float maxY;
    void FixedUpdate()
    {


        var newPosition = Vector2.Lerp(transform.position, GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position, Time.deltaTime * camSpeed);
        var camPosition = new Vector3(newPosition.x, newPosition.y, -10f);
        var v3 = camPosition;



        var clampX = Mathf.Clamp(v3.x, minX, maxX);
        var clampY = Mathf.Clamp(v3.y, minY, maxY);




        transform.position = new Vector3(clampX, clampY, -10f);

    }
}
