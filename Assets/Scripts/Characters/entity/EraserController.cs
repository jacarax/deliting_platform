
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LinearMovement))]
public class EraserController : MonoBehaviour
{
    [Header("��������� �����������")]

    Vector2 direction = Vector2.right;   // �����������

    [SerializeField] private LayerMask platformLayer;     // ���� ��������
    [SerializeField] private ParticleSystem eraseEffect;  // ������ ��������
    [SerializeField] private float radius = 1f;
    private ErasablePlatform[] currentErasingPlatforms = new ErasablePlatform[0]; // ������ ������� ��������� ��������

    private void FixedUpdate()
    {
        DetectPlatforms();
    }

    private void DetectPlatforms()
    {
        // ������� ��� ��������� � �������
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            radius,
            platformLayer
        );

        // ������������� �������� ��������, ������� ������ �� � �������
        foreach (var platform in currentErasingPlatforms)
        {
            if (platform != null && !IsPlatformInHits(platform, hits))
            {
                platform.StopErasing();
            }
        }

        // �������� �������� ����� ��������
        List<ErasablePlatform> newErasingPlatforms = new List<ErasablePlatform>();
        foreach (var hit in hits)
        {
            ErasablePlatform platform = hit.GetComponent<ErasablePlatform>();
            if (platform != null)
            {
                // ���� ��������� ��� �� ���������, �������� ��������
                if (!IsPlatformCurrentlyErasing(platform))
                {
                    platform.StartErasing(hit.ClosestPoint(transform.position));
                    SpawnEraseEffect(hit.ClosestPoint(transform.position));
                }
                newErasingPlatforms.Add(platform);
            }
        }

        // ��������� ������ ������� ��������� ��������
        currentErasingPlatforms = newErasingPlatforms.ToArray();
    }

    // ���������, ���� �� ��������� � ������� ������ ���������
    private bool IsPlatformCurrentlyErasing(ErasablePlatform platform)
    {
        foreach (var p in currentErasingPlatforms)
        {
            if (p == platform) return true;
        }
        return false;
    }

    // ���������, ���� �� ��������� � ����� ������ �����������
    private bool IsPlatformInHits(ErasablePlatform platform, Collider2D[] hits)
    {
        foreach (var hit in hits)
        {
            if (hit.GetComponent<ErasablePlatform>() == platform)
                return true;
        }
        return false;
    }

    private void SpawnEraseEffect(Vector2 position)
    {
        if (eraseEffect != null)
        {
            Instantiate(eraseEffect, position, Quaternion.identity);
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // ������������ ������� ��������
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif
}