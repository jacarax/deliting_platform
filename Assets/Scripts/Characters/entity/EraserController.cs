
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LinearMovement))]
public class EraserController : MonoBehaviour
{
    [Header("Настройки обнаружения")]

    Vector2 direction = Vector2.right;   // Направление

    [SerializeField] private LayerMask platformLayer;     // Слой платформ
    [SerializeField] private ParticleSystem eraseEffect;  // Эффект стирания
    [SerializeField] private float radius = 1f;
    private ErasablePlatform[] currentErasingPlatforms = new ErasablePlatform[0]; // Массив текущих стираемых платформ

    private void FixedUpdate()
    {
        DetectPlatforms();
    }

    private void DetectPlatforms()
    {
        // Находим ВСЕ платформы в радиусе
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            radius,
            platformLayer
        );

        // Останавливаем стирание платформ, которые больше не в радиусе
        foreach (var platform in currentErasingPlatforms)
        {
            if (platform != null && !IsPlatformInHits(platform, hits))
            {
                platform.StopErasing();
            }
        }

        // Начинаем стирание новых платформ
        List<ErasablePlatform> newErasingPlatforms = new List<ErasablePlatform>();
        foreach (var hit in hits)
        {
            ErasablePlatform platform = hit.GetComponent<ErasablePlatform>();
            if (platform != null)
            {
                // Если платформа ещё не стирается, начинаем стирание
                if (!IsPlatformCurrentlyErasing(platform))
                {
                    platform.StartErasing(hit.ClosestPoint(transform.position));
                    SpawnEraseEffect(hit.ClosestPoint(transform.position));
                }
                newErasingPlatforms.Add(platform);
            }
        }

        // Обновляем список текущих стираемых платформ
        currentErasingPlatforms = newErasingPlatforms.ToArray();
    }

    // Проверяет, есть ли платформа в текущем списке стираемых
    private bool IsPlatformCurrentlyErasing(ErasablePlatform platform)
    {
        foreach (var p in currentErasingPlatforms)
        {
            if (p == platform) return true;
        }
        return false;
    }

    // Проверяет, есть ли платформа в новом списке коллайдеров
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
        // Визуализация области стирания
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif
}