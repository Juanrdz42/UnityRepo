using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "PhotoSequenceData", menuName = "MiniGame/Photo Sequence Data")]
public class PhotoSequenceData : ScriptableObject
{
    public string birdName;
    public VideoClip videoClip;
    public PhotoTimingWindow[] timingWindows;
}