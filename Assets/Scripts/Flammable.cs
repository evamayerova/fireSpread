using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flammable : MonoBehaviour {
    
    public float sphereCastRadius;
    private bool spread;
    private bool burned;
    private float startBurnTime;

    private GameManager gm;

    // Use this for initialization
    void Start() {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        burned = false;
        spread = false;
        // for (int i = 0; i < gm.fireSampleNr; i++)
        // {
        //     Debug.Log("sddfjlsdjflsdjf");
        //     Vector3 start = transform.position;
        //     Vector3 end = gm.fireRays[i].normalized;
        // }
    }

    // Update is called once per frame
    void FixedUpdate() {
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
        startBurnTime = Time.realtimeSinceStartup;
        spread = true;
    }

    public IEnumerator burn()
    {
        int avoidMask = LayerMask.NameToLayer("Terrain");
        yield return new WaitForSeconds(0.8f);

        for (int i = 0; i < gm.fireSampleNr; i++)
        {
            Vector3 start = transform.position;
            Vector3 dir = gm.fireRays[i];
            Debug.DrawLine(start, start + dir * Vector3.Dot(dir, gm.windDirection) * gm.windSpeed, Color.red, 15, false);

            RaycastHit[] hits;
            hits = Physics.SphereCastAll(new Ray(start, dir), sphereCastRadius, Vector3.Dot(dir, gm.windDirection) * gm.windSpeed, avoidMask);
            Debug.Log(hits.Length);

            for (int hitIdx = 0; hitIdx < hits.Length; hitIdx ++)
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
