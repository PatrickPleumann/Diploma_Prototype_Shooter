using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BeatTracking : MonoBehaviour
{
    [SerializeField] public AudioSource source;
    [SerializeField] public AudioClip song;
    [SerializeField] public int asyncValue; // for feeling of asynchronous beat behavior

    [SerializeField] public Slider beatSliderLeft;
    [SerializeField] public Slider onBeatAreaSliderLeft;

    [SerializeField] public Slider beatSliderRight;
    [SerializeField] public Slider onBeatAreaSliderRight;

    //add async value slider with offset samples and preview on current song before starting.

    public int currentSamples = 0;
    public int currentTimeSamplesMin = 0;
    public int currentTimeSamplesMax = 0;

    [SerializeField] private float beatOffsetMultiplier = 0.10f;
    private int lastFrameSamples = 0;
    [SerializeField] public int samplesPerBeat = 0;
    [SerializeField] public int bpm = 90;
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
        samplesPerBeat = (int)(source.clip.frequency * (60f / bpm)) * beatMultiplier;
        onBeatOffset = (int)(samplesPerBeat * 0.10f) * beatMultiplier;

        currentSamples += asyncValue;


        beatSliderLeft.maxValue = samplesPerBeat;
        beatSliderRight.maxValue = samplesPerBeat;

        onBeatAreaSliderLeft.maxValue = samplesPerBeat ;
        onBeatAreaSliderLeft.value = samplesPerBeat - onBeatOffset;

        onBeatAreaSliderRight.maxValue = samplesPerBeat;
        onBeatAreaSliderRight.value = samplesPerBeat - onBeatOffset;

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
        beatSliderLeft.value = currentSamples;
        beatSliderRight.value = currentSamples;

        if ((currentSamples / samplesPerBeat) >= 1)
        {
            currentSamples -= samplesPerBeat;
            beatCounter++;
            if (ScoreBoard.Instance.lastActionOnBeat == true)
            {
                ScoreBoard.Instance.lastActionOnBeat = false;
            }
            else
            {
                ScoreBoard.Instance.SubtractCombo();
            }
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
