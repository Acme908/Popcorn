﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using NuGet;
using Popcorn.Helpers;
using Popcorn.Messaging;
using Popcorn.Models.ApplicationState;
using Popcorn.Services.Shows.Show;
using Popcorn.Services.User;

namespace Popcorn.ViewModels.Pages.Home.Show.Tabs
{
    public class RecentShowTabViewModel : ShowTabsViewModel
    {
        /// <summary>
        /// Initializes a new instance of the RecentShowTabViewModel class.
        /// </summary>
        /// <param name="applicationService">Application state</param>
        /// <param name="showService">Show service</param>
        /// <param name="userService">The user service</param>
        public RecentShowTabViewModel(IApplicationService applicationService, IShowService showService,
            IUserService userService)
            : base(applicationService, showService, userService,
                () => LocalizationProviderHelper.GetLocalizedValue<string>("RecentTitleTab"))
        {
        }

        /// <summary>
        /// Load shows asynchronously
        /// </summary>
        public override async Task LoadShowsAsync()
        {
            var watch = Stopwatch.StartNew();
            Page++;
            if (Page > 1 && Shows.Count == MaxNumberOfShows) return;
            Logger.Info(
                $"Loading shows recent page {Page}...");
            HasLoadingFailed = false;
            try
            {
                IsLoadingShows = true;
                var result =
                    await ShowService.GetShowsAsync(Page,
                            MaxShowsPerPage,
                            Rating * 10,
                            "year",
                            CancellationLoadingShows.Token,
                            Genre)
                        .ConfigureAwait(false);

                DispatcherHelper.CheckBeginInvokeOnUI(async () =>
                {
                    Shows.AddRange(result.shows);
                    IsLoadingShows = false;
                    IsShowFound = Shows.Any();
                    CurrentNumberOfShows = Shows.Count;
                    MaxNumberOfShows = result.nbShows;
                    await UserService.SyncShowHistoryAsync(Shows).ConfigureAwait(false);
                });
            }
            catch (Exception exception)
            {
                Page--;
                Logger.Error(
                    $"Error while loading shows recent page {Page}: {exception.Message}");
                HasLoadingFailed = true;
                Messenger.Default.Send(new ManageExceptionMessage(exception));
            }
            finally
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Logger.Info(
                    $"Loaded shows recent page {Page} in {elapsedMs} milliseconds.");
            }
        }
    }
}