using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace MBS{
	public class WUScoringUGUI : MonoBehaviour {
		
		static public WUScoringUGUI SpawnInstance(Canvas canvas = null)
		{
			if (null == canvas)
				canvas = FindObjectOfType<Canvas>();
			if (null == canvas) 
				return null;
			
			WUScoringUGUI result = Instantiate(Resources.Load<WUScoringUGUI>("WUScoringUGUI")) as WUScoringUGUI;
			if (null != result)
				result.transform.SetParent(canvas.transform, false);

			return result;
		}
		
		public WUScoringDisplayName		display_name;	// choose wether you wish to show the display name or the nickname in the high scores table
		public WUScoringAgeRating		age_rating;     // set to match your game's rating to make sure avatars are appropriate for your audience
        public System.Action            onWindowClosed;

        [SerializeField] Text			header_text;
		[SerializeField] GameObject		sliding_panel_parent;
        [SerializeField] int
            avatar_size = 32,
            number_of_scores_to_show = 20,
            hard_capped_display_size = 32;

        public int NumberOfScoresToShow => number_of_scores_to_show;

        //OnEnabled is called before Start so to make this work in the inspector or when working offline, put this code
        //in OnEnabled. If you do this in Start then you will only be registered to listen for the event AFTER it's fired
        //and thus you will have missed it and just have an empty screen to stare at...
        void OnEnable() => WUScoring.onFetched += OnFetched;		
		void OnDisable () => WUScoring.onFetched -= OnFetched;		
		
		public void HideScores()
		{
			onWindowClosed?.Invoke();
			onWindowClosed = null;			
			Destroy(gameObject);
		}
		
		/// <summary>
		/// This will loop through all the returned entries and spawn a prefab that will list their details, including their icon
		/// </summary>
		public void OnFetched(CML results)
		{
			List<CMLData> entries = results.AllNodesOfType("person");
			if (null == entries) 
				return;
			
			string name_to_display = display_name == WUScoringDisplayName.DisplayName ? "dname"	: "nname";
			
			if (avatar_size < 0)
				avatar_size = 32;
			else if (avatar_size > 512)
				avatar_size = 512;
			GridLayoutGroup glg = GetComponentInChildren<GridLayoutGroup>();
			glg.cellSize = new Vector2(glg.cellSize.x, avatar_size < hard_capped_display_size ? avatar_size : hard_capped_display_size);
			RectTransform recttransform = glg.GetComponent<RectTransform>();
			recttransform.sizeDelta = new Vector2(recttransform.sizeDelta.x, entries.Count * (glg.cellSize.y + glg.spacing.y));
			
			foreach(CMLData entry in entries)
			{
				WUScoreboardEntryUGUI.SpawnInstance(
					parent: recttransform,
					person: entry,
					name: name_to_display,
					age_rating: age_rating, 
					avatar_size: avatar_size,
                    display_cap: hard_capped_display_size);
			}
		}
		
	}
}