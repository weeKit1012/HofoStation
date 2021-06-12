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

        public CancellationTokenSource cts;
        public Geopoint point;

        public async Task<Geopoint> GetLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);
                var point = new Geopoint();

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
                return point;
            }
            catch (FeatureNotEnabledException)
            {
                // Handle not enabled on device exception
                await Shell.Current.DisplayAlert("Warning", "Please enable the GPS feature on your device.", "OK");
                return point;
            }
            catch (PermissionException)
            {
                // Handle permission exception
                await Shell.Current.DisplayAlert("Warning", "Please allow the GPS feature on your device.", "OK");
                return point;
            }
            catch (Exception)
            {
                // Unable to get location
                await Shell.Current.DisplayAlert("Warning", "Cannot get your current location. Please try again later.", "OK");
                return point;
            }
        }
    }

    public class Geopoint
    {
        public string longitude { get; set; }

        public string latitude { get; set; }
    }
}
