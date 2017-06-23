using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour {
    
    public Vector3 windDirection;
    public float windSpeed;
    public GameObject flowerObject;

    public float simulationSpeed;
    public bool pause;

    public int numberOfFlowers;
    private Text flowerNrText;
    public int randomFlowersNr;
    private Text randomFlowersNrText;
    public List<Vector3> fireRays;
    public int fireSampleNr;
    private Text pauseText;
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
        flowers = new List<GameObject>();
        terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        flowerNrText = GameObject.Find("FlowerNrText").GetComponent<Text>();
        pauseText = GameObject.Find("PauseText").GetComponent<Text>();
        flowerNrText.text = numberOfFlowers.ToString();
        randomFlowersNrText = GameObject.Find("randomFloNrTxt").GetComponent<Text>();
        randomFlowersNrText.text = randomFlowersNr.ToString();
        flammableLayer = LayerMask.NameToLayer("Flammable");
        pause = false;
        simulationSpeed = 0.0f;
        generateRandomFlowers();
        initializeRays();
        resizeGUI();
    }
	
	// Update is called once per frame
	void Update () {

        // stop when mouse is over UI object
        if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
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

    public void setWindSpeed(float speed)
    {
        windSpeed = speed;
    }

    public void setWindDir(float dir)
    {
        double d = dir / 360 * Math.PI * 2;
        windDirection = new Vector3((float)Math.Sin(-d), 0.0f, (float)Math.Cos(-d)).normalized;
    }

    void resizeGUI()
    {
        CanvasScaler cs = GameObject.Find("Main GUI").GetComponent<CanvasScaler>();
        cs.referenceResolution = new Vector2(Screen.width, Screen.height);
    }

    void initializeRays()
    {
        // generate vectors that will be casted via SphereCast when spreading fire
        double step = 2 * Math.PI / fireSampleNr;
        fireRays = new List<Vector3>();
        for (int i = 0; i < fireSampleNr; i++)
        {
            float x = (float)Math.Cos(i * step);
            float z = (float)Math.Sin(i * step);
            fireRays.Add(new Vector3(x, 0, z).normalized);

        }
    }

    public void pauseSimulation()
    {
        pause = !pause;
        pauseText.text = pause ? "Play" : "Pause";
    }

    void addFlower(Vector3 position)
    {
        GameObject newFlower = (GameObject)Instantiate(flowerObject, position, Quaternion.identity);
        flowers.Add(newFlower);
    }

    public void removeExistingFlowers()
    {
        foreach (GameObject obj in flowers)
        {
            Destroy(obj);
        }
        flowers = new List<GameObject>();
    }

    public void setFlowerNumber(float nr)
    {
        numberOfFlowers = (int)nr;
        flowerNrText.text = numberOfFlowers.ToString();
    }

    public void setRandomFlowerNr(float nr)
    {
        randomFlowersNr = (int)nr;
        randomFlowersNrText.text = randomFlowersNr.ToString();
    }

    public void generateRandomFlowers()
    {
        removeExistingFlowers();
        for (int i = 0; i < numberOfFlowers; i ++)
        {
            float x = UnityEngine.Random.Range(terrain.transform.position.x + 60, terrain.transform.position.x + terrain.terrainData.size.x - 60);
            float z = UnityEngine.Random.Range(terrain.transform.position.z + 60, terrain.transform.position.z + terrain.terrainData.size.z - 60);
            float y = Terrain.activeTerrain.SampleHeight(new Vector3(x, 0, z));
            addFlower(new Vector3(x, y, z));
        }
    }

    public void fireRandomFlowers()
    {
        System.Random rnd = new System.Random();
        int flowerNr = flowers.ToArray().Length;

        // fill array with indices [0, 1, .., n]
        int[] shuffledIndices = new int[flowerNr];
        for (int i = shuffledIndices.Length - 1; i >= 0; i--)
        {
            shuffledIndices[i] = i;
        }

        // shuffle indices - take i-th element and switch it with random element with lower index
        for (int i = shuffledIndices.Length - 1; i >= 0; i --)
        {
            int idx = rnd.Next(i + 1);
            shuffledIndices[idx] = shuffledIndices[i];
        }

        // burn first cnt flowers
        for (int i = 0; i < randomFlowersNr; i ++)
        {
            flowers[shuffledIndices[i]].GetComponent<Flammable>().fire();
        }
    }

    void resizeButton(int mode)
    {
        Debug.Log(mode);
        GameObject.Find("mode_0").GetComponent<Image>().color = new Color(0.84f, 0.84f, 0.84f);
        GameObject.Find("mode_1").GetComponent<Image>().color = new Color(0.84f, 0.84f, 0.84f);
        GameObject.Find("mode_2").GetComponent<Image>().color = new Color(0.84f, 0.84f, 0.84f);

        GameObject.Find("mode_" + mode.ToString()).GetComponent<Image>().color = Color.white;

    }

    public void changeMode(int mode)
    {
        switch (mode)
        {
            case 0: currMode = Mode.Add; break;
            case 1: currMode = Mode.Remove; break;
            case 2: currMode = Mode.Fire; break;
        }
        resizeButton(mode);
    }

    public void changeSimulationSpeed(float val)
    {
        simulationSpeed = val;
        Debug.Log(simulationSpeed);
    }

    public void quitApp()
    {
        Application.Quit();
    }
}
