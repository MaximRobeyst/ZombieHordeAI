using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
class Point
{
    public bool m_IsVisible;
    public Vector3 m_Position;

    public float m_Width;
    public float m_Height;

    public Point(Vector3 position, float width, float height)
    {
        m_Position = position;
        m_IsVisible = false;

        m_Width = width;
        m_Height = height;
    }
}

[Serializable]
class Grid
{
    public List<Point> m_Points;
    public bool m_PlayerInside;

    public Grid(List<Point> points)
    {
        m_Points = points;
    }
}

public class InfoGrid : MonoBehaviour
{

    [SerializeField] private float m_GridSize = 100.0f;
    [SerializeField] private int m_Columns = 100;
    [SerializeField] private int m_Rows = 100;
    [SerializeField] private float m_MaxHeightDifference = 20.0f;
    [SerializeField] private int m_WorldDivision = 5;

    [SerializeField] private float m_NoSpawnRadius = 25.0f;
    [SerializeField] private float m_VisionRadius = 50.0f;
    [SerializeField] private float m_FovAngle = 1.0f;


    Grid[] m_Points;

    private void Awake()
    {
        CreateWorld();
    }

    void CreateWorld()
    {
        m_Points = new Grid[m_WorldDivision * m_WorldDivision];
        
        Vector3 startPosition = new Vector3(-(m_GridSize / 2), 0, -(m_GridSize / 2));
        float cellSize = m_GridSize / m_WorldDivision;

        Vector3 cellCenter = new Vector3(cellSize / 2, 0, cellSize / 2);

        for(int i = 0; i < m_WorldDivision; ++i)
        {
            for(int j = 0; j < m_WorldDivision; ++j)
            {
                CreateGrid(startPosition + new Vector3(cellSize * i, 0, cellSize * j) , i * m_WorldDivision + j, cellSize);
            }
        }
    }

    void CreateGrid(Vector3 startPosition, int index, float size)
    {
        m_Points[index] = new Grid(new List<Point>());
        float cellWidth = size / m_Columns;
        float cellHeight = size / m_Rows;

        for(int column = 0; column < m_Columns; ++column)
        {
            for(int row = 0; row < m_Rows; ++row)
            {
                NavMeshHit navmeshHit;
                Vector3 currentPosition = startPosition + new Vector3(column * cellWidth, 0, row * cellHeight);

                if (NavMesh.SamplePosition(currentPosition, out navmeshHit, m_MaxHeightDifference, NavMesh.AllAreas))
                {
                    if (Vector3.Distance(new Vector3(navmeshHit.position.x, 0, navmeshHit.position.z), new Vector3(currentPosition.x, 0, currentPosition.z)) > Math.Max(cellWidth, cellHeight))
                        continue;

                    Point point = new Point(navmeshHit.position, cellWidth, cellHeight);

                    if (m_Points[index].m_Points.Contains(point)) continue;
                    m_Points[index].m_Points.Add(point);
                }

            }
        }
    }

    private void OnValidate()
    {
        CreateWorld();
    }

    private void OnDrawGizmosSelected()
    {
        if (m_Points == null) return;
             
        Gizmos.color = Color.green;

        Vector3 startPosition = new Vector3(-(m_GridSize / 2), 0, -(m_GridSize / 2));
        float cellSize = m_GridSize / m_WorldDivision;
        Vector3 cellCenter = new Vector3(cellSize / 2, 0, cellSize / 2);

        for (int i= 0; i < m_WorldDivision; ++i)
        {
            for(int j = 0; j < m_WorldDivision; ++j)
            {
                Gizmos.DrawWireCube(startPosition + new Vector3(cellSize * i, 0, cellSize * j) + cellCenter, new Vector3(cellSize, cellSize, cellSize));

                if (m_Points[i * m_WorldDivision + j].m_Points == null) continue;

                foreach (Point point in m_Points[i * m_WorldDivision + j].m_Points)
                {
                    Gizmos.color = point.m_IsVisible ? Color.red : Color.green;
                    Gizmos.DrawSphere(point.m_Position, 0.5f);
                }
            }
        }

    }

    public void UpdateVisibility(HealthComponent character)
    {
        foreach (var grid in m_Points)
            grid.m_PlayerInside = false;

        int column = (int)(((character.transform.position.z + (m_GridSize / 2)) / m_GridSize) * m_WorldDivision);
        int row = (int)(((character.transform.position.x + (m_GridSize / 2)) / m_GridSize) * m_WorldDivision);

        m_Points[row * m_WorldDivision + column].m_PlayerInside = true;

        for (int i = Math.Max(0, column - 1); i <= Math.Min(m_WorldDivision, column + 1); ++i)
        {
            for (int j = Math.Max(0, row - 1); j <= Math.Min(m_WorldDivision, row + 1); ++j)
            {
                UpdateCurrentGrid(j, i, character);
            }
        }
    }

    void UpdateCurrentGrid(int row, int column, HealthComponent character)
    {
        Grid currentGrid = m_Points[row * m_WorldDivision + column];

        for (int i = 0; i < currentGrid.m_Points.Count; ++i)
        {
            Vector3 direction = (currentGrid.m_Points[i].m_Position - character.transform.position);
            float sqrDistance = Vector3.SqrMagnitude(character.transform.position - currentGrid.m_Points[i].m_Position);

            if (sqrDistance < m_VisionRadius * m_VisionRadius)
            {
                if (sqrDistance < m_NoSpawnRadius * m_NoSpawnRadius)
                {
                    currentGrid.m_Points[i].m_IsVisible = true;
                    continue;
                }

                float angle = Vector3.Angle(new Vector3(character.transform.forward.x, 0, character.transform.forward.z).normalized, new Vector3(direction.x, 0, direction.z).normalized);

                if (angle < m_FovAngle)
                {
                    currentGrid.m_Points[i].m_IsVisible = true;
                    continue;
                }

                currentGrid.m_Points[i].m_IsVisible = false;
            }
            else
                currentGrid.m_Points[i].m_IsVisible = false;
        }
    }

    public Vector3 GetSuitableSpawnPosition()
    {
        var currentGrid = m_Points[UnityEngine.Random.Range(0, m_Points.Length)];
        while(currentGrid.m_Points == null || currentGrid.m_Points.Count == 0|| currentGrid.m_PlayerInside)
        {
            currentGrid = m_Points[UnityEngine.Random.Range(0, m_Points.Length)];
        }

        var currentPoint = currentGrid.m_Points[UnityEngine.Random.Range(0, currentGrid.m_Points.Count)];
        while(currentPoint.m_IsVisible)
        {
            currentPoint = currentGrid.m_Points[UnityEngine.Random.Range(0, currentGrid.m_Points.Count)];
        }

        return currentPoint.m_Position;
    }
}
