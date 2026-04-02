using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Hackathon_Project_DavidCaballero.Utilities
{
    public static class Jeeves
    {
        //Stores the base URL of your local Web API
        //make sure the Web API is running and the port is correct
        public static Uri DBUri = new Uri("http://localhost:5075/");

        //Shows a simple popup message dialog with an OK button
        internal static async void ShowMessage(string strTitle, string Msg)
        {
            ContentDialog diag = new ContentDialog()
            {
                Title = strTitle,
                Content = Msg,
                PrimaryButtonText = "Ok",
                DefaultButton = ContentDialogButton.Primary
            };
            _ = await diag.ShowAsync();
        }

        //Shows a popup confirmation dialog and returns whether the user clicked Yes or No
        internal static async Task<ContentDialogResult> ConfirmDialog(string strTitle, string Msg)
        {
            ContentDialog diag = new ContentDialog()
            {
                Title = strTitle,
                Content = Msg,
                PrimaryButtonText = "No",
                SecondaryButtonText = "Yes",
                DefaultButton = ContentDialogButton.Primary
            };
            ContentDialogResult result = await diag.ShowAsync();
            return result;
        }

    }
}
