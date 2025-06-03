using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Reposition : MonoBehaviour
{
    Collider2D coll;

    public GameObject grid;
    public Transform player;
    private Vector3Int playerGridPosition;
    private Vector3Int lastPlayerGridPosition;

    private Vector2 tilemapSize; 

    void Start()
    {
        coll = GetComponent<Collider2D>();

        var tile = grid.transform.GetChild(0);

        tilemapSize = CalculateTileSize(tile.GetComponent<Tilemap>());

        int index = 0;

        for (float x = -tilemapSize.x / 2; x <= tilemapSize.x / 2; x += tilemapSize.x)
        {
            for (float y = -tilemapSize.y / 2; y <= tilemapSize.y / 2; y += tilemapSize.y)
            {
                grid.transform.GetChild(index++).position = new Vector3(x, y, 0);
            }
        }

        player.transform.position = new Vector3(0, 0, 0);

        playerGridPosition = GetGridPosition(player.position);
        lastPlayerGridPosition = playerGridPosition;
    }

    Vector2 CalculateTileSize(Tilemap tilemap)
    {
        BoundsInt bounds = tilemap.cellBounds;

        Vector3Int? min = null;
        Vector3Int? max = null;

        foreach (var pos in bounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                if (min == null)
                {
                    min = pos;
                    max = pos;
                }
                else
                {
                    min = Vector3Int.Min(min.Value, pos);
                    max = Vector3Int.Max(max.Value, pos);
                }
            }
        }

        int width = -1;
        int height = -1;

        if (min.HasValue && max.HasValue)
        {
            width = max.Value.x - min.Value.x + 1;
            height = max.Value.y - min.Value.y + 1;
        }

        return new Vector2(width, height);
    }

    Vector3Int GetGridPosition(Vector3 position)
    {
        return new Vector3Int(Mathf.RoundToInt(position.x / tilemapSize.x), Mathf.RoundToInt(position.y / tilemapSize.y), 0);
    }
    void Update()
    {
        playerGridPosition = GetGridPosition(player.position);

        if (playerGridPosition != lastPlayerGridPosition)
        {
            UpdateTilemaps();
            lastPlayerGridPosition = playerGridPosition;
        }
    }
    void UpdateTilemaps()
    {
        Vector3Int offset = playerGridPosition - lastPlayerGridPosition;

        foreach (Transform child in grid.transform)
        {
            child.position += new Vector3(offset.x * tilemapSize.x, offset.y * tilemapSize.y, 0);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        switch (transform.tag)
        {
            case "Enemy":
                if (coll.enabled)
                {
                    Vector3 dist = playerPos - myPos;
                    Vector3 ran = new Vector3(Random.Range(-20, 20), Random.Range(-13, 13), 0);
                    transform.Translate(ran + dist * 2);
                }
                break;
        }
    }
}
