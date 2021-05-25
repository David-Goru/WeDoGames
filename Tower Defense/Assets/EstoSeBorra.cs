using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstoSeBorra : MonoBehaviour
{
    public GameObject ejercito;

    private GameObject[] goblins;
    private List<Animator> anims = new List<Animator>();

    public Material Skybox;

    public GameObject floor;
    public float speed;

    private bool translate = false;

    private void Awake()
    {
        goblins = GameObject.FindGameObjectsWithTag("Finish");
        foreach(GameObject GO in goblins)
        {
            anims.Add(GO.GetComponentInChildren<Animator>());
        }

        ejercito.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ejercito.SetActive(true);
            foreach(Animator anim in anims)
            {
                anim.SetFloat("animSpeed", 1.5f);
            }
            translate = true;
            RenderSettings.skybox = Skybox;
        }

        if (translate)
        {
            floor.transform.Translate(-Vector3.forward * speed * Time.deltaTime);
        }
    }
}
