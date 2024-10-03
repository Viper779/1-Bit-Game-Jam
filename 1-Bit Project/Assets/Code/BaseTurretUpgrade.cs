using System.Collections;
using UnityEngine;

public class BaseTurretUpgrade : MonoBehaviour
{
    private int currentTier = 0;
    public float yMove = 0.5f;
    public float buildSpeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = new Vector3(16, -4.7f, 0); // Reset to initial position
    }

    // Update is called once per frame
    void Update()
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

    private IEnumerator MoveUp()
    {
        for (int i = 0; i < 5; i++)
        {
            gameObject.transform.Translate(new Vector3(0, yMove, 0)); // Move up
            yield return new WaitForSeconds(buildSpeed);
        }
        currentTier++;
    }

    private IEnumerator MoveDown()
    {
        for (int i = 0; i < 5; i++)
        {
            gameObject.transform.Translate(new Vector3(0, -yMove, 0)); // Move down
            yield return new WaitForSeconds(buildSpeed);
        }
        currentTier--;
    }
}
