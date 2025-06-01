using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using DataAccess.Constants;
using DataAccess.DTOModels;
using DataAccess.Model.AdminDashboard;
using DataAccess.Model.Authentication;
using DataAccess.Service.Interfaces;
using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
using DrinkDb_Auth.ViewModel.AdminDashboard.Components;
using WinUiApp.Data.Data;
using WinUIApp;
using System.Reflection;

namespace DrinkDb_Auth.ViewModel.AdminDashboard
{
    public class DrinkModificationRequestWithUsername
    {
        public DrinkModificationRequestDTO Request { get; set; }
        public string Username { get; set; }
    }

    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly IReviewService reviewsService;
        private readonly IUserService userService;
        private readonly ICheckersService checkersService;
        private readonly IUpgradeRequestsService requestsService;
        private readonly IDrinkModificationRequestService drinkModificationRequestService;

        private ObservableCollection<ReviewDTO> flaggedReviews;
        private ObservableCollection<User> appealsUsers;
        private ObservableCollection<UpgradeRequest> upgradeRequests;
        private ObservableCollection<string> offensiveWords;
        private ObservableCollection<DrinkModificationRequestWithUsername> drinkModificationRequests;
        private User selectedAppealUser;
        private UpgradeRequest selectedUpgradeRequest;
        private ObservableCollection<string> userReviewsFormatted;
        private ObservableCollection<string> userReviewsWithFlags;
        private string userStatusDisplay;
        private string userUpgradeInfo;
        private bool isAppealUserBanned = true;
        private bool isWordListVisible = false;
        private ObservableCollection<User> usersWithHiddenReviews;
        private User selectedUserWithHiddenReviews;

