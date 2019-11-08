using System;
namespace Xam.CrossKeyboard
{
    public class XKeyboardImplementation: IXKeyboard
    {
        public event EventHandler<KeyboardStateEventArgs> KeyboardStateChanged;
    }
}
