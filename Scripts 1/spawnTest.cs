using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SocketIO;

public class spawnTest : MonoBehaviour {

    public int id = 0;
    private SocketIOComponent socket;

    public GameObject monster;
    public GameObject parent;
    public GameObject handler;

    private int currID;

	// Use this for initialization
	void Start () {
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();

        id = parent.GetComponent<currentID>().id;
        parent.GetComponent<currentID>().id++;

        Invoke("call", 2);
	}

          
    private void call()
    {
        JSONObject j = new JSONObject(JSONObject.Type.OBJECT);

        j.AddField("x", transform.position.x);
        j.AddField("y", transform.position.z);
        j.AddField("id", id);
        
        socket.Emit("monster_spawnpoints", j);
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
                    Debug.Log(key);
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
