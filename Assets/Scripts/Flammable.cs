using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flammable : MonoBehaviour {
    
    public float sphereCastRadius;
    private bool spread;
    private bool burned;

    private GameManager gm;

    // Use this for initialization
    void Start() {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        burned = false;
        spread = false;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (gm.pause)
            return;

        if (spread)
        {
            spread = false;
            StartCoroutine(burn());
        }
    }

    IEnumerator WaitToEndOfUpdate()
    {
        yield return new WaitForFixedUpdate();
    }


    IEnumerator WaitToRayCast(int time)
    {
        yield return new WaitForSeconds(time);
    }

    public void fire()
    {
        if (burned)
            return;

        gameObject.GetComponent<Renderer>().material.color = Color.red;
        gameObject.layer = LayerMask.NameToLayer("Burned");
        spread = true;
    }

    public IEnumerator burn()
    {
        int avoidMask = LayerMask.NameToLayer("Terrain");
       
        yield return new WaitForSeconds(gm.simulationSpeed);

        // wait if game is paused
        while (gm.pause)
            yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < gm.fireSampleNr; i++)
        {
            Vector3 start = transform.position;
            Vector3 dir = gm.fireRays[i];
            Debug.DrawLine(start, start + dir * Vector3.Dot(dir, gm.windDirection) * gm.windSpeed, Color.red, 15, false);


            RaycastHit[] hits;
            // dot product of the spread vector and wind direction determines the length of single casted rays
            hits = Physics.SphereCastAll(new Ray(start, dir), sphereCastRadius, Vector3.Dot(dir, gm.windDirection) * gm.windSpeed, avoidMask);
            //Debug.Log(hits.Length);

            for (int hitIdx = 0; hitIdx < hits.Length; hitIdx++)
            {
                RaycastHit hit = hits[hitIdx];
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Flammable"))
                {
                    hit.transform.gameObject.GetComponent<Flammable>().fire();

                }
            }
            burnOut();
        }
    }

    public void colorObj()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.blue;
    }

    public void burnOut()
    {
        burned = true;
        gameObject.GetComponent<Renderer>().material.color = Color.black;
    }
}
