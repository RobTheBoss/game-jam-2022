using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class GameOver : MonoBehaviour
{
    public PostProcessVolume volume;

    public void EndOfGame()
    {
        volume.GetComponent<DepthOfField>().enabled.value = true;
    }
}
