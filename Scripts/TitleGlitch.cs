using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleGlitch : MonoBehaviour
{
    public Material glitchMat;
    public Material robotoBoldMat;

    bool Glitching = false;

    void Start()
    {
        //run Glitch once a second
        InvokeRepeating("Glitch", 1.0f, 1.0f);
    }

    void Glitch()
    {
        if (Random.Range(0, 10) == 0 && Glitching == false)
        {
            //start glitching
            Glitching = true;
            GetComponent<CanvasRenderer>().SetMaterial(glitchMat, 0);

            //call coroutine to stop glitching in 1 second
            StartCoroutine(StopGlitching(1.0f));
        }
    }

    IEnumerator StopGlitching(float time)
    {
        yield return new WaitForSeconds(time);

        Glitching = false;
        GetComponent<CanvasRenderer>().SetMaterial(robotoBoldMat, 0);

    }
}
