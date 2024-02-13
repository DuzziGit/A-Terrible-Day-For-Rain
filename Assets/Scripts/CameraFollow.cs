using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float camSpeed;
    public float minX;
    public float minY;
    public float maxX;
    public float maxY;

    private void FixedUpdate()
    {


        Vector2 newPosition = Vector2.Lerp(transform.position, GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position, Time.deltaTime * camSpeed);
        Vector3 camPosition = new(newPosition.x, newPosition.y, -10f);
        Vector3 v3 = camPosition;



        float clampX = Mathf.Clamp(v3.x, minX, maxX);
        float clampY = Mathf.Clamp(v3.y, minY, maxY);




        transform.position = new Vector3(clampX, clampY, -10f);

    }
}
