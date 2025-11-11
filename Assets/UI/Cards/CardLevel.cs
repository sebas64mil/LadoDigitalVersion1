using UnityEngine;

public class CardLevel : MonoBehaviour
{
    public Vector3 posDissable;
    public Vector3 posActive;
    public float moveSpeed = 10f; // Velocidad de movimiento

    private bool isMoving = false;
    private Vector3 targetPos;
    private bool deactivateOnArrive = false;

    void Start()
    {
        // Al iniciar, coloca la carta en la posición local dissable
        transform.localPosition = posDissable;
    }

    void Update()
    {
        // Si está en movimiento, interpola hacia la posición local objetivo
        if (isMoving)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.localPosition, targetPos) < 0.01f)
            {
                transform.localPosition = targetPos;
                isMoving = false;

                // Desactiva el panel si corresponde
                if (deactivateOnArrive)
                {
                    gameObject.SetActive(false);
                    deactivateOnArrive = false;
                }
            }
        }
    }

    // Método para mostrar la carta (mover a posición local activa)
    public void ShowCard()
    {
        targetPos = posActive;
        isMoving = true;
        deactivateOnArrive = false;
        gameObject.SetActive(true); // Asegura que el panel esté activo
    }

    // Método para ocultar la carta (mover a posición local dissable)
    public void HideCard()
    {
        targetPos = posDissable;
        isMoving = true;
        deactivateOnArrive = true;
    }
}
