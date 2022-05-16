using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class chunkManager : MonoBehaviour
{
    public int tileWidth = 50, tileHeight = 50;

    public List<GameObject> chunkTiles;
    public List<GameObject> generatedChunks;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            string loadedChunks = Application.streamingAssetsPath + "/ChunkData/" + "Chunks.txt";
            List<string> chunkLines = File.ReadAllLines(loadedChunks).ToList();
            File.Create(loadedChunks).Close();

            foreach (string chunk in chunkLines)
            {
                string[] data = chunk.Split(',');

                int chunkX = int.Parse(data[0]),
                    chunkY = int.Parse(data[1]),
                    chunkType = int.Parse(data[2]);

                GenerateNewChunk(chunkX, chunkY, chunkType);

                Debug.Log("GENERATED " + chunk);
            }                        
        }
        catch
        {
            Debug.Log("NO VALID CHUNK FILE FOUND");

            GenerateNewChunk(0, 0, Random.Range(0, chunkTiles.Count));
            GenerateNewChunk(1, 0, Random.Range(0, chunkTiles.Count));
            GenerateNewChunk(0, 1, Random.Range(0, chunkTiles.Count));
            GenerateNewChunk(-1, 0, Random.Range(0, chunkTiles.Count));
            GenerateNewChunk(0, -1, Random.Range(0, chunkTiles.Count));
            GenerateNewChunk(-1, 1, Random.Range(0, chunkTiles.Count));
            GenerateNewChunk(1, -1, Random.Range(0, chunkTiles.Count));
            GenerateNewChunk(-1, -1, Random.Range(0, chunkTiles.Count));
            GenerateNewChunk(1, 1, Random.Range(0, chunkTiles.Count));
        }                    
    }

    // Update is called once per frame
    void Update()
    {
        LoadMoreChunks();
    }

    public void LoadMoreChunks()
    {
        try
        {
            int playerXPos = ((int)(player.transform.position.x + tileWidth / 2) / tileWidth);
            int playerZPos = ((int)(player.transform.position.z + tileHeight / 2) / tileHeight);

            SpawnChunksAround(playerXPos, playerZPos);
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex);
        }
    }

    bool CheckChunkExists(int X, int Y)
    {
        bool exists = false;

        foreach(GameObject chunk in generatedChunks)
        {
            if (chunk.GetComponent<chunkData>().chunkX == X
                && chunk.GetComponent<chunkData>().chunkY == Y)
            {
                exists = true;
            }
        }

        return exists;
    }

    void GenerateNewChunk(int X, int Y, int chunkType)
    {
        if (!CheckChunkExists(X, Y))
        {
            string loadedChunks = Application.streamingAssetsPath + "/ChunkData/" + "Chunks.txt";

            using (FileStream fs = new FileStream(loadedChunks, FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(X + "," + Y + "," + chunkType);
            }

            var chunkWidth = chunkTiles[chunkType].GetComponent<chunkData>().chunkWidth;
            var chunkHeight = chunkTiles[chunkType].GetComponent<chunkData>().chunkHeight;

            generatedChunks.Add(Instantiate(
                chunkTiles[chunkType],
                new Vector3(
                    X * chunkWidth - (chunkWidth / 2),
                    transform.position.y,
                    Y * chunkHeight - (chunkHeight / 2)),
                transform.rotation,
                transform));

            generatedChunks.Last().GetComponent<chunkData>().chunkX = X;
            generatedChunks.Last().GetComponent<chunkData>().chunkY = Y;


        }
        else
        {
            Debug.Log("Chunk " + X + "," + Y + " not created because this chunk already exists!");
        }
    }

    /*void SpawnChunkGrid(int width, int height)
    {
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                GenerateNewChunk(i, j, Random.Range(0, chunkTiles.Count));
    }*/

    void SpawnChunksAround(int X, int Y)
    {
        GenerateNewChunk(X, Y, Random.Range(0, chunkTiles.Count));
        GenerateNewChunk(X + 1, Y, Random.Range(0, chunkTiles.Count));
        GenerateNewChunk(X - 1, Y, Random.Range(0, chunkTiles.Count));
        GenerateNewChunk(X, Y + 1, Random.Range(0, chunkTiles.Count));
        GenerateNewChunk(X, Y - 1, Random.Range(0, chunkTiles.Count));
        GenerateNewChunk(X + 1, Y + 1, Random.Range(0, chunkTiles.Count));
        GenerateNewChunk(X - 1, Y - 1, Random.Range(0, chunkTiles.Count));
        GenerateNewChunk(X - 1, Y + 1, Random.Range(0, chunkTiles.Count));
        GenerateNewChunk(X + 1, Y - 1, Random.Range(0, chunkTiles.Count));
    }
}
