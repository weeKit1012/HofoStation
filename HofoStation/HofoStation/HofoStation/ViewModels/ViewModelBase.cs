/*
 Programmer Name    :   TEY WEE KIT
 Program Name       :   HofoStation (Front End, User Interface)
 */

using MvvmHelpers;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HofoStation.ViewModels
{
    public class ViewModelBase : BaseViewModel
    {
        //Refer to BaseViewModel to get more info

        public NetworkAccess connectivity;
        public CancellationTokenSource cts;
        public Geopoint point;

        public async Task<Geopoint> GetLocation()
        {
            try
            {
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                Location location = await Geolocation.GetLocationAsync(request, cts.Token);
                Geopoint point = new Geopoint();

                if (location != null)
                {
                    point.longitude = location.Longitude.ToString();
                    point.latitude = location.Latitude.ToString();
                    return point;
                }
                else
                {
                    return point;
                }
            }
            catch (FeatureNotSupportedException)
            {
                // Handle not supported on device exception
                await Shell.Current.DisplayAlert("Warning", "Your device does not support GPS feature.", "OK");
                point = null;
                return point;
            }
            catch (FeatureNotEnabledException)
            {
                // Handle not enabled on device exception
                await Shell.Current.DisplayAlert("Warning", "Please enable the GPS feature on your device.", "OK");
                point = null;
                return point;
            }
            catch (PermissionException)
            {
                // Handle permission exception
                await Shell.Current.DisplayAlert("Warning", "Please allow the GPS feature on your device.", "OK");
                point = null;
                return point;
            }
            catch (Exception)
            {
                // Unable to get location
                await Shell.Current.DisplayAlert("Warning", "Cannot get your current location. Please try again later.", "OK");
                point = null;
                return point;
            }
        }

        public bool CheckBackButton()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                bool exit = await Shell.Current.DisplayAlert("Confirm Exit", "Do you really want to exit the application?", "Yes", "No").ConfigureAwait(false);

                if (exit)
                {
                    _ = System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
                }
            });
            return true;
        }

        public bool IsConnected(NetworkAccess conn)
        {
            if (conn == NetworkAccess.None)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public bool IsNotContainNumber(string str)
        {
            return !str.Any(char.IsDigit);
        }
    }

    public class Geopoint
    {
        public string longitude { get; set; }

        public string latitude { get; set; }
    }
}
