using UnityEngine;

public class DestructionReciever : MonoBehaviour
{

    public float destroyTime = 2f;

    public Color destroyColor = new Color(252f / 255f, 94f / 255f, 3f / 255f);

    bool destroying;
    float time;

    public virtual void Destroy()
    {
        destroying = true;
    }

    public virtual void Heal()
    {
        destroying = false;
    }

    Color startColor;

    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
    }

    private void Update()
    {
        if (destroying)
        {
            time = Mathf.Clamp01(time + (Time.deltaTime / destroyTime));
        }
        else
        {
            time = Mathf.Clamp01(time - (Time.deltaTime / destroyTime));
        }

        spriteRenderer.color = Color.Lerp(startColor, destroyColor, time);

        if (time >= 1)
            Destroy(gameObject);

    }

}
