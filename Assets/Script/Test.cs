using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Test : MonoBehaviour
{
    public GameObject grid; // Grid 오브젝트
    public Transform player;
    private Vector3Int playerGridPosition;
    private Vector3Int lastPlayerGridPosition;

    private Vector2 tilemapSize; // 타일맵 크기

    void Start()
    {
        var tile = grid.transform.GetChild(0);

        tilemapSize = CalculateTileSize(tile.GetComponent<Tilemap>());



        int index = 0;

        // 타일맵 배치
        for (float x = -tilemapSize.x / 2; x <= tilemapSize.x / 2; x += tilemapSize.x)
        {
            for (float y = -tilemapSize.y / 2; y <= tilemapSize.y / 2; y += tilemapSize.y)
            {
                grid.transform.GetChild(index++).position = new Vector3(x, y, 0);
            }
        }

        // 플레이어를 중앙 타일맵의 중앙에 위치시키기
        player.transform.position = new Vector3(0, 0, 0);

        // 초기 플레이어 위치 저장
        playerGridPosition = GetGridPosition(player.position);
        lastPlayerGridPosition = playerGridPosition;
    }

    Vector2 CalculateTileSize(Tilemap tilemap)
    {
        // 타일맵의 셀 경계를 가져옵니다.
        BoundsInt bounds = tilemap.cellBounds;

        // 실제로 타일이 있는 영역을 계산합니다.
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
        // 현재 플레이어 위치를 그리드 좌표로 변환
        playerGridPosition = GetGridPosition(player.position);

        // 플레이어가 다른 그리드 칸으로 이동했는지 확인
        if (playerGridPosition != lastPlayerGridPosition)
        {
            UpdateTilemaps();
            lastPlayerGridPosition = playerGridPosition;
        }
    }

    // 월드 좌표를 그리드 좌표로 변환
    Vector3Int GetGridPosition(Vector3 position)
    {
        return new Vector3Int(Mathf.RoundToInt(position.x / tilemapSize.x), Mathf.RoundToInt(position.y / tilemapSize.y), 0);
    }

    // 타일맵 업데이트
    void UpdateTilemaps()
    {
        Vector3Int offset = playerGridPosition - lastPlayerGridPosition;

        foreach (Transform child in grid.transform)
        {
            child.position += new Vector3(offset.x * tilemapSize.x, offset.y * tilemapSize.y, 0);
        }
    }
}
