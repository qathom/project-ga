using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;
// using UnityEngine.EventSystems;

public class QuizButton : MonoBehaviourPun {
	public Button button;
	public string buttonReference;

	void Start () {
		Button btn = button.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick() {
		Debug.Log("You have clicked the button!");

		// TODO
		// photonView.RPC("AddNewResult", RpcTarget.All, buttonReference);

		/*
		MENEUR = {1.2, 2.1, 3.1}
		TIMIDE = {1.2, 2.2, 3.3}
		INVISIBLE = {1.2, 2.2, 3.2}
		*/

		int x = Int16.Parse(buttonReference[0].ToString());

		string currentPanel = String.Format("Panel {0}", x);
		string nextPanel = String.Format("Panel {0}", x + 1);

		// Update result
		QuizPanel.LocalInstance.AddResult(buttonReference);

		// Hide current
		QuizPanel.LocalInstance.SetVisibility(currentPanel, false);

		// Show result
		if (nextPanel == "Panel 4") {
			QuizPanel.LocalInstance.BuildResult();
			return;
		}

		// Show next
		QuizPanel.LocalInstance.SetVisibility(nextPanel, true);
	}
}
