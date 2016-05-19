using UnityEngine;
using System.Collections;
[RequireComponent(typeof(SpriteRenderer))]

public class AutoStretchSprite : MonoBehaviour {

	/// <summary>
	/// Resize the attached sprite according to the camera view
	/// </summary>
	public void Resize()
	{
		SpriteRenderer sr = GetComponent<SpriteRenderer>();

		float width = sr.sprite.bounds.size.x;
		float height = sr.sprite.bounds.size.y;

		var worldScreenHeight = Camera.main.orthographicSize * 2f;
		var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

		var imgScale = new Vector3(1f, 1f, 1f);
		var ratio = new Vector2(width / height, height / width);

		imgScale.y = worldScreenHeight / height;
		imgScale.x = imgScale.y * ratio.x;             

		transform.localScale = imgScale;
	}
}