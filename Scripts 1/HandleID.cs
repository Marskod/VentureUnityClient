using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SocketIO;

public class HandleID : MonoBehaviour {

    private SocketIOComponent socket;
    public GameObject obj;
    public Transform[] spawn;
    public int currSpawn;

    public List<GameObject> monsters;
    public float[] values;

    private int c = 0;
    private float id_float;
    private float[] xy = { 0, 0 };

    void Start()
    {
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();

        socket.On("new_monster", getID);
    }

    //Grab the current monster ID
    public void getID(SocketIOEvent e)
    {
        string decode = (e.data).ToString();
        JSONObject j = new JSONObject(decode);
        accessData(j);

        // Add monster to list
        Debug.Log("Handle: Spawn Monster at: " + currSpawn);
        monsters.Add( (GameObject) Instantiate(obj, spawn[(int)xy[0]].position, Quaternion.identity) );
        Debug.Log("ID called: " + xy[1]);
        monsters[monsters.Count - 1].GetComponent<MonsterIO>().id_float = xy[1];

        c = 0;
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
                fill(obj.n);
                Debug.Log(obj.n);
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
