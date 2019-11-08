using System;
using System.Collections.Generic;
using System.Text;

namespace Xam.CrossKeyboard
{
    /// <summary>
    /// Interface of XKeyboard
    /// </summary>
    public interface IXKeyboard
    {
        /// <summary>
        /// Starts listening for keyboard change events.
        /// Invoked when keyboard state is changed. <see cref="KeyboardStateEventArgs"/>
        /// </summary>
        event EventHandler<KeyboardStateEventArgs> KeyboardStateChanged;
    }

    /// <summary>
    /// KeyboardStateEventArgs containing <see cref="KeyboardEventTypes"/> and <see cref="KeyboardHeight"/>
    /// </summary>
    public class KeyboardStateEventArgs : EventArgs
    {
        /// <summary>
        /// Type of keyboard event. <see cref="KeyboardEventTypes"/>
        /// </summary>
        public KeyboardEventTypes EventType { get; set; }

        /// <summary>
        /// Height of keyboard.
        /// </summary>
        public float? KeyboardHeight { get; set; } = null;

        /// <summary>
        /// WillShow: EventArgs sent when keyboard will show.
        /// </summary>
        /// <returns><c>KeyboardEventType.WillShow</c></returns>
        public static KeyboardStateEventArgs WillShow()
        {
            return new KeyboardStateEventArgs
            {
                EventType = KeyboardEventTypes.WillShow
            };
        }

        /// <summary>
        /// WillShow: EventArgs sent when keyboard will hide.
        /// </summary>
        /// <returns><c>KeyboardEventType.WillHide</c></returns>
        public static KeyboardStateEventArgs WillHide()
        {
            return new KeyboardStateEventArgs
            {
                EventType = KeyboardEventTypes.WillHide
            };
        }

        /// <summary>
        /// WillShow: EventArgs sent when keyboard did hide.
        /// </summary>
        /// <returns><c>KeyboardEventType.DidHide</c> and <c>KeyboardHeight</c></returns>
        public static KeyboardStateEventArgs DidHide(float keyboardHeight)
        {
            return new KeyboardStateEventArgs
            {
                EventType = KeyboardEventTypes.DidHide,
                KeyboardHeight = keyboardHeight
            };
        }

        /// <summary>
        /// WillShow: EventArgs sent when keyboard did show.
        /// </summary>
        /// <returns><c>KeyboardEventType.DidShow</c> and <c>KeyboardHeight</c></returns>
        public static KeyboardStateEventArgs DidShow(float keyboardHeight)
        {
            return new KeyboardStateEventArgs
            {
                EventType = KeyboardEventTypes.DidShow,
                KeyboardHeight = keyboardHeight
            };
        }
    }
    /// <summary>
    /// Types of keyboard events.
    /// </summary>
    public enum KeyboardEventTypes
    {
        /// <summary>
        /// WillShow: EventType before keyboard will be visible on the  but it is not shown yet.
        /// </summary>
        WillShow,
        /// <summary>
        /// DidShow: EventType after keyboard became visible on the screen.
        /// </summary>
        DidShow,
        /// <summary>
        /// WillHide: EventType before keyboard is hidden.
        /// </summary>
        WillHide,
        /// <summary>
        /// DidHide: EventTye after keyboard is hidden.
        /// </summary>
        DidHide
    }
}
