using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using SocketIO;

public class PositionIO : MonoBehaviour {

    private SocketIOComponent socket;
    public GameObject obj;

    public bool bfreeze = false;

    public GameObject spawner;

    public GameObject[] doors;

    public GameObject end;

    private GameObject handler;

    private int currID; 

	// Use this for initialization
	void Start () {
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();

        socket.On("init", TestBoop);
        socket.On("freeze", freeze);
        socket.On("door", door);

        handler = GameObject.Find("MONSTERHANDLER");
    }

    // Update is called once per frame
    void Update () {
        call();

        if(Vector3.Distance(transform.position, end.transform.position) < 4)
        {
            SceneManager.LoadScene("Victory");   
        }
	}

    public void playScream()
    {
        gameObject.GetComponent<AudioSource>().Play();
    }

    //Send player position 
    private IEnumerator call()
    {
        JSONObject j = new JSONObject(JSONObject.Type.OBJECT);

        j.AddField("x", transform.position.x);
        j.AddField("y", transform.position.z);

        socket.Emit("new_coord", j);

        return null;
    }

    //Initial connection test
    public void TestBoop(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Boop received: " + e.name + " " + e.data);

        if (e.data == null) { return; }

        Debug.Log(
            "#####################################################" +
            "THIS: " + e.data.GetField("this").str +
            "#####################################################"
        );
    }

    //Freeze monst 
    public void freeze(SocketIOEvent e)
    {
        Debug.Log("FREEZE XD");

        bfreeze = true;

        Invoke("Unfreeze", 2);
    }

    public void Unfreeze()
    {
        bfreeze = false; 
    }

    //Open and close door 
    public void door(SocketIOEvent e)
    {
        JSONObject j = new JSONObject((e.data).ToString());
        accessData(j);
        Debug.Log("DOOR XD: " + currID);
        toggleDoor(currID);
    }

    void toggleDoor(int id)
    {
        float yVal = doors[id].GetComponent<Transform>().rotation.y;

        if (yVal == 0)
        {
            doors[id].GetComponent<Transform>().Rotate(new Vector3(0, -90, 0));
        }
        else
        {

            doors[id].GetComponent<Transform>().Rotate(new Vector3(0, 90, 0));
        }
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
                Debug.Log(obj.str);
                break;
            case JSONObject.Type.NUMBER:
                currID = (int)obj.n;
                break;
            case JSONObject.Type.BOOL:
                Debug.Log(obj.b);
                break;
            case JSONObject.Type.NULL:
                Debug.Log("NULL");
                break;

        }
    }
}
