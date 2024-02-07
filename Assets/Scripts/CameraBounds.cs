using UnityEngine;
//1
[RequireComponent(typeof(Camera))]
public class CameraBounds : MonoBehaviour
{
    //2
    public float minVisibleX;
    public float maxVisibleX;


    private float minValue;
    private float maxValue;

    public float minValueY;
    public float maxValueY;


    public float minValueX;
    public float maxValueX;


    public float cameraHalfWidth;
    //3
    private Camera activeCamera;
    //4
    public Transform cameraRoot;

    public Transform leftBounds;
    public Transform rightBounds;
    //5
    void Start()
    {

        activeCamera = Camera.main;
        //7
        cameraHalfWidth = Mathf.Abs(activeCamera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x - activeCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x) * 0.5f;
        minValue = minVisibleX + cameraHalfWidth;
        maxValue = maxVisibleX - cameraHalfWidth;



        Vector3 position;
        position = leftBounds.transform.localPosition;
        position.x = transform.localPosition.x - cameraHalfWidth;
        leftBounds.transform.localPosition = position;
        position = rightBounds.transform.localPosition;
        position.x = transform.localPosition.x + cameraHalfWidth;
        rightBounds.transform.localPosition = position;

    }
    //8
    public void SetXPosition(float x)
    {



        Vector3 trans = cameraRoot.position;
        trans.x = Mathf.Clamp(x, minValue, maxValue);
        Debug.Log("Trans Position: " + trans);
        cameraRoot.position = trans;


        var newPosition = Vector2.Lerp(transform.position, GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position, Time.deltaTime * 1);
        var camPosition = new Vector3(newPosition.x, newPosition.y, -10f);
        var v3 = camPosition;
        var clampX = Mathf.Clamp(v3.x, minValueX, minValueX);
        var clampY = Mathf.Clamp(v3.y, minValueY, maxValueY);
        transform.position = new Vector3(clampX, clampY, -10f);
    }
}