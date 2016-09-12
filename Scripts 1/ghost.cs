using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJson;
using SocketIO;

public class ghost : MonoBehaviour {

    private SocketIOComponent socket;

    public Transform target;
    public GameObject mesh;
    public float speed = 4;

    private float[] xy = { 0, 0 };
    private int c = 0;

    public bool findingPlayer = false; 

    private Vector3 basePosition;

	// Update is called once per frame
    void Start()
    {
        basePosition = transform.position;

        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();

        toggleMesh(false);
        socket.On("new_ghost", grabLocation);

    }

    void Update () {
        //Once delay is up, move to a point, once that point is reached, destroy. 


        if(findingPlayer)
            goToPlayer();


        if (Vector3.Distance(transform.position, target.position) < 1)
            reset();

	}

    void toggleMesh(bool x)
    {
        mesh.GetComponent<MeshRenderer>().enabled = x;
    }


    void goToPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    void reset()
    {
        Debug.Log("Resetting Ghost");
        toggleMesh(false);
        findingPlayer = false;
        transform.position = new Vector3(basePosition.x, -20, basePosition.z); 
    }

    void grabLocation(SocketIOEvent e)
    {
        c = 0;
        Debug.Log("Grab Ghost location");
        JSONObject j = new JSONObject((e.data).ToString());

        accessData(j);

        transform.position = new Vector3(xy[0], 2, xy[1]);

        Debug.Log(xy[0] + " " + xy[1]);

        findingPlayer = true;
        target.gameObject.GetComponent<PositionIO>().playScream();
        toggleMesh(true);

    }

    void accessData(JSONObject obj)
    {
        switch (obj.type)
        {
            case JSONObject.Type.OBJECT:
                for (int i = 0; i < obj.list.Count; i++)
                {
                    string key = (string)obj.keys[i];
                    JSONObject j = (JSONObject)obj.list[i];
                    accessData(j);
                }
                break;
            case JSONObject.Type.ARRAY:
                foreach (JSONObject j in obj.list)
                {
                    accessData(j);
                }
                break;
            case JSONObject.Type.STRING:
                break;
            case JSONObject.Type.NUMBER:
                fill(obj.n);
                break;
            case JSONObject.Type.BOOL:
                Debug.Log(obj.b);
                break;
            case JSONObject.Type.NULL:
                Debug.Log("NULL");
                break;

        }
    }

    void fill(float x)
    {
        xy[c] = x;
        c++;
    }
}
