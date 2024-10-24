using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    // This Script Created by Wahyu M Rizqi
    // Find me on youtube : https://www.youtube.com/@wahyumrizqi //
    // instagram : @wahyumrizqi
    // Thanks for using my product, good luck for your project //
    // if you faced, any problem let me know //
    // You can also buy me a coffe : https://www.buymeacoffee.com/wahyumrizqi //

    [Header("Menu Scene Settings")]
    public string menu_scene;
    public string first_quiz_scene;
    public string[] quiz_scenes;
    public string result_scene;


    [Header("Quiz Scene Settings")]
    public string correctAnswer;
    public int correctScore;
    //public string nextScene;
    public AudioSource sound;
    public AudioClip correctSound;
    public AudioClip wrongSound;
    public AudioClip hintSound;


    [Header("Result Scene Settings")]
    public Text text_score;
    public GameObject[] stars;
    public int _1_star_limit;
    public int _2_star_limit;
    public int _3_star_limit;

    [Header("For Debugging Only (Ignore Me)")]
    public int currentScore;
    Scene activeScene;
    // Start is called before the first frame update
    void Start()
    {
        currentScore = PlayerPrefs.GetInt("score");
        activeScene = SceneManager.GetActiveScene();
        
        if(activeScene.name == menu_scene)
        {
            PlayerPrefs.SetString("result_scene", result_scene);
            PlayerPrefs.SetString("quiz_scene", first_quiz_scene);

            PlayerPrefs.SetInt("total_quiz_scenes", quiz_scenes.Length);
            PlayerPrefs.SetInt("step", 0);
            currentScore = PlayerPrefs.GetInt("score");
            RandomScenes();

        }
        else if (activeScene.name == PlayerPrefs.GetString("result_scene"))
        {
            if(currentScore <= _1_star_limit)
            {
                stars[0].SetActive(true);
                stars[1].SetActive(false);
                stars[2].SetActive(false);
            }
            else if(currentScore <= _2_star_limit)
            {
                stars[0].SetActive(true);
                stars[1].SetActive(true);
                stars[2].SetActive(false);
            }
            else if(currentScore <= _3_star_limit)
            {
                stars[0].SetActive(true);
                stars[1].SetActive(true);
                stars[2].SetActive(true);
            }
            text_score.text = "" + currentScore;
            PlayerPrefs.SetInt("step", 0);

            RandomScenes();

        }
    }

    public void OpenScenes(string halaman)
    {
        SceneManager.LoadScene(halaman);
    }

    public void OpenPopup(GameObject gameobject)
    {
        gameobject.SetActive(true);
    }

    public void ClosePopup(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    public void UserAnswer(string answer)
    {
        StartCoroutine(AnwerCheck(answer));
    }

    IEnumerator AnwerCheck(string answer)
    {
        if (correctAnswer == answer)
        {
            currentScore = currentScore + correctScore;
            PlayerPrefs.SetInt("score", currentScore);
            sound.clip = correctSound;
            sound.Play();
        }
        else
        {
            sound.clip = wrongSound;
            sound.Play();
        }
        yield return new WaitForSeconds(1f);
        Rotator();
        //SceneManager.LoadScene(nextScene);
    }

    public void PlaySound()
    {
        sound.clip = hintSound;
        sound.Play();
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void RandomScenes()
    {
        List<int> ints = new List<int>();
        List<int> values = new List<int>();
        for (int i = 0; i < quiz_scenes.Length; ++i)
        {
            ints.Add(i);
        }

        for (int i = 0; i < quiz_scenes.Length; ++i)
        {
            int index = Random.Range(0, ints.Count);
            values.Add(ints[index]);
            ints.RemoveAt(index);
        }

        for (int i = 0; i < quiz_scenes.Length; ++i)
        {
            PlayerPrefs.SetString("scene_" + i, quiz_scenes[values[i]]);
        }
    }

    public void Rotator()
    {
        string sceneSelanjutnya = "scene_" + PlayerPrefs.GetInt("step");

        if (PlayerPrefs.GetInt("step") == 0)
        {
            PlayerPrefs.DeleteKey("score");
        }

        if (PlayerPrefs.GetInt("step") < PlayerPrefs.GetInt("total_quiz_scenes"))
        {
            int z = PlayerPrefs.GetInt("step");
            z++;
            PlayerPrefs.SetInt("step", z);
            SceneManager.LoadScene(PlayerPrefs.GetString(sceneSelanjutnya));
        }
        else
        {
            SceneManager.LoadScene(PlayerPrefs.GetString("result_scene"));
        }
    }
}
