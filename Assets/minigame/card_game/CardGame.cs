using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class CardGame : MonoBehaviour
{
    enum card_type {
        none,
        apple,
        banana,
        strawberry,
        leek,
        watermelon,
        carrot,
        eggplant,
        cherry,
        lemon,
        tomato,
        broccoli,
        orange,
        pineapple,
        pepper,
        cheese,
        onion,
        taco,
        sandwich,
        donut,
        egg,
        cookie,
        cupcake,
        kiwi,
        meat,
        milk
    }
    private int moves = 0;
    private card_type[,] board;
    private bool[,] matched;
    private (int,int) blen = (4,4);
    private GameObject active;
    [SerializeField]
    private GameObject endcard;
    [SerializeField]
    private GameObject board_cover;
    [SerializeField]
    private RectTransform game_box;

    [SerializeField]
    private GameObject blank_card;
    [SerializeField]
    private SpriteAtlas atlas;

    void Start() {
        endcard.SetActive(false);
        Canvas.ForceUpdateCanvases();
        Setup();
    }

    private void Setup() {
        moves = 0;
        Cleanup();
        FillBoard();
        StartCoroutine("PlaceCards");
    }

    private void FillBoard() {
        board = new card_type[blen.Item1,blen.Item2];
        int ct_max = blen.Item1 * blen.Item2 / 2;
        for (int ct = 1; ct <= ct_max; ct++) {
            // place two cards randomly
            card_type rx = (card_type) ct;
            for (int i = 0; i < 2; i++) {
                (int, int) pos = RandomPosition();
                while (board[pos.Item1, pos.Item2] != 0) {
                    pos = RandomPosition();
                }
                board[pos.Item1, pos.Item2] = rx;
            }
        }
    }

    private card_type RandomCard() {
        int i = Random.Range(1,System.Enum.GetNames(typeof(card_type)).Length);
        return (card_type)i;
    }

    private (int, int) RandomPosition() {
        return (Random.Range(0,blen.Item1), Random.Range(0,blen.Item2));
    }

    private IEnumerator PlaceCards() {
        board_cover.SetActive(true);
        for (int y = 0; y < blen.Item1; y++) {
            for (int x = 0; x < blen.Item2; x++) {
                if (board[y,x] != card_type.none) {
                    GameObject c = CreateCard(board[y,x],(y,x));
                    yield return new WaitForSeconds(0.2f);
                }
            }
        }
        yield return new WaitForSeconds(0.3f);
        board_cover.SetActive(false);
    }

    private GameObject CreateCard(card_type ctype, (int,int) pos) {
        float p;
        if (blen.Item1 > blen.Item2) {
            p = 0.2f / blen.Item1;
        } else {
            p = 0.2f / blen.Item2;
        }
        float o_h = (1.0f-p) / blen.Item2;
        float o_w = (1.0f-p) / blen.Item1;
        float c_h = o_h - p;
        float c_w = o_w - p;
        float x1 = p + o_w * pos.Item2;
        float y1 = p + o_w * pos.Item1;
        float dim = c_w > c_h ? c_h : c_w;

        GameObject c = Instantiate(blank_card, game_box.transform);
        c.name = "card_" + pos.Item1 + "-" + pos.Item2;
        RectTransform r = c.GetComponent<RectTransform>();
        r.sizeDelta = new Vector2(dim, dim);
        r.anchorMax = new Vector2(x1+dim,y1+dim);
        r.anchorMin = new Vector2(x1,y1);
        Button b = c.transform.Find("back").gameObject.GetComponent<Button>();
        Card scr = c.GetComponent<Card>();
        scr.SetCardType((int)ctype);
        b.onClick.AddListener(delegate {ClickCard(c, pos); });
        // set the proper texture
        Image img = c.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        img.sprite = atlas.GetSprite(System.Enum.GetName(typeof(card_type), ctype));
        return c;
    }

    private void ClickCard(GameObject c, (int,int) pos) {
        if (active != null) {
            ++moves;
            Card scr_a = active.GetComponent<Card>();
            Card scr_b = c.GetComponent<Card>();
            if (scr_a.GetCardType() == scr_b.GetCardType()) {
                // play a nice sound? honestly idk, might just do nothing
                scr_a.matched = true;
                scr_b.matched = true;
                if (CheckBoard()) {
                    WinRound();
                }
            } else {
                // wait a second first
                StartCoroutine(FlipBack(scr_a, scr_b));
            }
            active = null;
        } else {
            active = c;
        }
    }

    private IEnumerator FlipBack(Card a, Card b) {
        yield return new WaitForSeconds(1.0f);
        a.Flip();
        b.Flip();
    }

    private bool CheckBoard() {
        bool vic = true;
        foreach (Card c in game_box.gameObject.GetComponentsInChildren<Card>()) {
            vic &= c.matched;
        }
        return vic;
    }

    private void Cleanup() {
        foreach (Transform child in game_box.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void WinRound() {
        endcard.SetActive(true);
        endcard.transform.GetChild(0).Find("score").GetComponent<Text>().text = "Moves: " + moves;
    }
}
