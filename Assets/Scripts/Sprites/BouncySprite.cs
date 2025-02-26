using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncySprite : MonoBehaviour
{
    public float minDistanceForBounce = .05f;

    private float targetElevation, sourceElevation;
    private Quaternion targetRotation, sourceRotation;

    public float minBounceElevation;
    public float maxBounceElevation;
    public float minBounceAngle;
    public float maxBounceAngle;


    private float currentBounceFactor = 0f;
    public float bounceFadeFactor = 5f;
    public float bounceSpeed = 5f;
    public float rotationSpeed = 25f;

    private float bounceProgress = 0f;
    private bool bounceDirectionUp;
    private float rotationProgress = 0f;
    private bool rotationDirectionMax;
    private bool isDescending;

    private bool isMoving = false;
    public void SetMoving(Vector2 speed)
    {
        isMoving = speed.magnitude > minDistanceForBounce;
    }

    void Update()
    {
        float bouncePct, rotationPct;
        if (isMoving)
        {
            isDescending = false;
            
            currentBounceFactor = Mathf.Clamp01(currentBounceFactor + Time.deltaTime * bounceFadeFactor);

            if (rotationDirectionMax)
            {
                rotationProgress = Mathf.Clamp(rotationProgress + Time.deltaTime, 0, rotationSpeed);

                targetRotation = Quaternion.Euler(0, 0, minBounceAngle);
                sourceRotation = Quaternion.Euler(0, 0, maxBounceAngle);
                if (rotationProgress == rotationSpeed)
                    rotationDirectionMax = false;
            }
            else
            {
                rotationProgress = Mathf.Clamp(rotationProgress - Time.deltaTime, 0, rotationSpeed);

                targetRotation = Quaternion.Euler(0, 0, minBounceAngle);
                sourceRotation = Quaternion.Euler(0, 0, maxBounceAngle);
                if (rotationProgress == 0f)
                    rotationDirectionMax = true;
            }

            if (bounceDirectionUp)
            {
                bounceProgress = Mathf.Clamp(bounceProgress + Time.deltaTime, 0, bounceSpeed);
                sourceElevation = minBounceElevation;
                targetElevation = maxBounceElevation;

                if (bounceProgress == bounceSpeed)
                {

                    bounceDirectionUp = false;
                }
            }
            else
            {
                bounceProgress = Mathf.Clamp(bounceProgress - Time.deltaTime, 0, bounceSpeed);
                sourceElevation = minBounceElevation;
                targetElevation = maxBounceElevation;

                if (bounceProgress == 0f)
                {

                    bounceDirectionUp = true;
                }
            }
        }
        else
        {
            if (!isDescending)
            {
                isDescending = true;
                sourceElevation = transform.localPosition.y;
                targetElevation = 0f;

                sourceRotation = Quaternion.Euler(0, 0, transform.localRotation.z);
                targetRotation = Quaternion.Euler(0, 0, 0);
                bounceProgress = 0f;
            }

            currentBounceFactor = Mathf.Clamp01(currentBounceFactor - Time.deltaTime * bounceFadeFactor);
            bounceProgress = Mathf.Clamp(bounceProgress + Time.deltaTime, 0, bounceSpeed);
        }

        rotationPct = rotationProgress / rotationSpeed;
        bouncePct = bounceProgress / bounceSpeed;

        var pos = transform.localPosition;
        pos.y = Mathf.Lerp(sourceElevation, targetElevation, bouncePct);
        transform.localPosition = Vector3.Lerp(transform.localPosition, pos, currentBounceFactor);

        var desiredRotation = Quaternion.Slerp(sourceRotation, targetRotation, rotationPct * currentBounceFactor);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, desiredRotation, currentBounceFactor);
    }
}
