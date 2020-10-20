using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freeze_rot : MonoBehaviour
{
    public Rigidbody2D rb;
    public Rigidbody2D player_rb;
    public Transform trans;
    public int y;

    void Update() {
        trans.rotation = Quaternion.Euler(0,0,0);
        rb.position = player_rb.position - new Vector2(0, y);
    }
}
