using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SuggestionScreenController : MonoBehaviour
{
    public void OnOutAnimationFinished()
    {
        SceneManager.LoadSceneAsync(Scenes.StartScreen.ToString());
    }
}
