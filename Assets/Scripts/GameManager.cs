using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour {

    // public Quaternion windDirection;
    public Vector3 windDirection;
    public float windSpeed;
    public GameObject flowerObject;
    public int numberOfFlowers;

    public int fireSampleNr;
    public List<Vector3> fireRays;
    private Text flowerNrText;
    private List<GameObject> flowers;
    private Terrain terrain;
    private LayerMask flammableLayer;

    public enum Mode
    {
        Add, Remove, Fire
    };
    public Mode currMode = Mode.Fire;

	// Use this for initialization
	void Start () {
        // Debug.DrawLine(new Vector3(100.0f, 0.0f, 100.0f), new Vector3(100.0f, 500.0f, 100.0f), Color.red, 10, false);
        flowers = new List<GameObject>();
        terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        generateRandomFlowers();
        flowerNrText = GameObject.Find("FlowerNrText").GetComponent<Text>();
        flowerNrText.text = numberOfFlowers.ToString();
        initializeRays();
        flammableLayer = LayerMask.NameToLayer("Flammable");
    }
	
	// Update is called once per frame
	void Update () {
        // windDirection = Quaternion.AngleAxis(Random.Range(0, 10), new Vector3(1, 0, 0));

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitPoint = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitPoint);
            switch (currMode)
            {
                case Mode.Add:
                    if (hit && hitPoint.transform.gameObject.layer != flammableLayer)
                        addFlower(hitPoint.point);
                    break;
                case Mode.Remove:
                    if (hit && hitPoint.transform.gameObject.layer == flammableLayer)
                        Destroy(hitPoint.transform.gameObject);
                    break;
                case Mode.Fire:
                    if (hit && hitPoint.transform.gameObject.layer == flammableLayer)
                        hitPoint.transform.gameObject.GetComponent<Flammable>().fire();
                    break;
            }
        }
    }

    void initializeRays()
    {
        double step = 2 * Math.PI / fireSampleNr;
        fireRays = new List<Vector3>();
        for (int i = 0; i < fireSampleNr; i++)
        {
            float x = (float)Math.Cos(i * step);
            float z = (float)Math.Sin(i * step);
            fireRays.Add(new Vector3(x, 0, z).normalized);

        }
    }

    void addFlower(Vector3 position)
    {
        GameObject newFlower = (GameObject)Instantiate(flowerObject, position, Quaternion.identity);
        flowers.Add(newFlower);
    }

    void removeExistingFlowers()
    {
        foreach (GameObject obj in flowers)
        {
            Destroy(obj);
            Debug.Log("Destroying");
        }
        flowers = new List<GameObject>();
    }

    public void setFlowerNumber(float nr)
    {
        numberOfFlowers = (int)nr;
        flowerNrText.text = numberOfFlowers.ToString();
    }

    public void generateRandomFlowers()
    {
        removeExistingFlowers();
        for (int i = 0; i < numberOfFlowers; i ++)
        {
            float x = UnityEngine.Random.Range(terrain.transform.position.x + 75, terrain.transform.position.x + terrain.terrainData.size.x - 75);
            float z = UnityEngine.Random.Range(terrain.transform.position.z + 75, terrain.transform.position.z + terrain.terrainData.size.z - 75);
            float y = Terrain.activeTerrain.SampleHeight(new Vector3(x, 0, z));
            addFlower(new Vector3(x, y, z));
        }
    }

    public void changeMode(int mode)
    {
        switch (mode)
        {
            case 0: currMode = Mode.Add; break;
            case 1: currMode = Mode.Remove; break;
            case 2: currMode = Mode.Fire; break;
        }
    }
}
