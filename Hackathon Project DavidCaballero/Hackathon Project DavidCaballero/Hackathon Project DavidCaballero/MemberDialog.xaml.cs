using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Hackathon_Project_DavidCaballero.Data;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Hackathon_Project_DavidCaballero.Models;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Hackathon_Project_DavidCaballero
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MemberDialog : ContentDialog
    {
        private readonly IRegionRepository _regionRepo = new RegionRepository();
        private readonly IChallengeRepository _challengeRepo = new ChallengeRepository();

        // This is what MainPage reads after dialog closes
        public Member ResultMember { get; private set; } = new Member();

        // If editing, pass a copy of the selected member
        private readonly Member? _editingMember;

        public MemberDialog(Member? editingMember = null)
        {
            this.InitializeComponent();
            _editingMember = editingMember;

            // default category
            cmbCategory.SelectedIndex = 0;

            this.Loaded += MemberDialog_Loaded;
        }

        private async void MemberDialog_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Load lookups
                List<Region> regions = await _regionRepo.GetRegions();
                cmbRegion.ItemsSource = regions.OrderBy(r => r.Name).ToList();

                List<Challenge> challenges = await _challengeRepo.GetChallenges();
                cmbChallenge.ItemsSource = challenges.OrderBy(c => c.Name).ToList();

                // If editing, pre-fill fields
                if (_editingMember != null)
                {
                    Title = "Edit Member";

                    txtFirstName.Text = _editingMember.FirstName;
                    txtMiddleName.Text = _editingMember.MiddleName ?? "";
                    txtLastName.Text = _editingMember.LastName;
                    txtMemberCode.Text = _editingMember.MemberCode;

                    dpDOB.Date = _editingMember.DOB;

                    txtOrganization.Text = _editingMember.Organization;

                    // Category
                    cmbCategory.SelectedItem = cmbCategory.Items
                        .Cast<ComboBoxItem>()
                        .FirstOrDefault(i => (i.Content?.ToString() ?? "") == _editingMember.Category) ?? cmbCategory.Items[0];

                    nbSkillRating.Value = _editingMember.SkillRating;
                    nbYearsExp.Text = _editingMember.YearsExperience.ToString();

                    // Select region/challenge
                    cmbRegion.SelectedItem = regions.FirstOrDefault(r => r.ID == _editingMember.RegionID);
                    cmbChallenge.SelectedItem = challenges.FirstOrDefault(c => c.ID == _editingMember.ChallengeID);
                }
                else
                {
                    Title = "Add Member";

                    // default selections
                    cmbRegion.SelectedIndex = regions.Count > 0 ? 0 : -1;
                    cmbChallenge.SelectedIndex = challenges.Count > 0 ? 0 : -1;

                    dpDOB.Date = DateTimeOffset.Now.Date;
                    nbSkillRating.Value = 1;
                    nbYearsExp.Text = "";
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            txtError.Visibility = Visibility.Collapsed;

            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtMemberCode.Text))
            {
                args.Cancel = true;
                ShowError("First Name, Last Name, and Member Code are required.");
                return;
            }

            if (cmbRegion.SelectedItem is not Region selRegion)
            {
                args.Cancel = true;
                ShowError("Please select a Region.");
                return;
            }

            if (cmbChallenge.SelectedItem is not Challenge selChallenge)
            {
                args.Cancel = true;
                ShowError("Please select a Challenge.");
                return;
            }

            // --- Slider value ---
            int skill = (int)nbSkillRating.Value;

            if (skill < 1 || skill > 10)
            {
                args.Cancel = true;
                ShowError("Skill Rating must be between 1 and 10.");
                return;
            }

            // --- TextBox value ---
            if (!int.TryParse(nbYearsExp.Text, out int years) || years < 0)
            {
                args.Cancel = true;
                ShowError("Years Experience must be a valid number (0 or higher).");
                return;
            }

            var dto = new Member
            {
                ID = _editingMember?.ID ?? 0,
                RowVersion = _editingMember?.RowVersion,

                FirstName = txtFirstName.Text.Trim(),
                MiddleName = string.IsNullOrWhiteSpace(txtMiddleName.Text) ? null : txtMiddleName.Text.Trim(),
                LastName = txtLastName.Text.Trim(),
                MemberCode = txtMemberCode.Text.Trim(),

                DOB = dpDOB.Date.DateTime.Date,

                Organization = txtOrganization.Text.Trim(),
                Category = (cmbCategory.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "A",

                SkillRating = skill,
                YearsExperience = years,

                RegionID = selRegion.ID,
                ChallengeID = selChallenge.ID
            };

            ResultMember = dto;
        }

        private void ShowError(string message)
        {
            txtError.Text = message;
            txtError.Visibility = Visibility.Visible;
        }
    }
}
