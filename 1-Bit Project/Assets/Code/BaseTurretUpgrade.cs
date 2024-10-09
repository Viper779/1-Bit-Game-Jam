using System.Collections;
using UnityEngine;

public class BaseTurretUpgrade : MonoBehaviour
{
    public int currentTier = 0;
    public float yMove = 0.34f;
    public float buildSpeed = 0.5f;
    private bool isMoving = false;  // Flag to prevent multiple coroutine calls

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = new Vector3(-16, -4.5f, 0); // Reset to initial position
        currentTier = 0;
        UpgradeManager.towerTier = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            if (UpgradeManager.towerTier > currentTier)
            {
                StartCoroutine(MoveUp());
            }
            else if (UpgradeManager.towerTier < currentTier)
            {
                StartCoroutine(MoveDown());
            }
        }

        if (TurretHealth.isDestroyed)
        {
            currentTier = 0;
            UpgradeManager.towerTier = 0;
        }
    }

    private IEnumerator MoveUp()
    {
        isMoving = true;  // Set flag to true to prevent multiple moves

        for (int i = 0; i < 5; i++)
        {
            gameObject.transform.Translate(new Vector3(0, yMove, 0)); // Move up
            yield return new WaitForSeconds(buildSpeed);
        }
        currentTier++;
        isMoving = false;  // Reset flag when done
    }

    private IEnumerator MoveDown()
    {
        isMoving = true;  // Set flag to true to prevent multiple moves

        for (int i = 0; i < 5; i++)
        {
            gameObject.transform.Translate(new Vector3(0, -yMove, 0)); // Move down
            yield return new WaitForSeconds(buildSpeed);
        }
        currentTier--;
        isMoving = false;  // Reset flag when done
    }
}
