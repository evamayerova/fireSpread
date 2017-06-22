using UnityEngine;
using System.Collections;

public class FirePropagator : MonoBehaviour {

    SphereCollider sc;
    GameManager gm;
	// Use this for initialization
	void Start () {
        Debug.Log("started");
        sc = gameObject.GetComponent<SphereCollider>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
        sc.radius += 0.05f;
        Transform t = gameObject.GetComponent<Transform>();
        t.position += gm.windSpeed * Vector3.forward;
        // Debug.Log(gm.windDirection);
        // GameObject.Find("Sphere").transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
        // transform.position += new Vector3(0.1f, 0.0f, 0.0f);
	}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger");
        Debug.Log(other);
        Flammable flamable = other.GetComponent<Flammable>();
        flamable.burn();
    }
}