        // Constructor of warnings
        public MainPageViewModel(IReviewService reviewsService, IUserService userService,
            IUpgradeRequestsService upgradeRequestsService, ICheckersService checkersService, 
            IDrinkModificationRequestService drinkModificationRequestService)
        {
            this.reviewsService = reviewsService;
            this.userService = userService;
            this.requestsService = upgradeRequestsService;
            this.checkersService = checkersService;
            this.drinkModificationRequestService = drinkModificationRequestService;

            this.InitializeCommands();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand KeepBanCommand { get; private set; }

        public ICommand AcceptAppealCommand { get; private set; }

        public ICommand CloseAppealCaseCommand { get; private set; }

        public ICommand HandleUpgradeRequestCommand { get; private set; }

        public ICommand ResetReviewFlagsCommand { get; private set; }

        public ICommand HideReviewCommand { get; private set; }

        public ICommand RunAICheckCommand { get; private set; }

        public ICommand RunAutoCheckCommand { get; private set; }

        public ICommand AddOffensiveWordCommand { get; private set; }

        public ICommand DeleteOffensiveWordCommand { get; private set; }

        public ICommand ShowWordListPopupCommand { get; private set; }

        public ICommand HideWordListPopupCommand { get; private set; }

        public ICommand BanUserCommand { get; private set; }

        public ICommand ApproveDrinkModificationCommand { get; private set; }

        public ICommand DenyDrinkModificationCommand { get; private set; }

        public ObservableCollection<ReviewDTO> FlaggedReviews
        {
            get => this.flaggedReviews;
            set
            {
                this.flaggedReviews = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<User> AppealsUsers
        {
            get => this.appealsUsers;
            set
            {
                this.appealsUsers = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<UpgradeRequest> UpgradeRequests
        {
            get => this.upgradeRequests;
            set
            {
                this.upgradeRequests = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<string> OffensiveWords
        {
            get => this.offensiveWords;
            set
            {
                this.offensiveWords = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<DrinkModificationRequestWithUsername> DrinkModificationRequests
        {
            get => this.drinkModificationRequests;
            set
            {
                this.drinkModificationRequests = value;
                this.OnPropertyChanged();
            }
        }

        public User SelectedAppealUser
        {
            get => this.selectedAppealUser;
            set
            {
                this.selectedAppealUser = value;
                if (value != null)
                {
                    _ = LoadUserAppealDetails(value);
                }
                this.OnPropertyChanged();
            }
        }

        public UpgradeRequest SelectedUpgradeRequest
        {
            get => this.selectedUpgradeRequest;
            set
            {
                this.selectedUpgradeRequest = value;
                if (value != null)
                {
                    _ = LoadUpgradeRequestDetails(value);
                }
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<string> UserReviewsFormatted
        {
            get => this.userReviewsFormatted;
            set
            {
                this.userReviewsFormatted = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<string> UserReviewsWithFlags
        {
            get => this.userReviewsWithFlags;
            set
            {
                this.userReviewsWithFlags = value;
                this.OnPropertyChanged();
            }
        }

        public string UserStatusDisplay
        {
            get => this.userStatusDisplay;
            set
            {
                this.userStatusDisplay = value;
                this.OnPropertyChanged();
            }
        }

        public string UserUpgradeInfo
        {
            get => this.userUpgradeInfo;
            set
            {
                this.userUpgradeInfo = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsAppealUserBanned
        {
            get => this.isAppealUserBanned;
            set
            {
                this.isAppealUserBanned = value;
                this.UserStatusDisplay = this.GetUserStatusDisplay(this.SelectedAppealUser, value);
                this.OnPropertyChanged();
            }
        }

        public bool IsWordListVisible
        {
            get => this.isWordListVisible;
            set
            {
                this.isWordListVisible = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<User> UsersWithHiddenReviews
        {
            get => this.usersWithHiddenReviews;
            set
            {
                this.usersWithHiddenReviews = value;
                this.OnPropertyChanged();
            }
        }

        public User SelectedUserWithHiddenReviews
        {
            get => this.selectedUserWithHiddenReviews;
            set
            {
                this.selectedUserWithHiddenReviews = value;
                this.OnPropertyChanged();
            }
        }

        public async Task LoadAllData()
        {
            try
            {
                await LoadFlaggedReviews();
                await LoadAppeals();
                await LoadRoleRequests();
                await LoadOffensiveWords();
                await LoadDrinkModificationRequests();
                await LoadUsersWithHiddenReviews();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading data: {ex.Message}");
            }
        }

        public async Task LoadFlaggedReviews()
        {
            List<ReviewDTO> reviews = await this.reviewsService.GetFlaggedReviews();
            this.FlaggedReviews = new ObservableCollection<ReviewDTO>(reviews);
        }

        public async Task LoadAppeals()
        {
            try
            {
                List<User> appeals = await this.userService.GetBannedUsersWhoHaveSubmittedAppeals();
                this.AppealsUsers = new ObservableCollection<User>(appeals);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading appeals: {ex.Message}");
                this.AppealsUsers = new ObservableCollection<User>();
            }
        }

        public async Task LoadRoleRequests()
        {
            try
            {
                List<UpgradeRequest> requests = await this.requestsService.RetrieveAllUpgradeRequests();
                this.UpgradeRequests = new ObservableCollection<UpgradeRequest>(requests);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading role requests: {ex.Message}");
                this.UpgradeRequests = new ObservableCollection<UpgradeRequest>();
            }
        }

        public async Task LoadOffensiveWords()
        {
            try
            {
                var words = await checkersService.GetOffensiveWordsList();
                this.OffensiveWords = new ObservableCollection<string>(words);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading offensive words: {ex.Message}");
            }
        }

        public async Task LoadDrinkModificationRequests()
        {
            try
            {
                IEnumerable<DrinkModificationRequestDTO> allRequests = await this.drinkModificationRequestService.GetAllModificationRequests();

                // Filter out temporary "Add" requests that are part of "Edit" operations
                List<DrinkModificationRequestDTO> filteredRequests = allRequests
                    .Where(request =>
                    {
                        // Keep all requests except "Add" requests that have a corresponding "Edit" request
                        if (request.ModificationType == DrinkModificationRequestType.Add)
                        {
                            // Check if this "Add" request is part of an "Edit" operation
                            // by looking for an "Edit" request with the same NewDrinkId as this request's NewDrinkId
                            bool isPartOfEdit = allRequests.Any(editRequest =>
                                editRequest.ModificationType == DrinkModificationRequestType.Edit &&
                                editRequest.NewDrinkId == request.NewDrinkId);

                            return !isPartOfEdit; // Only include "Add" requests that are NOT part of an edit
                        }

                        return true; // Include all "Edit" and "Remove" requests
                    })
                    .ToList();

                // Create wrapper objects with usernames
                var requestsWithUsernames = new List<DrinkModificationRequestWithUsername>();
                foreach (var request in filteredRequests)
                {
                    var username = await GetUsernameById(request.RequestingUserId);
                    requestsWithUsernames.Add(new DrinkModificationRequestWithUsername 
                    { 
                        Request = request, 
                        Username = username 
                    });
                }

                this.DrinkModificationRequests = new ObservableCollection<DrinkModificationRequestWithUsername>(requestsWithUsernames);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading drink modification requests: {ex.Message}");
                this.DrinkModificationRequests = new ObservableCollection<DrinkModificationRequestWithUsername>();
            }
        }

        public async Task FilterReviews(string filter)
        {
            List<ReviewDTO> reviews = await this.reviewsService.FilterReviewsByContent(filter);
            this.FlaggedReviews = new ObservableCollection<ReviewDTO>(reviews);
        }

        public async Task FilterAppeals(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                await this.LoadAppeals();
                return;
            }

            filter = filter.ToLower();
            List<User> appeals = await this.userService.GetBannedUsersWhoHaveSubmittedAppeals();
            this.AppealsUsers = new ObservableCollection<User>(
                appeals.Where(user =>
                    user.EmailAddress.ToLower().Contains(filter) ||
                    user.Username.ToLower().Contains(filter) ||
                    user.UserId.ToString().Contains(filter))
                .ToList());
        }

        public async void ResetReviewFlags(int reviewId)
        {
            await this.reviewsService.ResetReviewFlags(reviewId);
            await this.LoadFlaggedReviews();
        }

        public async void HideReview(int reviewId)
        {
            await this.reviewsService.HideReview(reviewId);
            await this.reviewsService.ResetReviewFlags(reviewId);
            await this.LoadFlaggedReviews();
        }

        public async Task RunAICheck(ReviewDTO review)
        {
            try
            {
                //this.checkersService.RunAICheckForOneReviewAsync(review);
                await this.LoadFlaggedReviews();
            }
            catch
            {
            }
        }

        public async Task<List<string>> RunAutoCheck()
        {
            try
            {
                List<ReviewDTO> reviews = await this.reviewsService.GetFlaggedReviews();
                //List<string> messages = await Task.Run(() => this.checkersService.RunAutoCheck(reviews));
                await this.LoadFlaggedReviews();
                return null;
            }
            catch
            {
                throw;
            }
        }

        public async void AddOffensiveWord(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return;
            }

            try
            {
                await this.checkersService.AddOffensiveWordAsync(word);
                this.offensiveWords.Add(word);
                this.OnPropertyChanged(nameof(this.OffensiveWords));
            }
            catch
            {
            }
        }

        public async void DeleteOffensiveWord(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return;
            }

            try
            {
                await this.checkersService.DeleteOffensiveWordAsync(word);
                this.offensiveWords.Remove(word);
                this.OnPropertyChanged(nameof(this.OffensiveWords));
            }
            catch
            {
            }
        }

        public async Task HandleUpgradeRequest(bool isAccepted, int requestId)
        {
            try
            {
                await requestsService.ProcessUpgradeRequest(isAccepted, requestId);
                Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread()?.TryEnqueue(async () =>
                {
                    List<UpgradeRequest> requests = await requestsService.RetrieveAllUpgradeRequests();

                    UpgradeRequests = new ObservableCollection<UpgradeRequest>(requests);

                    this.OnPropertyChanged(nameof(UpgradeRequests));
                });
            }
            catch
            {
                throw;
            }
        }

        public async void CloseAppealCase(User user)
        {
            user.HasSubmittedAppeal = false;
            await this.LoadAppeals();
        }

        public List<ReviewDTO> GetUserReviews(Guid userId)
        {
            return this.reviewsService.GetReviewsByUser(userId).Result;
        }

        public async Task<User?> GetUserById(Guid userId)
        {
            return await this.userService.GetUserById(userId);
        }

        public async Task<RoleType?> GetHighestRoleTypeForUser(Guid userId)
        {
            return await this.userService.GetHighestRoleTypeForUser(userId);
        }

        public async Task<string> GetRoleNameBasedOnID(RoleType roleType)
        {
            return await this.requestsService.GetRoleNameBasedOnIdentifier(roleType);
        }

        public async Task LoadUserAppealDetails(User user)
        {
            try
            {
                this.IsAppealUserBanned = true;
                this.UserStatusDisplay = this.GetUserStatusDisplay(user, true);

                List<ReviewDTO> reviews = await this.reviewsService.GetReviewsByUser(user.UserId);
                this.UserReviewsFormatted = new ObservableCollection<string>(
                    reviews.Select(r => this.FormatReviewContent(r)).ToList());
            }
            catch
            {
                throw;
            }
        }

        private async Task UpdateUserHasAppealed(User user, bool newValue)
        {
            try
            {
                await this.userService.UpdateUserAppleaed(user, newValue);
            }
            catch
            {
                throw;
            }
        }

        public async Task KeepBanForUser(User user)
        {
            try
            {
                if (user == null)
                {
                    return;
                }

                await this.userService.UpdateUserRole(user.UserId, RoleType.Banned);
                await this.UpdateUserHasAppealed(user, false);
                this.IsAppealUserBanned = true;
                this.UserStatusDisplay = this.GetUserStatusDisplay(user, true);
                await this.LoadAppeals();
            }
            catch
            {
                throw;
            }
        }

        public async Task AcceptAppealForUser(User user)
        {
            try
            {
                if (user == null)
                {
                    return;
                }

                await this.userService.UpdateUserRole(user.UserId, RoleType.User);
                User? updatedUser = await this.userService.GetUserById(user.UserId);

                if (updatedUser == null)
                {
                    return;
                }

                await this.UpdateUserHasAppealed(updatedUser, false);
                this.IsAppealUserBanned = false;
                this.UserStatusDisplay = this.GetUserStatusDisplay(user, false);
                await this.LoadAppeals();
            }
            catch
            {
                throw;
            }
        }

        public async Task LoadUpgradeRequestDetails(UpgradeRequest request)
        {
            if (request == null)
            {
                return;
            }

            User? requestingUser = await this.GetUserById(request.RequestingUserIdentifier);
            RoleType? currentRoleID = await this.GetHighestRoleTypeForUser(request.RequestingUserIdentifier);

            if (requestingUser == null || currentRoleID == null)
            {
                return;
            }

            string currentRoleName = await this.GetRoleNameBasedOnID((RoleType)currentRoleID);
            string requiredRoleName = await this.GetRoleNameBasedOnID((RoleType)currentRoleID + 1);

            this.UserUpgradeInfo = this.FormatUserUpgradeInfo(requestingUser, currentRoleName, requiredRoleName);
        }

        public string GetUserStatusDisplay(User user, bool isBanned)
        {
            if (user == null)
            {
                return string.Empty;
            }

            return $"User ID: {user.UserId}\nEmail: {user.EmailAddress}\nStatus: {(isBanned ? "Banned" : "Active")}";
        }

        public string FormatUserUpgradeInfo(User user, string currentRoleName, string requiredRoleName)
        {
            if (user == null)
            {
                return string.Empty;
            }

            return $"User ID: {user.UserId}\nEmail: {user.EmailAddress}\n{currentRoleName} -> {requiredRoleName}";
        }

        public string FormatReviewContent(ReviewDTO review)
        {
            if (review == null)
            {
                return string.Empty;
            }

            return $"{review.Content}";
        }

        public string FormatReviewWithFlags(ReviewDTO review)
        {
            if (review == null)
            {
                return string.Empty;
            }

            return $"{review.Content}\nFlags: {review.NumberOfFlags}";
        }

        public void ShowWordListPopup()
        {
            this.IsWordListVisible = true;
        }

        public void HideWordListPopup()
        {
            this.IsWordListVisible = false;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void InitializeCommands()
        {
            this.KeepBanCommand = new RelayCommand(async () =>
            {
                try
                {
                    await this.KeepBanForUser(this.SelectedAppealUser);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error in KeepBanCommand: {ex.Message}");
                }
            });
            this.AcceptAppealCommand = new RelayCommand(async () =>
            {
                try
                {
                    await this.AcceptAppealForUser(this.SelectedAppealUser);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error in AcceptAppealCommand: {ex.Message}");
                }
            });
            this.CloseAppealCaseCommand = new RelayCommand(() => this.CloseAppealCase(this.SelectedAppealUser));

            this.HandleUpgradeRequestCommand = new RelayCommand<Tuple<bool, int>>(async param =>
                await this.HandleUpgradeRequest(param.Item1, param.Item2));

            this.ResetReviewFlagsCommand = new RelayCommand<int>(reviewId =>
                this.ResetReviewFlags(reviewId));

            this.HideReviewCommand = new RelayCommand<int>(param =>
                this.HideReview(param));
            

            this.RunAICheckCommand = new RelayCommand<ReviewDTO>(async review =>
                await this.RunAICheck(review));

            this.RunAutoCheckCommand = new AsyncRelayCommand(async () =>
            {
                try
                {
                    List<string> messages = await this.RunAutoCheck();
                    Console.WriteLine("Auto check completed with messages:");
                    foreach (var message in messages)
                    {
                        Console.WriteLine(message);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error in RunAutoCheckCommand: {ex.Message}");
                }
            });

            this.AddOffensiveWordCommand = new RelayCommand<string>(word =>
                this.AddOffensiveWord(word));

            this.DeleteOffensiveWordCommand = new RelayCommand<string>(word =>
                this.DeleteOffensiveWord(word));

            this.ShowWordListPopupCommand = new RelayCommand(() => this.ShowWordListPopup());
            this.HideWordListPopupCommand = new RelayCommand(() => this.HideWordListPopup());

            this.BanUserCommand = new RelayCommand<Guid>(userId =>
            {
                _ = this.BanUser(userId);
            });

            this.ApproveDrinkModificationCommand = new RelayCommand<int>(modificationRequestId =>
            {
                _ = this.ApproveDrinkModification(modificationRequestId);
            });

            this.DenyDrinkModificationCommand = new RelayCommand<int>(modificationRequestId =>
            {
                _ = this.DenyDrinkModification(modificationRequestId);
            });
        }


        private void UpdateUserRole(User user, RoleType roleType)
        {
            this.userService.UpdateUserRole(user.UserId, roleType);
        }

        public async Task LoadUsersWithHiddenReviews()
        {
            try
            {
                List<User> users = await this.userService.GetUsersWithHiddenReviews();
                users = users.Where(u => u.AssignedRole != RoleType.Admin && u.AssignedRole != RoleType.Banned).ToList();
                this.UsersWithHiddenReviews = new ObservableCollection<User>(users);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading users with hidden reviews: {ex.Message}");
                this.UsersWithHiddenReviews = new ObservableCollection<User>();
            }
        }

        public async Task BanUser(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return;
            }

            try
            {
                await this.userService.UpdateUserRole(userId, RoleType.Banned);

                User? userToRemove = this.UsersWithHiddenReviews.FirstOrDefault(u => u.UserId == userId);
                if (userToRemove == null)
                {
                    return;
                }

                await this.LoadUsersWithHiddenReviews();

                this.OnPropertyChanged(nameof(UsersWithHiddenReviews));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error banning user: {ex.Message}");
            }
        }

        public async Task ApproveDrinkModification(int modificationRequestId)
        {
            try
            {
                await this.drinkModificationRequestService.ApproveRequest(modificationRequestId, App.CurrentUserId);
                await LoadDrinkModificationRequests(); // Refresh the list
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error approving drink modification: {ex.Message}");
            }
        }

        public async Task DenyDrinkModification(int modificationRequestId)
        {
            try
            {
                await this.drinkModificationRequestService.DenyRequest(modificationRequestId, App.CurrentUserId);
                await LoadDrinkModificationRequests(); // Refresh the list
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error denying drink modification: {ex.Message}");
            }
        }

        public string GetOperationTypeDisplayName(DrinkModificationRequestType modificationType)
        {
            return modificationType switch
            {
                DrinkModificationRequestType.Add => "Add",
                DrinkModificationRequestType.Edit => "Update",
                DrinkModificationRequestType.Remove => "Remove",
                _ => "Unknown"
            };
        }

        public string GetOperationDisplayName(DrinkModificationRequestType type)
        {
            return type switch
            {
                DrinkModificationRequestType.Add => "Add",
                DrinkModificationRequestType.Edit => "Update", 
                DrinkModificationRequestType.Remove => "Remove",
                _ => type.ToString()
            };
        }

        public async Task<string> GetUsernameById(Guid userId)
        {
            try
            {
                User user = await userService.GetUserById(userId);
                return string.IsNullOrEmpty(user?.Username) ? $"User {userId}" : user.Username;
            }
            catch
            {
                return $"User {userId}";
            }
        }
    }
}
