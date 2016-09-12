using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SocketIO;

public class MonsterIO : MonoBehaviour {

    private SocketIOComponent socket;
    public GameObject obj;

    public float id_float; 

	// Use this for initialization
	void Start () {
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();

        //socket.On("new_monster", getID);
    }

    // Update is called once per frame
    void Update()
    {
        call();
    }


    //Send monster's movement
    private IEnumerator call()
    {
        JSONObject j = new JSONObject(JSONObject.Type.OBJECT);

        j.AddField("x", transform.position.x);
        j.AddField("y", transform.position.z);
        
        //Debug.Log(id_float);
        j.AddField("id",(int)id_float);

        socket.Emit("new_coord_monster", j);

        return null;
    }

    public void end()
    {
        JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
        j.AddField("id", (int)id_float);

        socket.Emit("monster_death", j);
    }
}
