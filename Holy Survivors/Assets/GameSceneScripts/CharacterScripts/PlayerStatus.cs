using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatus : MonoBehaviour
{
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI spText;
    internal Player player;

    private int hp;
    private int sp;

    void Update()
    {
        hp = player.getHP();
        sp = (int) player.getStamina();

        hpText.SetText("HP: " + hp.ToString());
        spText.SetText("SP: " + sp.ToString());
    }
}
