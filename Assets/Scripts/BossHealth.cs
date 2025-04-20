using System.Diagnostics.Tracing;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] Transition transition;
    private bool transitioning = false;

    private float currentHealth;
    private bool isDead = false;

    public ParticleSystem explosion;
    private float timer = 0;

    [System.Serializable]
    public class UserData
    {
        public int pSpeed;
        public int pHealth;
        public int pSScrap;
        public int parts;
    }
    private string filePath;

    void Start()
    {
        filePath = Application.persistentDataPath + "/userdata.json";
        currentHealth = maxHealth;
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            timer += Time.fixedDeltaTime;
            if (timer > 1){
                if (!transitioning) transition.TransitionScene("WinScene");
                transitioning = true;
            }
        }
    }

    public void SaveData()
    {
        UserData data = new UserData();
        int.TryParse(StaticData.startingScrap.ToString(), out data.pSScrap);
        int.TryParse(StaticData.speedIncrease.ToString(), out data.pSpeed);
        int.TryParse(StaticData.healthIncrease.ToString(), out data.pHealth);
        int.TryParse(StaticData.parts.ToString(), out data.parts);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);

    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        Debug.Log("Boss took " + damageAmount + " of damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            TriggerDeath();
        }
    }

    private void TriggerDeath()
    {
        if (!isDead)
        {
            isDead = true;
            Debug.Log("Boss is dead");
            explosion.Play();

            StaticData.parts += (StaticData.scrap + 100) / 10;
            SaveData();
        }

    }
    public float GetHealth() => currentHealth;
}
