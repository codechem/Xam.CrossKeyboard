## Keyboard plugin for Xamarin forms
Simple cross platform plugin to get keyboard events and keyboard height on crossplatform level.

NOTE: Restoing package will give warning of using .NETFramework but it will work perfectly fine. Fix is coming.

## Nuget

Available on NuGet: Xam.CrossKeyboard

## Documentation
1. Install nuget package
2. Subscribe to ```KeyboardStateChanged``` event that is provided by ```XKeyboard.Current``` and listen to triggered changes.
This event sends ```KeyboardStateEventArgs``` as argument, which contains ```KeyboardEventType``` and current visible ```KeyboardHeight```.

KeyboardEventTypes can be: ```WillShow```, ```DidShow```, ```WillHide```, ```DidHide```.

## Sample
```
XKeyboard.Current.KeyboardStateChanged += Current_KeyboardStateChanged;

private void Current_KeyboardStateChanged(object sender, KeyboardStateEventArgs e)
{
    KeyboardHeight = e.KeyboardHeight.ToString();
    KeyboardEventType = e.EventType.ToString();
}
```
For more detaild sample click [here](https://github.com/kocevilija/Xam.CrossKeyboard/tree/master/Sample)

### License
The MIT License (MIT). See [License File](https://github.com/kocevilija/Xam.CrossKeyboard/blob/master/LICENSE)
