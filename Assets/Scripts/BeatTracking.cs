using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BeatTracking : MonoBehaviour
{
    [SerializeField] public AudioSource source;
    [SerializeField] public AudioClip song;
    [SerializeField] public Slider beatSlider;
    [SerializeField] public Slider onBeatAreaSlider;

    public int currentSamples = 0;
    public int currentTimeSamplesMin = 0;
    public int currentTimeSamplesMax = 0;

    private int lastFrameSamples = 0;
    private int samplesPerBeat = 0;
    private int bpm = 132;
    private int beatMultiplier = 1;

    private int onBeatOffset;

    public bool isOnBeat;

    public int beatCounter = 1;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.clip = song;
    }
    void Start()
    {
        samplesPerBeat = (int)(source.clip.frequency * (60f / bpm));
        //Debug.Log("samplesPerBeat: " + samplesPerBeat);

        onBeatOffset = (int)(samplesPerBeat * 0.20f);
        //Debug.Log("omBeatOffset: " + onBeatOffset);

        beatSlider.maxValue = samplesPerBeat + onBeatOffset;

        onBeatAreaSlider.maxValue = samplesPerBeat + onBeatOffset;
        onBeatAreaSlider.value = samplesPerBeat - onBeatOffset;


        StartCoroutine(StartSongDelayed());
    }

    private void Update()
    {
        isOnBeat = CheckForNewBeat();
    }

    public bool CheckForNewBeat()
    {
        currentSamples += source.timeSamples - lastFrameSamples;
        lastFrameSamples = source.timeSamples;
        beatSlider.value = currentSamples;

        if ((currentSamples / samplesPerBeat) >= beatMultiplier)
        {
            currentSamples -= (samplesPerBeat * beatMultiplier);
            beatCounter++;
        }

        if ((currentSamples - samplesPerBeat) <= 0 && (currentSamples - samplesPerBeat) > (onBeatOffset * -1))
        {
            return true;
        }
        if (currentSamples > 0 && currentSamples <= onBeatOffset)
        {
            return true;
        }

        return false;
    }

    IEnumerator StartSongDelayed()
    {
        yield return new WaitForSecondsRealtime(5);
        source.Play();
    }
}
