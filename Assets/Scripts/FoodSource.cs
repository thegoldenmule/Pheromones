using UnityEngine;

public class FoodSource : MonoBehaviour
{
    public float Radius;
    public float Value;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position,
            Radius);
    }
}