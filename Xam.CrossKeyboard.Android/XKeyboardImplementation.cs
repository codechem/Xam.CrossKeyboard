using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;

namespace Xam.CrossKeyboard
{
    /// <summary>
    /// Interface for CrossKeyboard
    /// </summary>
    /// 
    public class XKeyboardImplementation :
        Java.Lang.Object,
        ViewTreeObserver.IOnGlobalLayoutListener,
        IXKeyboard,
        IDisposable
    {
        private readonly Activity _activity;
        private readonly View _parentView;
        private readonly View _resizableView;
        private readonly bool _isShowing;
        private int subscribers;
        private float lastKeyboardHeight = -1;
        private KeyboardEventTypes _lastState = KeyboardEventTypes.DidHide;
        private PopupWindow _popupWindow;
        private InputMethodManager _inputMethodManager;
        private event EventHandler<KeyboardStateEventArgs> _keyboardStateChanged;
        private InputMethodManager InputMethodManager
        {
            get
            {
                if (_inputMethodManager == null || _inputMethodManager.Handle == IntPtr.Zero)
                    _inputMethodManager = (InputMethodManager)Application.Context.GetSystemService(Context.InputMethodService);
                return _inputMethodManager;
            }
        }

        /// <summary>
        /// Starts listening for keyboard change events.
        /// Invoked when keyboard state is changed. <see cref="KeyboardStateEventArgs"/>
        /// </summary>
        public event EventHandler<KeyboardStateEventArgs> KeyboardStateChanged
        {
            add
            {
                subscribers++;
                _keyboardStateChanged += value;
                if (subscribers == 1)
                {
                    OnResume();
                }
            }
            remove
            {
                subscribers -= 1;
                subscribers = Math.Max(subscribers, 0);
                _keyboardStateChanged -= value;
                if (subscribers <= 0)
                {
                    OnPause();
                }
            }
        }

        /// <summary>
        /// Constructor of the KeyboardImplementation.
        /// </summary>
        public XKeyboardImplementation()
        {
            _activity = (Activity)Xamarin.Forms.Forms.Context;
            _parentView = _activity.Window.FindViewById(Android.Resource.Id.Content);
            _popupWindow = new PopupWindow(_activity)
            {
                SoftInputMode = SoftInput.AdjustResize | SoftInput.StateAlwaysVisible,
                InputMethodMode = Android.Widget.InputMethod.Needed,
                Width = 0,
                Height = ViewGroup.LayoutParams.MatchParent,
                ContentView = new FrameLayout(_activity)
                {
                    LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)
                }
            };

            _resizableView = _popupWindow.ContentView;
        }

        /// <summary>
        /// Interface implementation for a callback when the global layout state or 
        /// the visibility of views within the view tree changes. <see cref="ViewTreeObserver.IOnGlobalLayoutListener"/>
        /// </summary>
        public void OnGlobalLayout()
        {
            lastKeyboardHeight = CalculateKeyboardHeight();
            var nextState = CalculateKeyboardsNextState(_lastState, lastKeyboardHeight, InputMethodManager.IsAcceptingText);
            if (_lastState == nextState) return;
            _lastState = nextState;

            if (_lastState == KeyboardEventTypes.DidHide)
            {
                _keyboardStateChanged?.Invoke(this, new KeyboardStateEventArgs
                {
                    EventType = KeyboardEventTypes.WillHide,
                    KeyboardHeight = lastKeyboardHeight
                });
            }

            if (_lastState == KeyboardEventTypes.DidShow)
            {
                _keyboardStateChanged?.Invoke(this, new KeyboardStateEventArgs
                {
                    EventType = KeyboardEventTypes.WillShow,
                    KeyboardHeight = lastKeyboardHeight
                });
            }

            _keyboardStateChanged?.Invoke(this, new KeyboardStateEventArgs
            {
                EventType = nextState,
                KeyboardHeight = lastKeyboardHeight
            });
        }

        /// <summary>
        /// Override of Object.Dispose
        /// Cleanup before disposing.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                OnPause();
            base.Dispose(disposing);
        }

        private void OnResume()
        {
            _parentView.Post(() =>
            {
                _resizableView.ViewTreeObserver.AddOnGlobalLayoutListener(this);
                if (!_isShowing && _parentView?.WindowToken != null)
                {
                    _popupWindow.ShowAtLocation(_parentView, GravityFlags.NoGravity, 0, 0);
                }
            });
        }

        private void OnPause()
        {
            _resizableView.ViewTreeObserver.RemoveOnGlobalLayoutListener(this);
            _popupWindow.Dismiss();
        }

        private static KeyboardEventTypes CalculateKeyboardsNextState(KeyboardEventTypes currentState, float keyboardHeight, bool isAcceptingText)
        {
            var isKeyboardVisible = keyboardHeight > 10;
            if (!isKeyboardVisible)
                return KeyboardEventTypes.DidHide;
            //NOTE: In case the keyboard is visible(at least partially visible) we continue as follows:
            if (isAcceptingText)
                return KeyboardEventTypes.DidShow;
            //NOTE: If the keyboard is not accepting input yet it means it's not reached is final state yet, it is either in a process of hiding or showing it self
            if (currentState == KeyboardEventTypes.DidShow)
                return KeyboardEventTypes.WillHide;
            if (currentState == KeyboardEventTypes.DidHide)
                return KeyboardEventTypes.WillShow;
            return currentState;
        }

        private float CalculateKeyboardHeight()
        {
            var screenSize = new Point();
            _activity.WindowManager.DefaultDisplay.GetSize(screenSize);
            var rect = new Rect();
            _resizableView.GetWindowVisibleDisplayFrame(rect);
            var keyboardHeight = screenSize.Y - rect.Bottom;
            var keyboardHeightDp = ConvertPixelsToDp(keyboardHeight);
            return keyboardHeightDp;
        }

        private float ConvertPixelsToDp(int px)
        {
            var resources = _activity.Resources;
            DisplayMetrics metrics = resources.DisplayMetrics;
            float dp = px / ((float)metrics.DensityDpi / (float)DisplayMetricsDensity.Default);
            return dp;
        }
    }
}
