using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace XamarinForms3RestApiApp
{
	public partial class MainPage : ContentPage
	{
        private const string Url = "http://jsonplaceholder.typicode.com/posts"; //This url is a free public api intended for demos
        private readonly HttpClient _client = new HttpClient(); //Creating a new instance of HttpClient. (Microsoft.Net.Http)
        private ObservableCollection<Post> _posts; //Refreshing the state of the UI in realtime when updating the ListView's Collection

        /// <summary>
        /// Constructor
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
        }
        /// <inheritdoc />
        /// <summary>
        /// This method gets called before the UI appears.
        /// Async and await to get the value of the Task and for user experience
        /// </summary>
        protected override async void OnAppearing()
        {
            string content = await _client.GetStringAsync(Url); //Sends a GET request to the specified Uri and returns the response body as a string in an asynchronous operation
            List<Post> posts = JsonConvert.DeserializeObject<List<Post>>(content); //Deserializes or converts JSON String into a collection of Post
            _posts = new ObservableCollection<Post>(posts); //Converting the List to ObservalbleCollection of Post
            MyListView.ItemsSource = _posts; //Assigning the ObservableCollection to MyListView in the XAML of this file
            base.OnAppearing();
        }

        /// <summary>
        /// Click event of the Add Button. It sends a POST request adding a Post object in the server and in the collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnAdd(object sender, EventArgs e)
        {
            Post post = new Post { Title = $"Title: Timestamp {DateTime.UtcNow.Ticks}" }; //Creating a new instane of Post with a Title Property and its value in a Timestamp format
            string content = JsonConvert.SerializeObject(post); //Serializes or convert the created Post into a JSON String
            await _client.PostAsync(Url, new StringContent(content, Encoding.UTF8, "application/json")); //Send a POST request to the specified Uri as an asynchronous operation and with correct character encoding (utf9) and contenct type (application/json).
            _posts.Insert(0, post); //Updating the UI by inserting an element into the first index of the collection 
        }

        /// <summary>
        /// Click event of the Update Button. It sends a PUT request updating the first Post object in the server and in the collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnUpdate(object sender, EventArgs e)
        {
            Post post = _posts[0]; //Assigning the first Post object of the Post Collection to a new instance of Post
            post.Title += " [updated]"; //Appending an [updated] string to the current value of the Title property
            string content = JsonConvert.SerializeObject(post); //Serializes or convert the created Post into a JSON String
            await _client.PutAsync(Url + "/" + post.Id, new StringContent(content, Encoding.UTF8, "application/json")); //Send a PUT request to the specified Uri as an asynchronous operation.
        }

        /// <summary>
        /// Click event of the Delete Button. It sends a DELETE request removing the first Post object in the server and in the 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnDelete(object sender, EventArgs e)
        {
            Post post = _posts[0]; //Assigning the first Post object of the Post Collection to a new instance of Post
            await _client.DeleteAsync(Url + "/" + post.Id); //Send a DELETE request to the specified Uri as an asynchronous 
            _posts.Remove(post); //Removes the first occurrence of a specific object from the Post collection. This will be visible on the UI instantly
        }
    }
}
