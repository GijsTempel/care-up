using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MBS {
	public class WUScoreboardEntryUGUI : MonoBehaviour {
		
		static public WUScoreboardEntryUGUI SpawnInstance(Transform parent, CMLData person, string name, WUScoringAgeRating age_rating = WUScoringAgeRating.PG, int avatar_size = 32, int display_cap = 512)
		{
			WUScoreboardEntryUGUI result = Instantiate(Resources.Load<WUScoreboardEntryUGUI>("WUScoreEntryPrefab")) as WUScoreboardEntryUGUI;
			if (null != result)
			{
				result.transform.SetParent(parent, false);
				result.age_rating = age_rating;
				result.avatar_size = avatar_size;
				
				result.gravatar = person.String("gravatar");
				result.name_text.text = person.String(name);
				result.score_text.text = person.String("score");
                result.hard_cap_display_size = display_cap;
				
				result.score_text.color = result.name_text.color = person.Bool("highlight") ? result.highlighted_color : result.normal_color;
			}
			return result;
		}
		
		public Color
			normal_color = Color.white,
			highlighted_color = Color.yellow;
		
		public Text 
			score_text,
			name_text;
		
		public Image
			icon;
		
		public WULGravatarTypes avatartype;
        int hard_cap_display_size = 32;
		
		WUScoringAgeRating age_rating = WUScoringAgeRating.PG;
		int avatar_size = 32;
		string gravatar;
		
		IEnumerator Start()
		{
			icon.color = new Color(1f,1f,1f,0f);
            string URL = $"http://www.gravatar.com/avatar/{gravatar}?r={age_rating}&s={avatar_size}&d={avatartype.ToString().ToLower()}";
#pragma warning disable
            WWW w = new WWW(URL);
#pragma warning restore
            yield return w;
			if (w.error == null)
			{
                int display_size = hard_cap_display_size < avatar_size ? hard_cap_display_size : avatar_size;
				Texture2D avatar = new Texture2D(1,1);
				avatar = w.texture; 
				Sprite avatar_s = Sprite.Create(avatar,new Rect(0,0,avatar.width,avatar.height), new Vector2(0f,0.5f));
				icon.rectTransform.sizeDelta = new Vector2(display_size, display_size);
				icon.rectTransform.anchoredPosition = new Vector2(0, display_size / 2f);
				icon.sprite = avatar_s;
				icon.color = Color.white;
			}
		}
	}
}