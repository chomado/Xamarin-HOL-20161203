using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;

namespace DevDaysSpeakers.iOS
{
	public class EmotionService
	{
		private static async Task<Emotion[]> GetHappinessAsync(string url)
		{
			var client = new HttpClient();
			var emotionClient = new EmotionServiceClient("INSERT_EMOTION_SERVICE_KEY_HERE");

			var emotionResults = await emotionClient.RecognizeAsync(url);

			if (emotionResults == null || emotionResults.Count() == 0)
			{
				throw new Exception("Can't detect face");
			}

			return emotionResults;
		}

		//複数の被検対象が存在する場合の平均幸福度算出
		public static async Task<float> GetAverageHappinessScoreAsync(string url)
		{
			Emotion[] emotionResults = await GetHappinessAsync(url);

			float score = 0;
			foreach (var emotionResult in emotionResults)
			{
				score = score + emotionResult.Scores.Happiness;
			}

			return score / emotionResults.Count();
		}

		public static string GetHappinessMessage(float score)
		{
			score = score * 100;
			double result = Math.Round(score, 2);

			if (score >= 50)
				return result + " % ヽ（ヽ *ﾟ▽ﾟ*）ノ";
			else
				return result + "% （；＿；）";
		}
	}
}
