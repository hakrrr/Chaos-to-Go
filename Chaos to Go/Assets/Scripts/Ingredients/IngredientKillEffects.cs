using UnityEngine;

public class IngredientKillEffects : MonoBehaviour
{
    public GameObject tomatoDeathEffect;
    public GameObject onionDeathEffect;
    public GameObject asparagusDeathEffect;
    public GameObject chickenDeathEffect;
    public GameObject carrotDeathEffect;

    private static GameObject[] effects;

    void Start()
    {
        effects = new GameObject[]
        {
            asparagusDeathEffect,
            carrotDeathEffect,
            chickenDeathEffect,
            onionDeathEffect,
            tomatoDeathEffect
        };
    }


    public static void PlayEffect(GameObject effectPrefab, Vector3 position, Transform root)
    {
        GameObject effect = Instantiate(effectPrefab);
        effect.name = effectPrefab.name + "DeathEffect";
        effect.transform.parent = root;
        effect.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        effect.transform.position = position;
    }


    public static void PlayEffect(int effectID, Vector3 position, Transform root)
    {
        GameObject effect = Instantiate(effects[effectID]);
        effect.name = effects[effectID].name + "DeathEffect";
        effect.transform.parent = root;
        effect.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        effect.transform.position = position;
    }
}
