using Makini.Src.Constants;
using Makini.Src.Models;
using Makini.Src.Services;
using System.Windows.Input;
using Newtonsoft.Json;
using VM = Makini.Src.ViewModels; 

namespace Makini.Src.ViewModels.ChatViewModels
{
    public class ChatViewModel : VM.BaseViewModel
    {
        #region fields

        private HubConnection _connection;
        private string outGoingText = string.Empty;
        private bool contentVisibility;

        #endregion


        // ctor

        public ChatViewModel()
        {
            Messages = new ObservableRangeCollection<Message>();
            _connection = new HubConnectionBuilder().WithUrl(AppConstants.ChatUri + "MakiniMessage").Build();
            _connection.On<Message>("MakiniMessage", (message) =>
            {
                var mainMessage = new Message()
                {
                    Text = message.Text,
                    IsMine = message.IsMine,
                    DateAndTime = message.DateAndTime
                };
                Device.BeginInvokeOnMainThread(() =>
                {
                    Messages.Add(mainMessage);
                });
            });
            _connection.StartAsync().ConfigureAwait(false);
            InitializeMockAsync();
        }


        #region properties

        public ObservableRangeCollection<Message> Messages { get; }

        public string OutGoingText      
        {
            get { return outGoingText; }
            set 
            { 
                outGoingText = value;
                OnPropertyChanged(nameof(OutGoingText));
            }
        }

        public  bool ContentVisibility
        {
            get { return contentVisibility; }
            set 
            {
                contentVisibility = value;
                OnPropertyChanged(nameof(ContentVisibility));
            }
        }


        #endregion

        public ICommand SendMessageCommand => new Command(async () =>
        {
            var afrilearnText = new Message()
            {
                Text = OutGoingText,
                DateAndTime = DateTime.Now.ToString(),
                IsMine =  true
            };    
            
            await _connection.InvokeAsync("MakiniMessage", afrilearnText);

            OutGoingText = string.Empty;                    
        });

        public async void InitializeMockAsync()
        {
            IsBusy = true;
            ContentVisibility = false;
            Messages.ReplaceRange(
                new List<Message>
                {
                    new Message
                    {
                        Text = "Hi Squirrel! \uD83D\uDE0A", 
                        IsMine = true, 
                        DateAndTime = DateTime.Now.AddMinutes(-25).ToString()
                    },
                    new Message
                    {
                        Text = "Hi Baboon, How are you? \uD83D\uDE0A",
                        IsMine =  false, 
                        DateAndTime = DateTime.Now.AddMinutes(-24).ToString()
                    },
                    new Message 
                    { 
                        Text = "We've a party at Mandrill's. Would you like to join? We would love to have you there! \uD83D\uDE01", 
                        IsMine = true,
                        DateAndTime = DateTime.Now.AddMinutes(-23).ToString()
                    },
                    new Message 
                    {
                        Text = "You will love it. Don't miss.", 
                        IsMine = true, 
                        DateAndTime = DateTime.Now.AddMinutes(-23).ToString()
                    },
                    new Message 
                    { 
                        Text = "Sounds like a plan. \uD83D\uDE0E", 
                        IsMine = false, 
                        DateAndTime = DateTime.Now.AddMinutes(-23).ToString()
                    },

                    new Message 
                    {
                        Text = "\uD83D\uDE48 \uD83D\uDE49 \uD83D\uDE49",
                        IsMine = false, 
                        DateAndTime = DateTime.Now.AddMinutes(-20).ToString()
                    },
                     new Message
                    {
                        Text = "I made it, stand on roof tops and shout, '.... coode!'",
                        IsMine = false,
                        DateAndTime = DateTime.Now.AddMinutes(-23).ToString()
                    },
                });

            if (InternetService.Internet())
            {
                // var appUser = await BlobCache.UserAccount.GetObject<AppUser>("appUser");
                var appUser = new AppUser();

                var authHttpClient = new HttpClientService(appUser.AuthKey);

                var serverMessage = await authHttpClient.Get("Messages/get");
              
                if (serverMessage != "")
                {
                    var messages = JsonConvert.DeserializeObject<ObservableRangeCollection<Message>>(serverMessage);
                   
                    foreach (var message in messages)
                    {
                        Messages.Add(new Message()
                        {
                            DateAndTime = message.DateAndTime,
                            IsMine = message.IsMine,
                            Sender = message.Sender,
                            Text = message.Text
                        });
                    }
                }
            }

            IsBusy = false;
            ContentVisibility = true;
        }
    }
}
