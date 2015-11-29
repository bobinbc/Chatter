using Chat_App.ViewModel;
using Parse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Chat_App
{
    /// <summary>
    /// Interaction logic for Chatting_Window.xaml
    /// </summary>
    public partial class Chatting_Window : Window
    {
        ObservableCollection<FriendItem> List_friendName = new ObservableCollection<FriendItem>();
        MessageViewModel vm = null;
        ParseUser currentUser;

        string currentUserName;

        public Chatting_Window()
        {
            InitializeComponent();
            vm = new MessageViewModel();
            this.DataContext = vm;
            this.List_message.ItemsSource = vm.FilteredMessageCollection;
            //getCurrentUsername();
        }

        public void setCurrentUser(string userName)
        {
            this.currentUserName = userName;
            vm.setCurrentUserName(this.currentUserName);
        }

        public void getCurrentUsername()
        {
            currentUser = ParseUser.CurrentUser;
            if (currentUser == null)
                return;

            vm.setCurrentUserName(currentUser.Username);
        }

        public async void loadAllFriends() 
        {
            this.friend_list.Items.Clear();


            var query = ParseObject.GetQuery("user_friend")
            .WhereEqualTo("userName", this.currentUserName);

            IEnumerable<ParseObject> results = await query.FindAsync();

            if (results.Count() == 0)
                return;

            foreach (ParseObject o in results) 
            {
                FriendItem item = new FriendItem();
                item.FriendName = o.Get<string>("friendName");
                item.ImagePath = getImagePath(item.FriendName);
                List_friendName.Add(item);
            }

            this.friend_list.ItemsSource = List_friendName;
            this.friend_list.SelectedIndex = 0;

        }


        public void LoadMessage()
        {



           // vm.LoadAllRelevantMessages();
        }

        string getImagePath(string friendName) 
        {
            if (string.IsNullOrEmpty(friendName))
                return string.Empty;


            return "Image/" + friendName + ".jpg";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartTimer();  
        }

        private void StartTimer()
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            vm.LoadAllRelevantMessages();
            vm.updateFilteredMessageCollection();
        }


        private void btn_send_Click(object sender, RoutedEventArgs e)
        {
            string messageSender = this.currentUserName;
            string messageReceiver = vm.FriendName;
            string messageInfo = this.sending_message.Text;

            if (string.IsNullOrEmpty(messageSender) || string.IsNullOrWhiteSpace(messageSender) 
                || string.IsNullOrEmpty(messageReceiver) || string.IsNullOrWhiteSpace(messageReceiver) 
                || string.IsNullOrEmpty(messageInfo) || string.IsNullOrWhiteSpace(messageInfo))
                return;            

            ParseObject newMessage = new ParseObject("message");
            newMessage["message"] = messageInfo;
            newMessage["sender"] = messageSender;
            newMessage["receiver"] = messageReceiver;
            newMessage.SaveAsync();

            this.sending_message.Text = "";

        }

       
        private void friend_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FriendItem item = (FriendItem)this.friend_list.SelectedItem;
            string friendName = item.FriendName;

            vm.FriendName = friendName;

        }

    
    }

    public class FriendItem
    {
        public string ImagePath {get; set;}
        public string FriendName {get; set;}
    }


}
