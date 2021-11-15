using AppProjekt.Constants;
using AppProjekt.Models;
using MonkeyCache.FileStore;
using Repository;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TinyIoC;
using Xamarin.Essentials;

namespace AppProjekt.Services
{
    public class TelemetricsService : ITelemetricsService
    {
        private readonly IGenericRepository _genericRepository;
        public TelemetricsService()
        {
            _genericRepository = TinyIoCContainer.Current.Resolve<IGenericRepository>();
        }

        public async Task<IEnumerable<Telemetrics>> GetTelemetricsAsync()
        {
            UriBuilder builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = $"{ApiConstants.TelemetricsEndpoint}/GetTelemetryData"
            };
            string url = builder.Path;

            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                return Barrel.Current.Get<IEnumerable<Telemetrics>>(key: url);
            }
            if (!Barrel.Current.IsExpired(key: url))
            {
                return Barrel.Current.Get<IEnumerable<Telemetrics>>(key: url);
            }
            var telemetrics = await _genericRepository.GetAsync<IEnumerable<Telemetrics>>(builder.ToString());

            Barrel.Current.Add(key: url, data: telemetrics, expireIn: TimeSpan.FromSeconds(20));

            return telemetrics;
        }

        //public async Task<Telemetrics> GetTelemetricsAsync(string id)
        //{
        //    UriBuilder builder = new UriBuilder(ApiConstants.BaseApiUrl)
        //    {
        //        Path = $"{ApiConstants.TelemetricsEndpoint}/{id}"
        //    };
        //    return await _genericRepository.GetAsync<Telemetrics>(builder.ToString());
        //}

        //public async Task<bool> AddTelemetricsAsync(Telemetrics Telemetrics)
        //{
        //    UriBuilder builder = new UriBuilder(ApiConstants.BaseApiUrl)
        //    {
        //        Path = ApiConstants.TelemetricsEndpoint
        //    };
        //    await _genericRepository.PostAsync(builder.ToString(), Telemetrics);
        //    return true;
        //}

        //public async Task<bool> UpdateTelemetricsAsync(Telemetrics Telemetrics)
        //{
        //    UriBuilder builder = new UriBuilder(ApiConstants.BaseApiUrl)
        //    {
        //        Path = $"{ApiConstants.TelemetricsEndpoint}/{Telemetrics.Id}"
        //    };
        //    await _genericRepository.PutAsync(builder.ToString(), Telemetrics);
        //    return true;
        //}

        //public async Task<bool> DeleteTelemetricsAsync(string id)
        //{
        //    UriBuilder builder = new UriBuilder(ApiConstants.BaseApiUrl)
        //    {
        //        Path = $"{ApiConstants.TelemetricsEndpoint}/{id}"
        //    };
        //    await _genericRepository.DeleteAsync(builder.ToString());
        //    return true;
        //}
    }
}