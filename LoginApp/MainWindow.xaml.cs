using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Windows;
using Newtonsoft.Json;

namespace LoginApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CancellationToken ct;
        private static string Uri { get; set; }
        public MainWindow()
        {
            Uri = "https://dev.prezentor.com/auth/local";
            InitializeComponent();
        }

        async private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("email", emailTextBox.Text.ToString()),
                new KeyValuePair<string, string>("password", passwordTextBox.Text.ToString())
            });

            try
            {                
                var client = new HttpClient();
                
                HttpResponseMessage response = await client.PostAsync(Uri, formContent, ct);
                //var content = await response.Content.ReadAsStringAsync();
                var task = response.Content.ReadAsStringAsync();
                var content = await task;

                AuthorizationToken t = JsonConvert.DeserializeObject<AuthorizationToken>(content);
                
                string token = t.Token;
                string message = t.Message;

                if (!string.IsNullOrEmpty(token))
                {
                    tokenTextBox.Text = token;
                }
                else
                {
                    tokenTextBox.Text = "ERROR - " + message;
                }                
            }

            catch (Exception ex)
            {
                tokenTextBox.Text += "Download error...";
                tokenTextBox.Text = ex.Message;
            }
            
        }
    }
}
