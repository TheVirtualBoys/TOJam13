using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EndScreen : MonoBehaviour {

    public List<GameObject> didObjects = null;

    public List<GameObject> didntObjects = null;

    private static List<string> persistentAchievements = new List<string>();

    public void AddAchievements(List<string> achievements) {
        EndScreen.persistentAchievements.AddRange(achievements);

        foreach (GameObject didObject in this.didObjects) {
            didObject.SetActive(false);
        }
        foreach (GameObject didntObject in this.didntObjects) {
            didntObject.SetActive(false);
        }

        for (int i = EndScreen.persistentAchievements.Count-1; i >=0 ; i--) {
            int slot = EndScreen.persistentAchievements.Count-1 - i;
            if (slot < this.didObjects.Count) {
                this.didObjects[slot].SetActive(true);
                this.didObjects[slot].GetComponentInChildren<Text>().text = EndScreen.persistentAchievements[i];
            }
        }

        List<string> otherAchievements = new List<string>(DataManager.Instance.allAchievements.Except(EndScreen.persistentAchievements));
        otherAchievements.Shuffle();
        for (int i = 0; i < otherAchievements.Count && i < this.didntObjects.Count; i++) {
            this.didntObjects[i].SetActive(true);
            this.didntObjects[i].GetComponentInChildren<Text>().text = otherAchievements[i];
        }
    }

    public void ClearAchievements() {
        EndScreen.persistentAchievements.Clear();
    }

    public void HandleRetryButton() {
        
    }

    public void HandleQuitButton() {

    }

}
