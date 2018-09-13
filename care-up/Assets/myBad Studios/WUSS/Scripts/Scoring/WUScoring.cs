using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace MBS {
	public enum WUScoringDisplayName	{DisplayName, Nickname}
	public enum WUScoringAgeRating		{G, PG, R, X}
	public enum WUScoringAction			{SubmitScore, FetchScores, SubmitScoreForUser}
	
	static public class WUScoring {

		static readonly string scoring_filepath = "wuss_scoring/unity_functions.php";
		static public readonly string SCORINGConstant = "SCORING";

		static public Action<CML>
			onSubmitted,	//callbacks you can hook into
			onFetched;
		
		static public Action<CMLData>
			onSubmissionFailed,
			onFetchingFailed;
			
		static public void SubmitScore(int score, int game_id = -1)
		{
			CMLData	data = new CMLData();
			data.Seti ("score", score);
			data.Seti ("gid", game_id);
			WPServer.ContactServer(WUScoringAction.SubmitScore.ToString(), scoring_filepath, SCORINGConstant, data, onSubmitted, onSubmissionFailed);
		}
		
		static public void SubmitScoreForUser(int user, int score, int game_id = -1, Action<CML> onSubmitted = null, Action<CMLData> onSubmissionFailed = null)
		{
			CMLData	data = new CMLData();
			data.Seti ("score", score);
			data.Seti ("gid", game_id);
			data.Seti ("uid", user);
			WPServer.ContactServer(WUScoringAction.SubmitScoreForUser.ToString(), scoring_filepath, SCORINGConstant, data, onSubmitted, onSubmissionFailed);
		}
		
		static public void SubmitScoreForUsername(string user, int score, int game_id = -1, Action<CML> onSubmitted = null, Action<CMLData> onSubmissionFailed = null)
		{
			CMLData	data = new CMLData();
			data.Seti ("score", score);
			data.Seti ("gid", game_id);
			data.Set  ("username", user);
			WPServer.ContactServer(WUScoringAction.SubmitScoreForUser.ToString(), scoring_filepath, SCORINGConstant, data, onSubmitted, onSubmissionFailed);
		}
		
		static public void FetchScores(int limit = 0, int game_id = -1)
		{
			CMLData	data = new CMLData();
			data.Seti("limit", limit);
			data.Seti("gid", game_id);
			WPServer.ContactServer(WUScoringAction.FetchScores.ToString(), scoring_filepath, SCORINGConstant, data, onFetched, onFetchingFailed);
		}
	}
	
}
