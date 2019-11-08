using Foundation;
using System;
using UIKit;

namespace Xam.CrossKeyboard
{
    /// <summary>
    /// Interface for Keyboard
    /// </summary>
    public class XKeyboardImplementation : IXKeyboard
    {
        /// <summary>
        /// Starts listening for keyboard change events.
        /// Invoked when keyboard state is changed. <see cref="KeyboardStateEventArgs"/>
        /// </summary>
        public event EventHandler<KeyboardStateEventArgs> KeyboardStateChanged;

        /// <summary>
        /// Constructor of the CrossKeyboardImplementation.
        /// </summary>
        public XKeyboardImplementation()
        {
            UIKeyboard.Notifications.ObserveDidHide((s, eventArgs) => NotifyKeyboardState(KeyboardEventTypes.DidHide, eventArgs));
            UIKeyboard.Notifications.ObserveWillShow((s, eventArgs) => NotifyKeyboardState(KeyboardEventTypes.WillShow, eventArgs));
            UIKeyboard.Notifications.ObserveDidShow((s, eventArgs) => NotifyKeyboardState(KeyboardEventTypes.DidShow, eventArgs));
            UIKeyboard.Notifications.ObserveWillHide((s, eventArgs) => NotifyKeyboardState(KeyboardEventTypes.WillHide, eventArgs));
        }

        /// <summary>
        /// Saving the last state of keyboard.
        /// </summary>
        protected KeyboardEventTypes _lastState = KeyboardEventTypes.DidHide;

        private float _keyboardHeight;
        private void NotifyKeyboardState(KeyboardEventTypes eventType, UIKeyboardEventArgs eventArgs)
        {
            _lastState = eventType;

            if (eventType == KeyboardEventTypes.DidHide)
                _keyboardHeight = 0;
            else
                _keyboardHeight = CalculateKeyboardHeight(eventArgs);

            KeyboardStateChanged?.Invoke(this, new KeyboardStateEventArgs
            {
                EventType = eventType,
                KeyboardHeight = _keyboardHeight
            });
        }

        private float CalculateKeyboardHeight(UIKeyboardEventArgs eventArgs)
        {
            NSValue result = (NSValue)eventArgs.Notification.UserInfo.ObjectForKey(new NSString(UIKeyboard.FrameEndUserInfoKey));
            if (result is null) return 0;
            return result.RectangleFValue.Size.Height;
        }

    }
}
