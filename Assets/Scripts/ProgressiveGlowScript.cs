using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressiveGlowScript : MonoBehaviour
{
    public GameObject crystal;
    float animationTime = 0.5f;
    bool beingLit = false;
    float redSubtract;
    float greenBlueSubtract;
    float blueSubstract;
    Color c;
    Material material;

    // Start is called before the first frame update
    void Start()
    {
        material = crystal.GetComponent<Renderer>().material;
        c = material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && !beingLit) {
            Lit();
        }

        if (beingLit) {
            float r = c.r;
            float g = c.g;
            float b = c.b;
            float finalRed = 229.0f / 255.0f;
            float finalGreenBlue = 94.0f / 255.0f;

            if (c.r >= finalRed) {
                r -= Time.deltaTime * 0.1f;
            }

            if (g >= finalGreenBlue && b >= finalGreenBlue) {
                g -= Time.deltaTime * 0.5f;
                b -= Time.deltaTime * 0.5f;
            }
            c = new Color(r, g, b, c.a);
            material.SetColor("_Color", c);

            if (c.r <= finalRed && c.g <= finalGreenBlue && c.b <= finalGreenBlue) {
                beingLit = false;
                Behaviour halo = (Behaviour) crystal.transform.GetChild(0).GetComponent("Halo");
                halo.enabled = true;
            }
        }
    }

    public void Lit() {
        beingLit = true;
    }
}
