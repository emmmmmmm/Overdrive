using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverPoint : MonoBehaviour {
    public float heightCorrection = 0f;

void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}
