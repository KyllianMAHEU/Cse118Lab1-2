using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{
    public Material Lmaterial;
    private GameObject GameObj;
    private GameObject currGameObject;
    private LineRenderer currLine;

    private int numClicks = 0;
    private int numGO = 0;

    public GameObject tabletGO;
    bool drawEnable = false;

    Vector3 ZGap = new Vector3(0, 0, -1f);
    private void Start()
    {
        //gameObject.GetComponent<Renderer>().material.color = Color.red;
        Setup();

    }
    private void Setup()
    {
        numClicks = 0;
        GameObj = new GameObject();
        GameObj.tag = "Draw"; //Add a tag to be able to find it later
        currLine = GameObj.AddComponent<LineRenderer>();
        currLine.transform.position = new Vector3(currLine.transform.position.x, currLine.transform.position.y, currLine.transform.position.z + ZGap.z);
        currLine.useWorldSpace = false;
        currLine.startWidth = 0.01f;
        currLine.endWidth = 0.01f;
        currLine.material = Lmaterial;
        currLine.SetPosition(0, new Vector3(0, 0, 0));
        currLine.SetPosition(1, new Vector3(0, 0, 0));
    }
    // Update is called once per frame
    void Update()
    {
        /*
        if (OVRInput.GetDown(OVRInput.RawButton.A) || Input.GetKeyDown(KeyCode.V))//When the player press the A Button
        {
            //We create a new Game Object with a LineRenderer when the player press down again the A button
            currGameObject = Instantiate(GameObj, tabletGO.transform);

            //We create a LineRenderer and add some graphics options
            currLine = currGameObject.GetComponent<LineRenderer>();
            

            //We initialize the first two position to not have a line coming to the pen
            currLine.SetPosition(0, OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch));
            currLine.SetPosition(1, OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch));
            numClicks = 2;
        }
        if (OVRInput.Get(OVRInput.RawButton.A))//When the player keep the A button press
        {
            //We add +1 to the number of point into the LineRenderer
            currLine.positionCount = currLine.positionCount + 1;

            //We create the point
            currLine.SetPosition(numClicks, OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch));
            //We actualize the counter
            numClicks++;
        }

        if (OVRInput.GetDown(OVRInput.RawButton.B) || Input.GetKeyDown(KeyCode.C))//When the player press the B Button
        {
            //Go through all the Game Obj
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Draw"))
            {
                Destroy(go); //Delete all gameObj with the tag draw
            }
            Setup();
        }
     */   
    }

    private Vector3 RotatePoint(Vector3 point,Vector3 angles)
    {
        angles = angles * -1.0f;
        angles = angles * Mathf.PI / 180;
        Debug.Log(angles);
        float[,] RMatrix = new float[3, 3];
        
        if (angles.x != 0.0f)
        {
            RMatrix[0, 0] = 1;
            RMatrix[0, 1] = 0;
            RMatrix[0, 2] = 0;
            RMatrix[1, 0] = 0;
            RMatrix[1, 1] = Mathf.Cos(angles.x);
            RMatrix[1, 2] = -Mathf.Sin(angles.x);
            RMatrix[2, 0] = 0;
            RMatrix[2, 1] = Mathf.Sin(angles.x);
            RMatrix[2, 2] = Mathf.Cos(angles.x);

            float x = RMatrix[0, 0] * point.x;
            float y = RMatrix[1, 1] * point.y + RMatrix[1, 2] * point.z;
            float z = RMatrix[2, 1] * point.y + RMatrix[2, 2] * point.z;
            point = new Vector3(x, y, z);
        }

        if (angles.y != 0.0f)
        {
            RMatrix[0, 0] = Mathf.Cos(angles.y);
            RMatrix[0, 1] = 0;
            RMatrix[0, 2] = Mathf.Sin(angles.y);
            RMatrix[1, 0] = 0;
            RMatrix[1, 1] = 1;
            RMatrix[1, 2] = 0;
            RMatrix[2, 0] = -Mathf.Sin(angles.y);
            RMatrix[2, 1] = 0;
            RMatrix[2, 2] = Mathf.Cos(angles.y);

            float x = RMatrix[0, 0] * point.x + RMatrix[0, 2] * point.z;
            float y = RMatrix[1, 1] * point.y;
            float z = RMatrix[2, 0] * point.x + RMatrix[2, 2] * point.z;
            point = new Vector3(x, y, z);
        }

        if (angles.z != 0.0f)
        {
            RMatrix[0, 0] = Mathf.Cos(angles.z);
            RMatrix[0, 1] = -Mathf.Sin(angles.z);
            RMatrix[0, 2] = 0;
            RMatrix[1, 0] = Mathf.Sin(angles.z);
            RMatrix[1, 1] = Mathf.Cos(angles.z); 
            RMatrix[1, 2] = 0;
            RMatrix[2, 0] = 0;
            RMatrix[2, 1] = 0;
            RMatrix[2, 2] = 1;

            float x = RMatrix[0, 0] * point.x + RMatrix[0, 1] * point.y;
            float y = RMatrix[1, 0] * point.x + RMatrix[1, 1] * point.y;
            float z = RMatrix[2, 2] * point.z;
            point = new Vector3(x, y, z);
        }


        return point;
    }

    private void OnCollisionStay(UnityEngine.Collision collision)
    {
       
        Vector3 contactPoint = new Vector3((gameObject.transform.position.x - tabletGO.transform.position.x), gameObject.transform.position.y - tabletGO.transform.position.y, gameObject.transform.position.z - tabletGO.transform.position.z);
        
        contactPoint = new Vector3(contactPoint.x / tabletGO.transform.localScale.x, contactPoint.y / tabletGO.transform.localScale.y, contactPoint.z );
        contactPoint = RotatePoint(contactPoint, tabletGO.transform.eulerAngles);
        
        //Debug.Log(contactPoint);
        /*Debug.Log(tabletGO.transform.eulerAngles);*/
        //Debug.Log("Pos sur tablet : " + contactPoint);
        if (OVRInput.GetDown(OVRInput.RawButton.A) || Input.GetKeyDown(KeyCode.V))//When the player press the A Button
        {
            //We create a new Game Object with a LineRenderer when the player press down again the A button
            currGameObject = Instantiate(GameObj, tabletGO.transform);

            //We create a LineRenderer and add some graphics options
            currLine = currGameObject.GetComponent<LineRenderer>();


            //We initialize the first two position to not have a line coming to the pen
            currLine.SetPosition(0, contactPoint);
            currLine.SetPosition(1, contactPoint);
            numClicks = 2;
        }
        if (OVRInput.Get(OVRInput.RawButton.A))//When the player keep the A button press
        {
            //We add +1 to the number of point into the LineRenderer
            currLine.positionCount = currLine.positionCount + 1;

            //We create the point
            currLine.SetPosition(numClicks, contactPoint);
            //We actualize the counter
            numClicks++;
        }

        if (OVRInput.GetDown(OVRInput.RawButton.B) || Input.GetKeyDown(KeyCode.C))//When the player press the B Button
        {
            //Go through all the Game Obj
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Draw"))
            {
                Destroy(go); //Delete all gameObj with the tag draw
            }
            Setup();
        }
    }
    void OnCollisionEnter(UnityEngine.Collision collision)
    {
        Debug.Log("Pos contact : " + collision.GetContact(0).point);
        Debug.Log("Pos tablet : " + tabletGO.transform.position);
        Debug.Log("Pos pen" + gameObject.transform.position);
        
        //gameObject.GetComponent<Renderer>().material.color = Color.blue;
        if (collision.gameObject.tag == "Tablet")
        {
            //tabletGO = collision.gameObject;
            Debug.Log("collision avec tablet");
            drawEnable = true;
            gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
    }

    private void OnCollisionExit(UnityEngine.Collision collision)
    {
        if (collision.gameObject.tag == "Tablet")
        {
            Debug.Log("sorti collision avec tablet");
            gameObject.GetComponent<Renderer>().material.color = Color.red;
            drawEnable = false;
        }
    }
}
