using Parse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Chat_App.ViewModel
{
    class ChattingMessage
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string MessageInfo { get; set; }
        public DateTime TimeStamp { get; set; }
    }


    class MessageViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ChattingMessage> messagesCollection = null;
        private ObservableCollection<ChattingMessage> filteredMessageCollection = null;
        private string currentFriendName = string.Empty;
        private string currentUserName = "";


        private DateTime lastMessageTimeStamp = new DateTime();
        private DateTime defaultDT = new DateTime();

        public MessageViewModel()
        {
            this.messagesCollection = new ObservableCollection<ChattingMessage>();
            this.filteredMessageCollection = new ObservableCollection<ChattingMessage>();
        }

       

        public ObservableCollection<ChattingMessage> FilteredMessageCollection
        {
            get
            {
                return filteredMessageCollection;
            }
            set
            {
                filteredMessageCollection = value;
                NotifyPropertychanged();
            }
        }


        public string FriendName
        {
            get
            {
                return this.currentFriendName;
            }
            set
            {
                if (value != currentFriendName)
                {
                    this.currentFriendName = value;

                    this.FilteredMessageCollection.Clear();

                    updateFilteredMessageCollection();
                    NotifyPropertychanged();
                }
            }

        }

        public void updateFilteredMessageCollection()
        {
            if (this.messagesCollection.Count == 0)
                return;

            var collection = from message in this.messagesCollection
                                            where message.Sender == FriendName || message.Receiver == FriendName
                                            orderby message.TimeStamp
                                            select message;

            if (filteredMessageCollection.Count == 0)
            {
                foreach (ChattingMessage m in collection)
                {
                    filteredMessageCollection.Add(m);
                }
            }
            else
            {
                foreach (ChattingMessage m in collection)
                {
                    if (m.TimeStamp > filteredMessageCollection.Last().TimeStamp)
                    {
                        filteredMessageCollection.Add(m);
                    }
                }
            }
           
        }



        public void setCurrentUserName(string userName)
        {
            this.currentUserName = userName;
        }

        public async void LoadAllRelevantMessages()
        {
            if(this.lastMessageTimeStamp != defaultDT)
            {
                var query = from message in ParseObject.GetQuery("message")
                            where( message.Get<string>("sender") == this.currentUserName || message.Get<string>("receiver") == this.currentUserName )
                            && message.CreatedAt > this.lastMessageTimeStamp
                            orderby message.Get<DateTime>("createdAt")
                            select message;

                IEnumerable<ParseObject> results = await query.FindAsync();
                foreach (ParseObject o in results)
                {
                    ChattingMessage cm = new ChattingMessage();
                    cm.Sender = o.Get<string>("sender");
                    cm.Receiver = o.Get<string>("receiver");
                    cm.MessageInfo = o.Get<string>("message");
                    cm.TimeStamp = (DateTime)o.CreatedAt;
                    this.messagesCollection.Add(cm);

                    this.lastMessageTimeStamp = cm.TimeStamp; 
                }

               
            }
            else
            {
                this.messagesCollection.Clear();

                var query = from message in ParseObject.GetQuery("message")
                            where message.Get<string>("sender") == this.currentUserName || message.Get<string>("receiver") == this.currentUserName
                            orderby message.Get<DateTime>("createdAt")
                            select message;

                IEnumerable<ParseObject> results = await query.FindAsync();

                foreach (ParseObject o in results)
                {
                    ChattingMessage cm = new ChattingMessage();
                    cm.Sender = o.Get<string>("sender");
                    cm.Receiver = o.Get<string>("receiver");
                    cm.MessageInfo = o.Get<string>("message");
                    cm.TimeStamp = (DateTime)o.CreatedAt;
                    this.messagesCollection.Add(cm);
                }

                this.lastMessageTimeStamp = this.messagesCollection.Last().TimeStamp;
            }

           
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertychanged([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
