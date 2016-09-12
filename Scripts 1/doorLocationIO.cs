using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SocketIO;

public class doorLocationIO : MonoBehaviour
{
    public int id = 0;
    private SocketIOComponent socket;

    public GameObject parent;

    // Use this for initialization
    void Start()
    {
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();

        id = parent.GetComponent<currentID>().doorid;
        parent.GetComponent<currentID>().doorid++;

        Invoke("call", 2);
    }


    private void call()
    {
        JSONObject j = new JSONObject(JSONObject.Type.OBJECT);

        j.AddField("x", transform.position.x);
        j.AddField("y", transform.position.z);
        j.AddField("id", id);

        socket.Emit("door_spawnpoints", j);
    }
}
