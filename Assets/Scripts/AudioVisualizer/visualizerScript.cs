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

    void MakeFrequencyBands()
    {
        /*
         * Songs range from 20-20000 hertz usually
         * 20000/512 is almost 40 hertz per sample
         * 
         * 20-60 hertz
         * 60-250 hertz
         * 250-500
         * 500-2000
         * 2000-4000
         * 4000-6000
         * 6000-20000
         * 
         * 8 Bands:
         * 
         * 0 - 2 Samples = 80hertz  0-80
         * 1 - 4 Samples = 160hertz 80-240
         * 2 - 8 Samples = 320 hertz 240-560
         * 3 - 16 Samples = 640 hertz 560-1200
         * 4 - 32 Samples = 1280 hertz 1200-2480
         * 5 - 64 Samples = 2560 hertz 2480 - 5060
         * 6 - 128 Samples = 5160 hertz 5060 - 10220
         * 7 - 256 Samples = 10320 hertz 10220 - 20540                  
         */

        int count = 0;
        for(int i = 0; i < freqBand.Length; i++)
        {
            float average = 0;
            int sampleCount = (2 ^ (i + 1));
            //If on the last chunk of samples then add 2 to ensure getting the whole spectrum
            if(i == 7)
            {
                sampleCount += 2;
            }
            for (int j = 0; j < sampleCount; j++)
            {
                average += spectrumSamples[count] * (count + 1);
                count++;
            }

            average /= count;
            freqBand[i] = average * 10;            
        }
    }
}
