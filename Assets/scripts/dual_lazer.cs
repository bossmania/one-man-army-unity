using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dual_lazer : MonoBehaviour {

    public Transform barrel;
    public LineRenderer lr;
    public player player_scr;

    void Update() {
        if (Input.GetButton("aim") || Input.GetAxisRaw("aim") < 0) {
            if (player_scr.can_shoot) {
                lr.enabled = true;
                RaycastHit2D hit = Physics2D.Raycast(barrel.position, barrel.TransformDirection(Vector2.up), Mathf.Infinity, Physics2D.DefaultRaycastLayers, -5, 5);
                List<Vector3> pos = new List<Vector3>();
                pos.Add(barrel.position);
                if (hit.collider != null) {
                    pos.Add(hit.point);
                } else {
                    pos.Add(barrel.TransformDirection(Vector2.up).normalized * 1000);
                }
                lr.startColor = Color.red;
                lr.endColor = Color.red;
                lr.startWidth = 0.05f;
                lr.endWidth = 0.05f;
                lr.SetPositions(pos.ToArray());
                lr.useWorldSpace = true;
            } else {
                lr.enabled = false;
            }
        }

        if (Input.GetButtonUp("aim") || Input.GetAxisRaw("aim") == 0) {
            lr.enabled = false;
        }
    }
}
