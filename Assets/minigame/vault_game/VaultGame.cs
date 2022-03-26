using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultGame : MonoBehaviour
{
    private string combo;
    private string guess;
    private int difficulty = 5; // start at 1 or 2
    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    private void Setup() {
        combo = randomCombo(difficulty);
        guess = "";
    }

    public void PressButton(string d) {
        guess += d;
        Debug.Log("pressed " + d + " digit, guess now " + guess);
    }

    public void PressEnter() {
        if (guess.Equals(combo)) {
            Debug.Log("you win");
        } else {
            Debug.Log("you lose");
        }
    }

    public void PressDelete() {
        if (guess.Length > 1) {
            guess = guess.Substring(0, guess.Length-1);
            Debug.Log("guess is now " + guess);
        } else {
            guess = "";
            Debug.Log("guess is now empty");
        }
    }

    private string randomCombo(int len) {
        Debug.Log("generating " + len + "-digit combo...");
        string com = "12345";
        Debug.Log("secret combo is " + com);
        return com;
    }
}
