using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Test : MonoBehaviour
{
    public GameObject grid; // Grid ������Ʈ
    public Transform player;
    private Vector3Int playerGridPosition;
    private Vector3Int lastPlayerGridPosition;

    private Vector2 tilemapSize; // Ÿ�ϸ� ũ��

    void Start()
    {
        var tile = grid.transform.GetChild(0);

        tilemapSize = CalculateTileSize(tile.GetComponent<Tilemap>());



        int index = 0;

        // Ÿ�ϸ� ��ġ
        for (float x = -tilemapSize.x / 2; x <= tilemapSize.x / 2; x += tilemapSize.x)
        {
            for (float y = -tilemapSize.y / 2; y <= tilemapSize.y / 2; y += tilemapSize.y)
            {
                grid.transform.GetChild(index++).position = new Vector3(x, y, 0);
            }
        }

        // �÷��̾ �߾� Ÿ�ϸ��� �߾ӿ� ��ġ��Ű��
        player.transform.position = new Vector3(0, 0, 0);

        // �ʱ� �÷��̾� ��ġ ����
        playerGridPosition = GetGridPosition(player.position);
        lastPlayerGridPosition = playerGridPosition;
    }

    Vector2 CalculateTileSize(Tilemap tilemap)
    {
        // Ÿ�ϸ��� �� ��踦 �����ɴϴ�.
        BoundsInt bounds = tilemap.cellBounds;

        // ������ Ÿ���� �ִ� ������ ����մϴ�.
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

    void Update()
    {
        // ���� �÷��̾� ��ġ�� �׸��� ��ǥ�� ��ȯ
        playerGridPosition = GetGridPosition(player.position);

        // �÷��̾ �ٸ� �׸��� ĭ���� �̵��ߴ��� Ȯ��
        if (playerGridPosition != lastPlayerGridPosition)
        {
            UpdateTilemaps();
            lastPlayerGridPosition = playerGridPosition;
        }
    }

    // ���� ��ǥ�� �׸��� ��ǥ�� ��ȯ
    Vector3Int GetGridPosition(Vector3 position)
    {
        return new Vector3Int(Mathf.RoundToInt(position.x / tilemapSize.x), Mathf.RoundToInt(position.y / tilemapSize.y), 0);
    }

    // Ÿ�ϸ� ������Ʈ
    void UpdateTilemaps()
    {
        Vector3Int offset = playerGridPosition - lastPlayerGridPosition;

        foreach (Transform child in grid.transform)
        {
            child.position += new Vector3(offset.x * tilemapSize.x, offset.y * tilemapSize.y, 0);
        }
    }
}
