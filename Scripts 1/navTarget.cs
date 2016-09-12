using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using SocketIO;

public class navTarget : MonoBehaviour {

    public Transform target;

    public GameObject matHolder;

    public Material newMat;
    public Material originalMat;

    //public bool freeze;
   
    public float despawnTime = 10;
    public float freezeTime = 3;
    public bool mainMonster = false;

    public Vector3 startPosition; 

    NavMeshAgent agent;

	void Start () {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        if(mainMonster)
            startPosition = target.position;
    }
	
	// Update is called once per frame
	void Update () 
    {        
        //Make sure the player is still alive
        if(target != null)
        {
            //See if the is a freeze request 
            if (target.gameObject.GetComponent<PositionIO>().bfreeze)
                Pause();
            else
                unPause();

            //Move towards the player at all times - ONLY AFTER THE PLAYER MOVES AT THE START
            if (target.position != startPosition)
            {
                agent.SetDestination(target.position);
            }

            //Check if the monster is within killing distance 
            if ((Vector3.Distance(transform.position, target.position) < 1) && agent.speed != 0)
            {
                SceneManager.LoadScene("Death");
            }

            //Slow decay until despawn 
            despawnTime -= Time.deltaTime;

            if (despawnTime < 0)
            {
                GetComponent<MonsterIO>().end();
                Destroy(gameObject);
            }
        }
	}

    /* FREEZING */
    void Pause()
    {
        matHolder.GetComponent<Renderer>().material = newMat;
        StopAnimation();
        agent.speed = 0;
    }

    void unPause()
    {
        agent.speed = 3;
        matHolder.GetComponent<Renderer>().material = originalMat;
        PlayAnimation();
    }
    /* FREEZING */

    /*Animations*/
    void StopAnimation() {gameObject.GetComponent<Animator>().enabled = false;}
    void PlayAnimation() {gameObject.GetComponent<Animator>().enabled = true;}
}
