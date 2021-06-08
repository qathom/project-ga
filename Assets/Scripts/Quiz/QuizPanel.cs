using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.Linq;
using UnityEngine.UI;

public class QuizPanel : MonoBehaviourPun
{
    public static QuizPanel LocalInstance;
    
    public static List<string> results = new List<string>() {};

    public Text resultText;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("SETUP");

        LocalInstance = this;
        Cursor.lockState = CursorLockMode.None;

        // Setup visibility
        SetVisibility("Panel 1", true);
		SetVisibility("Panel 2", false);
		SetVisibility("Panel 3", false);
		SetVisibility("PanelRes", false);
    }

    // Avoid UnityException: Tag: Panel 4 is not defined.
    private GameObject[] getGameObjects(string tagName) {
        try {
		  return GameObject.FindGameObjectsWithTag(tagName);

        } catch {}

        return new GameObject[] {};
    }

    public void AddResult(string res) {
        results.Add(res);
    }

    public void BuildResult() {
        // Show
        SetVisibility("PanelRes", true);

        // Profiles
        string[] LEADER = {"1.2", "2.1", "3.1"};
        string[] SHY = {"1.2", "2.2", "3.3"};
        string[] INVISIBLE = {"1.3", "2.2", "3.2"};

        var profileRes = new Dictionary<string, int>();

        // Init
        profileRes["LEADER"] = 0;
        profileRes["SHY"] = 0;
        profileRes["INVISIBLE"] = 0;

        foreach (string res in results) {
            if (LEADER.Contains(res)) {
                profileRes["LEADER"] += 1;
            } else if (SHY.Contains(res)) {
                profileRes["SHY"] += 1;
            } else if (INVISIBLE.Contains(res)) {
                profileRes["INVISIBLE"] += 1;
            }
        }

        var max = profileRes.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;

        resultText.text = max;
    }

	public void SetVisibility(string tagName, bool active = true) {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tagName);

		foreach (GameObject gameObject in gameObjects)
		{
            if (active) {
                gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            } else {
                gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(100000, 100000, 0);
            }
		}
	}

    [PunRPC]
    void AddNewResult(string a) {
        Debug.LogFormat("Info: {0}", a);
    }
}
