using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chunkData : MonoBehaviour
{
    public float chunkWidth, chunkHeight;
    public int chunkX, chunkY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 1, 1);

        Gizmos.DrawWireCube(
            new Vector3(transform.position.x, transform.position.y + chunkHeight / 2, transform.position.z),
            new Vector3(chunkWidth, chunkHeight, chunkHeight));
    }
}
