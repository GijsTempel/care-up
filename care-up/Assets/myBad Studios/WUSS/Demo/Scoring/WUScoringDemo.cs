using UnityEngine;
using UnityEngine.UI;

namespace MBS {
	public class WUScoringDemo : MonoBehaviour {

		public Text
			Message;

		public GameObject[]
			demo_buttons;

		public Canvas
			canvas;

		WUScoringUGUI 
			scores;

		public InputField id_input;
		int GameIdi { get { int result = 0; if (int.TryParse(id_input.text, out result)) return result; return 1; } }

		/// <summary>
		/// If there is no login component in the scene, quit.
		/// If there is then delete the on screen message, show the demo buttons and
		/// configure WUScoring to automatically fetch the high scores after a
		/// high score has been submitted. (Optional, of course)
		/// </summary>
		void Start () {
			if (null != FindObjectOfType<WUUGLoginGUI>())
			{
				Destroy (Message.gameObject);
				WULogin.onLoggedIn += ShowButtons;
				WUScoring.onSubmitted += AutoFetchScores;
			} 
			else
				Message.text = "This demo scene requires the WULoginUGUI prefab to be in the scene.\nPlease add it to the scene";
		}

		/// <summary>
		/// The feature to auto fetch high scores is attached to a static function.
		/// It's always a good idea to clean up event responders when you don't need them
		/// any more but it's an even better idea to make SURE you do so with static events!
		/// </summary>
		void OnDestroy() =>	WUScoring.onSubmitted -= AutoFetchScores;
		

		/// <summary>
		/// This gets triggered by a successful score submission
		/// This fetches the high scores
		/// </summary>
		void AutoFetchScores(CML data) => FetchScores();
		
		/// <summary>
		/// This gets triggered after a successful login.
		/// The demo starts with the buttons hidden. This just shows them
		/// </summary>
		void ShowButtons(CML response) => ShowDemoButtons();
		
		/// <summary>
		/// Shows the demo buttons.
		/// </summary>
		void ShowDemoButtons()
		{
			for ( int i = 0; i < demo_buttons.Length; i++)
				demo_buttons[i].SetActive(true);
		}

		/// <summary>
		/// Hides the demo buttons.
		/// </summary>
		void HideDemoButtons()
		{
			for ( int i = 0; i < demo_buttons.Length; i++)
				demo_buttons[i].SetActive(false);
		}

		/// <summary>
		/// Submits a random score.
		/// Successful submission will trigger FetchScores
		/// </summary>
		public void SubmitRandomScore()
		{
			HideDemoButtons();
			int my_score = Random.Range(1, 10000);
			Debug.Log("Submitting score: "+ my_score);
			WUScoring.SubmitScore(my_score, GameIdi);
		}

		/// <summary>
		/// This hides the demo buttons to prevent multiple clicking.
		/// Spawn the high score window and request the high scores from the server.
		/// also set up the button on the high scores window to show the demo buttons again
		/// </summary>
		public void FetchScores()
		{
			HideDemoButtons();
			scores = WUScoringUGUI.SpawnInstance(canvas);
			scores.onWindowClosed += ShowDemoButtons;
			WUScoring.FetchScores(scores.NumberOfScoresToShow, GameIdi);
		}
	
	}
}
