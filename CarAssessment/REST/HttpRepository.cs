using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using CarAssessment.Models.Row;
using CarAssessment.Services;
using Xamarin.Forms;

namespace CarAssessment.REST {
    public class HttpRepository {

        public static HttpRepository Instance { get; } = new HttpRepository();
        public User User { get; private set; }

        private const string BaseUrl = "https://cismart.digital:5001/api/";
        private readonly HttpClient httpClient;
        private readonly LiteDatabaseDataStore store = DependencyService.Get<IDataStore<Assessment>>() as LiteDatabaseDataStore; // TODO must be changed to IDataStore<User>

        private HttpRepository() {
            var uri = new Uri(BaseUrl);
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true;
            httpClient = new HttpClient(handler);
            User = store.GetUser().Result;
            // httpClient.BaseAddress = uri;
        }

        public async Task PostPicture(string path) {
            var content = File.ReadAllBytes(path);
            var fileName = Path.GetFileName(path);
            await httpClient.PostAsJsonAsync($"{BaseUrl}picture/{User.Id}/{fileName}", content);
        }

        public async Task<int> PostPicture(string path, int assessmentId) {
            var content = File.ReadAllBytes(path);
            var fileName = Path.GetFileName(path);
            var response = await httpClient.PostAsJsonAsync($"{BaseUrl}picture/{User.Id}/{assessmentId}/{fileName}", content);
            var retVal = response.Content;
            if (retVal == null) {
                var length = await retVal.ReadFromJsonAsync<int>();
                return length;
            }
            return 0;
        }

        public async Task PostAssessment(Assessment assessment) {
            assessment.UserId = User.Id;
            var responseMessage = await httpClient.PostAsJsonAsync($"{BaseUrl}assessment", assessment);
            var content = responseMessage.Content;
            assessment.ObjectId = await content.ReadFromJsonAsync<int>();
        }

        public async Task PutAssessment(Assessment assessment) {
            await httpClient.PutAsJsonAsync($"{BaseUrl}assessment", assessment);
        }



        public async Task<bool> Login(string userName, string password) {
            User = await httpClient.GetFromJsonAsync<User>($"{BaseUrl}user/login/{userName}/{password}");
            if (User.Id > 0) {
                await store.SaveUser(User);
                return true;
            }
            return false;
        }

        public async Task<String[]> GetPicturesOnServer(int assessmentId) {
            var pictureNames = await httpClient.GetFromJsonAsync<string[]>($"{BaseUrl}picture/{User.Id}/{assessmentId}");
            return pictureNames;
		}

        private bool contain (string[] array, string entry) {
            foreach(var strg in array) {
                if (strg.Equals(entry)) {
                    return true;
				}
			}
            return false;
		}

        public async Task PostAssessmentAndPictures(Assessment assessment) {
            await PostAssessment(assessment);
            var pictures = assessment.PictureList;
            var picturesAllredyOnServer = await GetPicturesOnServer(assessment.Id);
            foreach (var picture in pictures) {
                if (contain(picturesAllredyOnServer, Path.GetFileName(picture))) {
                    continue;
				}
                await PostPicture(picture, assessment.Id);
			}
		}
	}
}

