using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Tween 
{
    public static void FadeImage(Image image, Color targetColor,float speed, float delay = 0, MonoBehaviour monoBehaviour = null)
    {
        if(image == null)
            return;
        
        monoBehaviour.StartCoroutine(FadeImageColor(image, targetColor, speed, delay));
    }

    public static void Move(Transform piece, Vector3 targetPosition, float speed, float delay = 0, MonoBehaviour monoBehaviour = null)
    {
        if(piece == null)
            return;
        monoBehaviour.StartCoroutine(MovePiece( piece, targetPosition, speed, delay));
    }
    
    public static void MoveParticle(RectTransform piece, Vector3 targetPosition, float speed, float delay = 0, MonoBehaviour monoBehaviour = null)
    {
        if(piece == null)
            return;
        monoBehaviour.StartCoroutine(MoveParticlePiece( piece, targetPosition, speed, delay));
    }
        
    private static IEnumerator MovePiece(Transform pieceTransform, Vector3 targetPos, float duration, float delay = 0, MonoBehaviour monoBehaviour = null)
    {
        if (delay > 0) {
            yield return new WaitForSeconds(delay) ; 
        }
        float elapsedTime = 0;
        Vector3 startPos = pieceTransform.localPosition;
        while (elapsedTime <= duration) {
            pieceTransform.localPosition = Lerp(startPos, targetPos, Back.Out(elapsedTime / duration));				
            yield return null; 
            elapsedTime += Time.deltaTime;                
        }
        pieceTransform.localPosition = targetPos;
    }
    private static IEnumerator MoveParticlePiece(RectTransform pieceTransform, Vector3 targetPos, float duration, float delay = 0)
    {
        if (delay > 0) {
            yield return new WaitForSeconds(delay) ; 
        }
        float elapsedTime = 0;
        Vector3 startPos = pieceTransform.anchoredPosition;
        while (elapsedTime <= duration) {
            pieceTransform.anchoredPosition = Lerp(startPos, targetPos, Back.In(elapsedTime / duration));				
            yield return null; 
            elapsedTime += Time.deltaTime;                
        }
        pieceTransform.anchoredPosition = new Vector2(0,1000);
    }

    private static IEnumerator FadeImageColor(Image image, Color targetColor, float duration, float delay = 0)
    { 
        if (delay > 0) {
            yield return new WaitForSeconds(delay) ; 
        }
        float elapsedTime = 0;
        Color startColor = image.color;
        while (elapsedTime <= duration) {
            image.color = Color.Lerp(startColor, targetColor, Back.Out(elapsedTime / duration));				
            yield return null; 
            elapsedTime += Time.deltaTime;                
        }

        image.color = targetColor;
    }

    private static Vector3 Lerp(Vector3 startValue, Vector3 endValue, float t)
    {
        return new Vector3(
            startValue.x + (endValue.x - startValue.x) * t,
            startValue.y + (endValue.y - startValue.y) * t,
            startValue.z + (endValue.z - startValue.z) * t
        );
    }
    
}

//Reference from Robert Penner’s easing functions,
public static class Back
{
    private const float S = 1.30158f;
    private const float S2 = 2.5949095f;

    public static float In (float k) {
        return k*k*((S + 1f)*k - S);
    }
		
    public static float Out (float k) {
        return (k -= 1f)*k*((S + 1f)*k + S) + 1f;
    }
		
    public static float InOut (float k) {
        if ((k *= 2f) < 1f) return 0.5f*(k*k*((S2 + 1f)*k - S2));
        return 0.5f*((k -= 2f)*k*((S2 + 1f)*k + S2) + 2f);
    }		
};

public static class Sinusoidal
{		
    public static float In (float k) {
        return 1f - Mathf.Cos(k*Mathf.PI/2f);
    }
		
    public static float Out (float k) {
        return Mathf.Sin(k*Mathf.PI/2f);
    }
		
    public static float InOut (float k) {
        return 0.5f*(1f - Mathf.Cos(Mathf.PI*k));
    }		
};

public class Circular
{		
    public static float In (float k) {
        return 1f - Mathf.Sqrt(1f - k*k);
    }
		
    public static float Out (float k) {
        return Mathf.Sqrt(1f - ((k -= 1f)*k));
    }
		
    public static float InOut (float k) {
        if ((k *= 2f) < 1f) return -0.5f*(Mathf.Sqrt(1f - k*k) - 1);
        return 0.5f*(Mathf.Sqrt(1f - (k -= 2f)*k) + 1f);
    }		
};
