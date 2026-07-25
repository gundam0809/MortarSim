using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI explosionCountText;
    public string bulletTag = "Bullet"; // tag your bullet prefab with this

    private int lastKnownBulletCount = 0;
    private int totalExplosions = 0;

    private void Start()
    {
        lastKnownBulletCount = GameObject.FindGameObjectsWithTag(bulletTag).Length;
        UpdateText();
    }

    private void Update()
    {
        int currentBulletCount = GameObject.FindGameObjectsWithTag(bulletTag).Length;

        // If bullets increased since last frame, update baseline (new bullet fired)
        if (currentBulletCount > lastKnownBulletCount)
        {
            lastKnownBulletCount = currentBulletCount;
        }
        // If a bullet disappeared, count it as an explosion
        else if (currentBulletCount < lastKnownBulletCount)
        {
            totalExplosions += (lastKnownBulletCount - currentBulletCount);
            lastKnownBulletCount = currentBulletCount;
            UpdateText();
        }
    }

    private void UpdateText()
    {
        if (explosionCountText != null)
            explosionCountText.text = "Explosions: " + totalExplosions;
    }
}
