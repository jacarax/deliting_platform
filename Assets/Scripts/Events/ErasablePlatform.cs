using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ErasablePlatform : MonoBehaviour
{
    [Header("��������� ��������")]
    [SerializeField] private float eraseSpeed = 0.5f; // �������� ��������
    [SerializeField] private float minWidth = 0.1f; // ����������� ����� ����� ���������
    private float initialWidth;
    private float currentErasedAmount = 0f;


    private int flip;
    private SpriteRenderer spriteRenderer;
  
    private bool isErasing;
    private int eraseDirection; // 1 = �������� �� ������� pivot, -1 = �������� ������ �� pivot
    private Vector2 initialPosition;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialPosition = transform.position;
       
    }

    public void StartErasing(Vector2 contactPoint)
    {
        // ���������� ����������� �������� (� ����� ������� �������)
        float relativeContactX = Math.Abs(contactPoint.x - transform.position.x);
        eraseDirection = relativeContactX < 1 ? 1 : -1;
        if (transform.rotation.eulerAngles.y == 180f)
            flip = -1;
        else flip = 1;
        isErasing = true;
        //spriteRenderer.color = new Color(1f, 0.7f, 0.7f);
        initialWidth = spriteRenderer.size.x;
        currentErasedAmount = 0f;
    }

    public void StopErasing()
    {
        isErasing = false;
        //spriteRenderer.color = Color.white;
    }
    private void UpdatePosition()
    {
        // ������� ������� ��� ������� �������� � ����� �������
        transform.position = new Vector2(
            initialPosition.x + flip * transform.localScale.x,//+ offset,
            initialPosition.y
        );
        // ��� ����� (������ ��� 2D):
        if (flip == -1)
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        else
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        
    }
    private void UpdateCollider()
    {
        // ������� ����� ������� � ���� �����
        float eraseThisFrame = eraseSpeed * Time.deltaTime;
        currentErasedAmount += eraseThisFrame;

        // ��������� ����� ������ (�� ������ minWidth)
        float newWidth = Mathf.Max(initialWidth - currentErasedAmount, minWidth);
        spriteRenderer.size = new Vector2(newWidth, spriteRenderer.size.y);

        if (newWidth <= minWidth)
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if (!isErasing) return;
        if(eraseDirection == 1 )
            UpdatePosition();
        UpdateCollider();

    }
}