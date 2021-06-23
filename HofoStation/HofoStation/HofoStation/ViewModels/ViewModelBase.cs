using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
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
    }

    public class Geopoint
    {
        public string longitude { get; set; }

        public string latitude { get; set; }
    }
}
