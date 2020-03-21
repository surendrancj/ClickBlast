using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ThemeData", menuName = "Theme Data", order = 51)]
public class ThemeData : ScriptableObject
{

    public string themeName = "";
    public AudioClip burstAudioClip;
    public AudioClip bgAudioClip;
    public Color[] allBallColors;
    public GameObject burstEffectPrefab;
    public GameObject bgPrefab;
}
