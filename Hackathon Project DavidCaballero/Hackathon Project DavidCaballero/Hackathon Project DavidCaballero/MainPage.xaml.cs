using System;
using System.Collections.Generic;
using Hackathon_Project_DavidCaballero.Data;
using Hackathon_Project_DavidCaballero.Models;
using Hackathon_Project_DavidCaballero.Utilities;
using Windows.UI.Xaml; 
using Windows.UI.Xaml.Controls;

namespace Hackathon_Project_DavidCaballero
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a <see cref="Frame">.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly IRegionRepository regionRepository;
        private readonly IChallengeRepository challengeRepository;
        private readonly IMemberRepository memberRepository;

        private Member _selectedMember = null;
        public MainPage()
        {
            this.InitializeComponent();

            regionRepository = new RegionRepository();
            challengeRepository = new ChallengeRepository();
            memberRepository = new MemberRepository();

            FillFilterType();
        }

        private void FillFilterType()
        {
            //cmbFilterType.ItemsSource = new List<string> { "Region", "Challenge" };
            //cmbFilterType.SelectedIndex = 0; // default
            var selectedItem = cmbFilterType.SelectedItem as ComboBoxItem;
            string type = selectedItem?.Content?.ToString() ?? "Region";

            cmbFilterType.SelectedIndex = 0;
        }

        // 2) Fill the second combo based on filter type
        private async void FillDropDown()
        {
            progRing.IsActive = true;
            progRing.Visibility = Visibility.Visible;

            try
            {
                //string type = cmbFilterType.SelectedItem?.ToString() ?? "Region";
                var selectedItem = cmbFilterType.SelectedItem as ComboBoxItem;
                string type = selectedItem?.Content?.ToString() ?? "Region";

                if (type == "Region")
                {
                    List<Region> regions = await regionRepository.GetRegions();

                    // Add All option (resumen visible)
                    regions.Insert(0, new Region { ID = 0, Name = "All Regions", Code = "--" });

                    cmbFilterValue.ItemsSource = regions;
                    cmbFilterValue.DisplayMemberPath = "Summary";
                    cmbFilterValue.SelectedIndex = 0;

                    ShowMembers(regionID: null, challengeID: null);
                }
                else // Challenge
                {
                    List<Challenge> challenges = await challengeRepository.GetChallenges();

                    // Add All option
                    challenges.Insert(0, new Challenge { ID = 0, Name = "All Challenges", Code = "--" });

                    cmbFilterValue.ItemsSource = challenges;
                    cmbFilterValue.DisplayMemberPath = "Summary"; 
                    cmbFilterValue.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                if (ex.GetBaseException().Message.Contains("connection with the server"))
                {
                    Jeeves.ShowMessage("Error", "No connection with the server.");

                }

                else
                {
                    Jeeves.ShowMessage("Error", ex.GetBaseException().Message);
                }
                    
            }
            finally
            {
                progRing.IsActive = false;
                progRing.Visibility = Visibility.Collapsed;
            }
        }

        // 3) Show members based on filter selection
        private async void ShowMembers(int? regionID, int? challengeID)
        {
            progRing.IsActive = true;
            progRing.Visibility = Visibility.Visible;

            try
            {
                List<Member> members;

                // If a Region filter is chosen
                if (regionID.GetValueOrDefault() > 0)
                {
                    members = await memberRepository.GetMembersByRegion(regionID.Value);
                }
                // If a Challenge filter is chosen
                else if (challengeID.GetValueOrDefault() > 0)
                {
                    members = await memberRepository.GetMembersByChallenge(challengeID.Value);
                }
                else
                {
                    members = await memberRepository.GetMembers();
                }

                gvMembers.ItemsSource = members;
                txtCount.Text = $"Members: {members.Count}";
            }
            catch (Exception ex)
            {
                txtCount.Text = "";
                if (ex.GetBaseException().Message.Contains("connection with the server"))
                    Jeeves.ShowMessage("Error", "No connection with the server.");
                else
                    Jeeves.ShowMessage("Error", ex.GetBaseException().Message);
            }
            finally
            {
                progRing.IsActive = false;
                progRing.Visibility = Visibility.Collapsed;
            }
        }

        private void cmbFilterType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillDropDown();
        }

        private void cmbFilterValue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFilterValue.SelectedItem == null) return;
            var typeItem = cmbFilterType.SelectedItem as ComboBoxItem;
            string type = typeItem?.Content?.ToString() ?? "Region";
            if (type == "Region" && cmbFilterValue.SelectedItem is Region r)
            {
                ShowMembers(r.ID > 0 ? r.ID : (int?)null, null);
            }
            else if (type == "Challenge" && cmbFilterValue.SelectedItem is Challenge c)
            {
                ShowMembers(null, c.ID > 0 ? c.ID : (int?)null);
            }
        }

        // Refresh button
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            FillDropDown();
        }
        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new MemberDialog();
            var result = await dlg.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                try
                {
                    progRing.IsActive = true;
                    progRing.Visibility = Visibility.Visible;

                    await memberRepository.AddMember(dlg.ResultMember);
                    Jeeves.ShowMessage("Saved", "Member added successfully.");

                    // refresh list
                    btnRefresh_Click(null, null);
                }
                catch (Exception ex)
                {
                    Jeeves.ShowMessage("API Error", ex.GetBaseException().Message);
                }
                finally
                {
                    progRing.IsActive = false;
                    progRing.Visibility = Visibility.Collapsed;
                }
            }
        }

        private async void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedMember == null)
            {
                Jeeves.ShowMessage("Edit", "Please select a member first.");
                return;
            }

            // Pass a copy so dialog edits safely
            var copy = new Member
            {
                ID = _selectedMember.ID,
                FirstName = _selectedMember.FirstName,
                MiddleName = _selectedMember.MiddleName,
                LastName = _selectedMember.LastName,
                MemberCode = _selectedMember.MemberCode,
                DOB = _selectedMember.DOB,
                SkillRating = _selectedMember.SkillRating,
                YearsExperience = _selectedMember.YearsExperience,
                Category = _selectedMember.Category,
                Organization = _selectedMember.Organization,
                RegionID = _selectedMember.RegionID,
                ChallengeID = _selectedMember.ChallengeID,
                RowVersion = _selectedMember.RowVersion
            };

            var dlg = new MemberDialog(copy);
            var result = await dlg.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                try
                {
                    progRing.IsActive = true;
                    progRing.Visibility = Visibility.Visible;

                    await memberRepository.UpdateMember(dlg.ResultMember);
                    Jeeves.ShowMessage("Saved", "Member updated successfully.");

                    btnRefresh_Click(null, null);
                }
                catch (Exception ex)
                {
                    // If API returns 409 for concurrency, you'll see it here too
                    Jeeves.ShowMessage("API Error", ex.GetBaseException().Message);
                }
                finally
                {
                    progRing.IsActive = false;
                    progRing.Visibility = Visibility.Collapsed;
                }
            }
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedMember == null)
            {
                Jeeves.ShowMessage("Delete", "Please select a member first.");
                return;
            }

            var result = await Jeeves.ConfirmDialog("Delete Member", $"Delete {_selectedMember.Summary}?");
            if (result == ContentDialogResult.Secondary)
            {
                try
                {
                    progRing.IsActive = true;
                    progRing.Visibility = Visibility.Visible;

                    await memberRepository.DeleteMember(_selectedMember.ID);
                    Jeeves.ShowMessage("Deleted", "Member deleted successfully.");

                    btnRefresh_Click(null, null);
                }
                catch (Exception ex)
                {
                    Jeeves.ShowMessage("API Error", ex.GetBaseException().Message);
                }
                finally
                {
                    progRing.IsActive = false;
                    progRing.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void gvMembers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedMember = gvMembers.SelectedItem as Member;
        }

    }
}
