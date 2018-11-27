using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class visualizerScript : MonoBehaviour {
    #region PUBLIC VARIABLES
    [Tooltip("Where to get the audio")]
    public AudioSource audioSource;    
    [Tooltip("Cube to use as prefab for the visualizer")]
    public GameObject visualizeCubePrefab;    
    [Tooltip("Amount of bars in the visualizer")]
    public int barAmount;
    [Tooltip("How large the bars can get")]
    public float maxScale;
    [Tooltip("How wide should each bar be?")]
    public float barWidth = 10;
    #endregion

    #region PRIVATE VARIABLES
    private float[] spectrumSamples = new float[512];
    private float[] freqBand = new float[8];
    private GameObject[] cubePrefabArray;
    #endregion

    // Use this for initialization
    void Start () {
        cubePrefabArray = new GameObject[barAmount];
        for(int i = 0; i < barAmount; i++)
        {
            GameObject cubeInstance = (GameObject)Instantiate(visualizeCubePrefab);
            Renderer tempRenderer = cubeInstance.GetComponent<Renderer>();
            float hueUpdatePerBar = 360 / (float)barAmount;
            tempRenderer.material.SetColor("Color_5202825D", Color.HSVToRGB(((i + 1) * hueUpdatePerBar) / 360, 1, 1));
            cubeInstance.transform.position = this.transform.position;
            cubeInstance.transform.parent = this.transform;
            cubeInstance.name = "VisualCube" + i;
            this.transform.eulerAngles = new Vector3(0, 0, (-360 / (float)barAmount) * i);
            cubeInstance.transform.position = Vector3.up * 1;
            cubePrefabArray[i] = cubeInstance;
        }
	}
	
	// Update is called once per frame
	void Update () {
        audioSource.GetSpectrumData(spectrumSamples, 0, FFTWindow.Blackman);
        if (cubePrefabArray != null)
        {
            for (int i = 0; i < barAmount; i++)
            {
                int sampleBatchSize = 512 / barAmount;
                float spectrumSample = 0;
                for(int j = 0; j < sampleBatchSize; j++)
                {
                    spectrumSample += spectrumSamples[j * i] * (i + 1);
                }
                spectrumSample /= sampleBatchSize;
                cubePrefabArray[i].transform.localScale = new Vector3(barWidth, (spectrumSample * maxScale) + 0.5f, 10);
            }
        }
            
	}
}
