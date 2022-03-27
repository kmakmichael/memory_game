using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VaultGame : MonoBehaviour
{
    private int difficulty = 5; // start at 1 or 2, max 15s
    [SerializeField]
    private GameObject paper;
    private Text paper_txt;
    [SerializeField]
    private AudioSource paper_sound;
    [SerializeField]
    private Text disp_txt;
    // Start is called before the first frame update
    void Awake()
    {
        paper_txt = paper.transform.GetChild(0).GetComponent<Text>();
        disp_txt.text = "";
    }

    void Start() {
        Setup();
        ShowCode();
    }

    private void Setup() {
        paper_txt.text = randomCombo(difficulty);
        disp_txt.text = "";
    }

    private void ShowCode() {
        paper.SetActive(true);
        paper_sound.Play();
        Invoke("HidePaper", 3);
    }

    private void HidePaper() {
        paper_sound.PlayOneShot(paper_sound.clip, 1.0f);
        paper.SetActive(false);
    }

    public void PressButton(string d) {
        disp_txt.text += d;
        Debug.Log("pressed " + d + " digit, guess now " + disp_txt.text);
    }

    public void PressEnter() {
        if (disp_txt.text.Equals(paper_txt.text)) {
            Debug.Log("you win");
        } else {
            Debug.Log("you lose");
        }
    }

    public void PressDelete() {
        if (disp_txt.text.Length > 1) {
            disp_txt.text = disp_txt.text.Substring(0, disp_txt.text.Length-1);
            Debug.Log("guess is now " + disp_txt.text);
        } else {
            disp_txt.text = "";
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