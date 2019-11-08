using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xam.CrossKeyboard;
using Xamarin.Forms;

namespace XKeyboardSample
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public string KeyboardHeight { get; set; }
        public string KeyboardEventType { get; set; }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            XKeyboard.Current.KeyboardStateChanged += Current_KeyboardStateChanged;
        }

        private void Current_KeyboardStateChanged(object sender, KeyboardStateEventArgs e)
        {
            KeyboardHeight = e.KeyboardHeight.ToString();
            KeyboardEventType = e.EventType.ToString();

            OnPropertyChanged(nameof(KeyboardHeight));
            OnPropertyChanged(nameof(KeyboardEventType));

            Debug.Print($"KeyboardHeight: {e.KeyboardHeight}    KeyboardEventType: {e.EventType}");
        }
    }
}
