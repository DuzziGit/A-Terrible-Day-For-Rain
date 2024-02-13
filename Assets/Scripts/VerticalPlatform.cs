using System.Collections;
using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{

    public PlatformEffector2D effector;
    public float waitTime;

    // Start is called before the first frame update
    private void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
    private void Update()
    {

        if (Input.GetKeyUp(KeyCode.K) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            waitTime = 0f;
        }


        if (Input.GetButtonDown("Jump"))
        {
            effector.rotationalOffset = 0;
        }

        if (Input.GetKeyUp(KeyCode.K) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            if (waitTime <= 0)
            {
                effector.rotationalOffset = 180f;
                _ = StartCoroutine(SmallDelay());
                waitTime = 0.05f;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    private IEnumerator SmallDelay()
    {

        yield return new WaitForSeconds(0.5f);
        effector.rotationalOffset = 0;
    }

}


