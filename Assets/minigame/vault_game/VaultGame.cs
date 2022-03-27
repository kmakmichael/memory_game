using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VaultGame : MonoBehaviour
{
    private int difficulty = 3; // start at 1 or 2, max 15s
    [SerializeField]
    private GameObject paper;
    private Text paper_txt;
    [SerializeField]
    private Text disp_txt;
    [SerializeField]
    private AudioClip paper_flip, boop;
    [SerializeField]
    private AudioSource asrc;
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
        asrc.PlayOneShot(paper_flip, 1.0f);
        Invoke("HidePaper", 3);
    }

    private void HidePaper() {
        asrc.PlayOneShot(paper_flip, 1.0f);
        paper.SetActive(false);
    }

    public void PressButton(string d) {
        asrc.PlayOneShot(boop, 0.6f);
        disp_txt.text += d;
        Debug.Log("pressed " + d + " digit, guess now " + disp_txt.text);
    }

    public void PressEnter() {
        if (disp_txt.text.Equals(paper_txt.text)) {
            // open the vault and show the loot inside
            ++difficulty;
            Setup();
            ShowCode();
        } else {
            Debug.Log("you lose");
        }
    }

    public void PressDelete() {
        if (disp_txt.text.Length > 1) {
            disp_txt.text = disp_txt.text.Substring(0, disp_txt.text.Length-1);
        } else {
            disp_txt.text = "";
        }
    }

    private string randomCombo(int len) {
        string com = "";
        for (int i = 0; i < difficulty; i++) {
            com += Random.Range(0,10).ToString();
        }
        return com;
    }
}
