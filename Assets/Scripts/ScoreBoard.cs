using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
public class ScoreBoard : MonoBehaviour
{
    public static ScoreBoard Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    } //Urrrghh

    [SerializeField] public TMP_Text comboMultiplier;
    [SerializeField] public TMP_Text score;


    private int points = 0;
    private int multiplier = 1;

    public bool lastActionOnBeat =  true;
    public int Points
    {
        get {  return points; ; }
        set
        {
            points = value;
            UpdateUI(score, points);
        }
    }
    public int Multiplier
    {
        get 
        { 
            UpdateUI(comboMultiplier, multiplier);
            return multiplier;
        }
        set
        {
            multiplier = value;
            UpdateUI(comboMultiplier, multiplier);
        }
    }

    private void Start()
    {
        UpdateUI(comboMultiplier, multiplier);
    }
    private void UpdateUI(TMP_Text _text, int points)
    {
        _text.text = points.ToString();
    }
    public void AddPoints(int _points, int _combo)
    {
        Points += (_points * _combo);
    }
    public void AddCombo()
    {
        if (Multiplier <= 4)
        {
            Multiplier++;
        }
    }
    public void SubtractCombo()
    {
        if (Multiplier > 1)
        {
            multiplier--;
        }
    }

    private void OnDestroy() //Urrrghhh...
    {
        Destroy(Instance);
    }
}
