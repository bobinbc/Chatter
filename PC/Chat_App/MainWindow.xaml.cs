using Parse;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chat_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Chatting_Window wnd = null;

        public MainWindow()
        {
            InitializeComponent();

            ParseClient.Initialize("6GDVKxEbjD12uv4BCBwBG14O4A0lha677JrSFOt7", "VyrQCedwl8mgM9nObLoB9GYbyH1DKHBJ4vj7jAX7");

            wnd = new Chatting_Window();
            wnd.Closed += wnd_Closed;
        }

        

        void wnd_Closed(object sender, EventArgs e)
        {
            this.Close();
        }

       

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            string userName = this.tb_UserName.Text;
            string password = this.pb_Password.Password;

            if(string.IsNullOrEmpty(userName))
            {
               MessageBox.Show("Please input your username.");
               return;
            }

            if(string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please input your password");
                return;
            }

            ParseUser.LogInAsync(userName, password).ContinueWith(t =>
            {
                if (t.IsFaulted || t.IsCanceled)
                {
                    MessageBox.Show("Login failed!");
                    return;
                }
                else
                {
                    
                    this.Dispatcher.BeginInvoke(new Action(() => wnd.setCurrentUser(userName)));
                    this.Dispatcher.BeginInvoke(new Action(() => this.Hide()));
                    this.Dispatcher.BeginInvoke(new Action(() => wnd.loadAllFriends()));
                    this.Dispatcher.BeginInvoke(new Action(() => wnd.Show()));

                    //this.Dispatcher.BeginInvoke(new Action(() => wnd.LoadMessage()));
                   
                }
            });

         
        }

        private void showChattingDlg() 
        {
           
        }

    }
}
