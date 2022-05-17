using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider2D))]
public class Inspectable : Interactable
{
    public const float FadeDistance = 0.05f;

    [Tooltip("The font size the text will attempt to be.")]
    public float maxFontSize = 2.5f;
    [Tooltip("How long the text is visible for, in seconds.")]
    public float lifetime = 4f;
    [Min(float.Epsilon), Tooltip("How long the text will take to appear, in seconds.")]
    public float fadeTime = 0.25f;
    [Tooltip("The layer the text will appear on.")]
    public int sortingLayer = 1000000000;
    [Tooltip("The offset of where the text will appear.")]
    public Vector2 offset = Vector2.zero;
    [Tooltip("How big the text box will be.")]
    public Vector2 size = new Vector2(2f, 1f);
    [Tooltip("The color the text will be appear in.")]
    public Color color = Color.white;
    [TextArea, Tooltip("The text that will be rendered.")]
    public string text;

    private InspectTextFader textObject;

    public override void OnInteract(int playerNumber)
    {
        GameObject tempobject = new GameObject("inspectText");
        tempobject.transform.position = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y + (1 - FadeDistance), transform.position.z);

        textObject = tempobject.AddComponent<InspectTextFader>();
        TextMeshPro tmp = tempobject.AddComponent<TextMeshPro>();

        tmp.rectTransform.sizeDelta = size;

        tmp.fontSize = maxFontSize;
        tmp.enableAutoSizing = true;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontSizeMin = 0.25f;
        tmp.fontSizeMin = maxFontSize;
        tmp.sortingOrder = 1;
        tmp.color = color;
        tmp.sortingOrder = sortingLayer;

        tmp.text = text;

        textObject.tmp = tmp;
        textObject.readTime = lifetime;
        textObject.fadeTime = fadeTime;

        Player1Interactions.Instance.UpdateInteractables();
        Player2Interactions.Instance.UpdateInteractables();
    }

    public override bool Conditions(GameObject from) => textObject == null;

    private class InspectTextFader : MonoBehaviour
    {

        bool fadeIn = true;
        bool fadeOut = false;
        float delta;

        public float readTime;
        public float fadeTime;

        public TextMeshPro tmp;

        private void Start()
        {
            tmp.alpha = 0;
        }

        private void Update()
        {
            if (fadeIn)
            {
                tmp.alpha = Mathf.Clamp01(delta / fadeTime);
                transform.position += transform.up * (FadeDistance / fadeTime * Time.deltaTime);
            }

            if (fadeIn && delta >= fadeTime)
            {
                fadeIn = false;
                delta = 0;
            }

            if (fadeOut)
            {
                tmp.alpha = Mathf.Clamp01(1 - (delta / fadeTime));
                transform.position += transform.up * (FadeDistance / fadeTime * Time.deltaTime);
            }

            if (delta >= readTime && !fadeIn && !fadeOut)
            {
                fadeOut = true;
                delta = 0;
            }

            if (fadeOut && delta >= fadeTime)
                Destroy(gameObject);

            delta += Time.deltaTime;
        }
    }

}
