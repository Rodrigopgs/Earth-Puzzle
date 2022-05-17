using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class LevelTransition : MonoBehaviour
{

    public string sceneName;

    int players;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger)
            return;

        OldPlayerController c = collision.GetComponent<OldPlayerController>();
        if (c != null)
        {
            players++;

            if (players >= 2)
            {
                Destroy(Player1Interactions.Instance);
                Destroy(Player2Interactions.Instance);
                StartCoroutine(LoadScene());
            }

        }
    }

    private IEnumerator LoadScene()
    {
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene(sceneName);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.isTrigger)
            return;

        OldPlayerController c = collision.GetComponent<OldPlayerController>();
        if (c != null)
            players--;
    }
}
