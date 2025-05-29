namespace DataAccess.Service
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DataAccess.Model.AdminDashboard;
    using DataAccess.AutoChecker;
    using Newtonsoft.Json;
    using DataAccess.Service.Interfaces;
    using DataAccess.Service.Components;
    using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
    using WinUiApp.Data.Data;

    public class CheckersService : ICheckersService
    {
        private IReviewService reviewsService;
        private IAutoCheck autoCheck;

        public CheckersService(IReviewService reviewsService, IAutoCheck autoCheck)
        {
            this.reviewsService = reviewsService;
            this.autoCheck = autoCheck;
        }

        public async Task<List<string>> RunAutoCheck(List<Review> receivedReviews)
        {
            if (receivedReviews == null)
            {
                return new List<string>();
            }

            List<string> checkingMessages = new List<string>();

            try
            {
                foreach (Review currentReview in receivedReviews)
                {
                    if (currentReview?.Content == null)
                    {
                        continue;
                    }

                    bool reviewIsOffensive = await autoCheck.AutoCheckReview(currentReview.Content);
                    if (reviewIsOffensive)
                    {
                        checkingMessages.Add($"Review {currentReview.ReviewId} is offensive. Hiding the review.");
                        await reviewsService.HideReview(currentReview.ReviewId);
                        await reviewsService.ResetReviewFlags(currentReview.ReviewId);
                    }
                    else
                    {
                        checkingMessages.Add($"Review {currentReview.ReviewId} is not offensive.");
                    }
                }

                return checkingMessages;
            }
            catch
            {
                return new List<string>();
            }
        }

        public async Task<HashSet<string>> GetOffensiveWordsList()
        {
            try
            {
                return await autoCheck.GetOffensiveWordsList();
            }
            catch
            {
                return new HashSet<string>();
            }
        }

        public async Task AddOffensiveWordAsync(string newWord)
        {
            if (string.IsNullOrWhiteSpace(newWord))
            {
                return;
            }

            if (autoCheck == null)
            {
                return;
            }

            try
            {
                await autoCheck.AddOffensiveWordAsync(newWord);
            }
            catch
            {
            }
        }

        public async Task DeleteOffensiveWordAsync(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return;
            }

            if (autoCheck == null)
            {
                return;
            }

            try
            {
                await autoCheck.DeleteOffensiveWordAsync(word);
            }
            catch
            {
            }
        }

        public void RunAICheckForOneReviewAsync(Review review)
        {
            if (review?.Content == null)
            {
                return;
            }

            try
            {
                bool reviewIsOffensive = CheckReviewWithAI(review);
                if (!reviewIsOffensive)
                {
                    return;
                }
                reviewsService.HideReview(review.ReviewId);
                reviewsService.ResetReviewFlags(review.ReviewId);
            }
            catch
            {
            }
        }

        private static bool CheckReviewWithAI(Review review)
        {
            if (review?.Content == null)
            {
                return false;
            }

            try
            {
                string response = OffensiveTextDetector.DetectOffensiveContent(review.Content);

                if (string.IsNullOrEmpty(response) || response.StartsWith("Error:") || response.StartsWith("Exception:"))
                {
                    return false;
                }

                List<List<Dictionary<string, object>>>? arrayResults = JsonConvert.DeserializeObject<List<List<Dictionary<string, object>>>>(response);

                if (arrayResults == null)
                {
                    return false;
                }

                if (arrayResults?.Count > 0 && arrayResults[0]?.Count > 0)
                {
                    Dictionary<string, object> prediction = arrayResults[0][0];

                    // This if is diabolical
                    if (prediction != null &&
                        prediction.TryGetValue("label", out object labelObj) &&
                        prediction.TryGetValue("score", out object scoreObj) &&
                        labelObj != null && scoreObj != null)
                    {
                        string label = labelObj.ToString().ToLower();
                        if (double.TryParse(scoreObj.ToString(), out double score))
                        {
                            if (label == "offensive" && score > 0.6)
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}