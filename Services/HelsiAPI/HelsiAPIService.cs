using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Valeo.Bot.Configuration;
using Valeo.Bot.Configuration.Entities;
using Valeo.Bot.Data.Entities;
using Valeo.Bot.Services.HelsiAPI.Models;

namespace Valeo.Bot.Services.HelsiAPI
{
    public class HelsiAPIService : IHelsiAPIService
    {
        private const int SafeExpirationGap = 60 * 5;
        private static AuthorizationData authData;

        public static AuthorizationData Auth
        {
            get
            {
                if (authData == null || authData.expiration_date < DateTime.Now)
                {
                    LoadAuth();
                }
                return authData;
            }
        }
        private HttpClient _client;
        private HttpClient Client
        {
            get
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Auth.token_type, Auth.access_token);
                return _client;
            }
        }

        private static void LoadAuth()
        {
            HttpClientHandler hmh = new HttpClientHandler()
            {
                // Proxy = new WebProxy("http://proxy.isd.dp.ua:8080")
            };
            HttpClient client = new HttpClient(hmh);
            string token = Startup.StaticConfiguration
                .GetSection("HelsiAPI")
                .GetSection("Urls")
                .GetSection("Token").Value;
            HelsiAPIAuth valeoAuth = Startup.StaticConfiguration
                .GetSection("HelsiAPI")
                .GetSection("HelsiAPIAuth").Get<HelsiAPIAuth>();
            var response = client.PostAsync(token, new FormUrlEncodedContent(
                new Dictionary<string, string>
                { { nameof(valeoAuth.client_id), valeoAuth.client_id },
                    { nameof(valeoAuth.client_secret), valeoAuth.client_secret },
                    { nameof(valeoAuth.grant_type), valeoAuth.grant_type },
                    { nameof(valeoAuth.scope), valeoAuth.scope }
                }
            )).Result;
            authData = response.Content.ReadAsAsync<AuthorizationData>().Result;
            authData.expiration_date = DateTime.Now.AddSeconds(authData.expires_in - SafeExpirationGap);

        }
        private HelsiAPIConfig _config;

        public HelsiAPIService(IOptions<HelsiAPIConfig> config)
        {
            _config = config.Value;
            HttpClientHandler hmh = new HttpClientHandler()
            {
                // Proxy = new WebProxy("http://proxy.isd.dp.ua:8080")
            };
            _client = new HttpClient(hmh);
            LoadAuth();
        }

        public async Task<List<Speciality>> GetOrganizationSpecialities()
        {
            string json = await Client.GetStringAsync(_config.Urls.Specialities)
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<HelsiResponse<List<Speciality>>>(json);
            return response.Data;
        }

        public async Task<List<Doctor>> GetDoctors(int limit = 10, string specialityId = "")
        {
            string query = string.Format(_config.Urls.Doctors, limit);

            if (string.IsNullOrEmpty(specialityId))
                query += $"&specialityId={specialityId}";

            string json = await Client.GetStringAsync(query)
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<HelsiResponse<List<Doctor>>>(json);
            return response.Data;
        }

        public async Task<Doctor> GetDoctor(string doctorId)
        {
            string query = string.Format(_config.Urls.DoctorInfo, doctorId);

            string json = await Client.GetStringAsync(query)
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<HelsiResponse<Doctor>>(json);
            return response.Data;
        }

        public async Task<List<TimeSlot>> GetFreeTimeByDoctor(string doctorId, DateTime date)
        {
            List<TimeSlot> result = new List<TimeSlot>();

            Doctor doctor = await GetDoctor(doctorId);

            if (!doctor.Available.ToLower().Equals("available"))
                return result;

            if (Holidays.Any(d => d.Year == date.Year && d.Month == date.Month && d.Day == date.Day))
                return result;

            List<Period> blockedPeriods;
            try
            {
                string jsonBlockedTimeSlots = await Client.GetStringAsync(string.Format(_config.Urls.BlockedTimes, doctorId))
                    .ConfigureAwait(false);
                var response = JsonConvert.DeserializeObject<HelsiResponse<List<Period>>>(jsonBlockedTimeSlots);
                blockedPeriods = response.Data;
            }
            catch(HttpRequestException e)
            {
                blockedPeriods = new List<Period>();
            }
            

            IEnumerable<Period> schedules = doctor.Period
                                                    .Where(v => v.Msg == 1 && v.Day == (int)date.DayOfWeek)
                                                    .OrderBy(v => v.TimeStart);

            
            if (schedules.Count() == 0)
                return result;


            foreach (var schedule in schedules)
            {
                DateTime startTime = DateTime.ParseExact(schedule.TimeStart, "HH:mm", null);
                DateTime startSlot = new DateTime(date.Year, date.Month, date.Day, startTime.Hour, startTime.Minute, 0);

                DateTime endTime = DateTime.ParseExact(schedule.TimeEnd, "HH:mm", null);
                DateTime endSlot;
                if (endTime.Hour != 0)
                    endSlot = new DateTime(date.Year, date.Month, date.Day, endTime.Hour, endTime.Minute, 0);
                else 
                {
                    DateTime nextDay = date.AddDays(1);
                    endSlot = new DateTime(nextDay.Year, nextDay.Month, nextDay.Day, endTime.Hour, endTime.Minute, 0);
                }
                    

                foreach (var na in doctor.ResourceNA)
                {
                    if (startSlot >= na.DateStartNA && startSlot <= na.DateStopNA)
                        startSlot = na.DateStopNA;
                }

                DateTime startSlotEnd = startSlot.AddMinutes(doctor.Time_slot);
                while (startSlotEnd <= endSlot)
                {
                    if(startSlot >= DateTime.Now)
                        result.Add(new TimeSlot { Start = startSlot });
                    startSlot = startSlotEnd;
                    startSlotEnd = startSlot.AddMinutes(doctor.Time_slot);
                }
            }

            return result;
        }

        public async Task<bool> SaveTime(Order order)
        {
            throw new NotImplementedException();
        }

        private static readonly ReadOnlyCollection<DateTime> Holidays = new ReadOnlyCollection<DateTime>(new List<DateTime> {
            new DateTime(2019, 03, 08), // 8 March 2019 13:00
            new DateTime(2019, 04, 29), // 29 April 2019 13:00
            new DateTime(2019, 04, 30), // 30 April  2019 13:00
            new DateTime(2019, 05, 01), // 01 May  2019 13:00
            new DateTime(2019, 05, 09), // 09 May  2019 13:00
            new DateTime(2019, 06, 17), // 17 June 2019 13:00
            new DateTime(2019, 06, 28), // 28 June 2019 13:00
            new DateTime(2019, 08, 26), // 26 August  2019 13:00
            new DateTime(2019, 10, 14), // 14 October  2019 13:00
            new DateTime(2019, 12, 25), // 25 December 2019 13:00
            new DateTime(2019, 12, 30), // 30 December  2019 13:00
            new DateTime(2019, 12, 31), // 31 December 2019 13:00
        });
    }

    //   doctors/{id} - дані по лікарю
    //     const doctor = {
    //   "available": "AVAILABLE",
    //   "resourceId": "4b27f4c3-77e4-42a3-a80a-e0afe0067fb6",
    //   "period": [
    //     {
    //       "schedulePeriodId": "c0e616f3-6ad2-469f-a9d5-d979e83fc7b9",
    //       "scheduleId": "@schedule1",
    //       "day": "0", // день неділі: 0 - Неділя, ...,  6 - Субота
    //       "parity": "3", // 1 - Непарний, 2 - Парний, 3 - Всі
    //       "type": "work",
    //       "timeStart": "10:00", // Початок періоду
    //       "timeEnd": "14:00", // Кінець періоду
    //       "msg": "1", // Значення 1 - це період для запису пацієнтів
    //       "createdAt": "2017-05-19T07:53:20.47Z",
    //       "updatedAt": "2017-05-19T07:53:20.47Z"
    //     },
    //   ],
    //   resourceNA: [ // Періоди коли лікар недоступний 
    //     {
    //       naId: '2a677a79-c672-4e49-ac6a-0e5646ddb960',
    //       typeNAId: '42537f06-01b1-4ab3-8c63-c28daa6b9b91',
    //       dateStartNA: '2019-10-12T10:00:00Z', // Початок
    //       dateStopNA: '2019-10-12T23:59:00Z', // Закінчення
    //       replacementDoctors: [],
    //       createdAt: '2019-10-11T11:13:43.581Z',
    //       updatedAt: '2019-10-11T11:13:43.581Z',
    //     },
    //   ],
    //   "time_slot": "15", // розмір слота в хвилинах 
    //   "rules": []
    // }

    // *Де:*
    //  period - список всіх інтервалів. Юзер може записатися лише на ті, в яких буде "msg": "1"`
    //  resourceNA - cписок інтервалів, коли лікар недоступний

    // *Також враховуємо:*
    // /doctors/{id}/events - список слотів, які вже зайняті


    // *Дні, коли лікарі не працюють, не важливо який графік:*

    // const HOLIDAYS = [
    //   moment('2019-03-08T13:00:00Z'), // 8 March 2019 13:00
    //   moment('2019-04-29T13:00:00Z'), // 29 April 2019 13:00
    //   moment('2019-04-30T13:00:00Z'), // 30 April  2019 13:00
    //   moment('2019-05-01T13:00:00Z'), // 01 May  2019 13:00
    //   moment('2019-05-09T13:00:00Z'), // 09 May  2019 13:00
    //   moment('2019-06-17T13:00:00Z'), // 17 June 2019 13:00
    //   moment('2019-06-28T13:00:00Z'), // 28 June 2019 13:00
    //   moment('2019-08-26T13:00:00Z'), // 26 August  2019 13:00
    //   moment('2019-10-14T13:00:00Z'), // 14 October  2019 13:00
    //   moment('2019-12-25T13:00:00Z'), // 25 December 2019 13:00
    //   moment('2019-12-30T13:00:00Z'), // 30 December  2019 13:00
    //   moment('2019-12-31T13:00:00Z'), // 31 December 2019 13:00
    // ];
}