using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class TimeCounter : MonoBehaviour {

    public GameObject text;
    public float timer =  15;

	// Use this for initialization
	void Start () {
        Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;

        TextMesh tm = text.GetComponent<TextMesh>();
        tm.text = timer.ToString("F0");

        if (timer < 10)
        {
            tm.color = Color.red;
        }

        if(timer < 0)
        {
            SceneManager.LoadScene("TimeLose");
        }
	}
}
