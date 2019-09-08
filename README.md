# Nuterra.NativeOptions
A recreation of the vanilla option tabs for mod options

# Options types
## Toggle
A toggle option that has a boolean value
```csharp
OptionToggle option = new OptionToggle("Option name", "Mod name");
// Setting default value
OptionToggle option = new OptionToggle("Option name", "Mod name", true);
```

## Keybinding
A keybind option that has a KeyCode value
```csharp
OptionKey option = new OptionKey("Option name", "Mod name", KeyCode.A);
```

## Text
A text field option that has a string value
```csharp
OptionText option = new OptionText("Option name", "Mod name");
// Setting default value
OptionText option = new OptionText("Option name", "Mod name", "Default Value");
// Setting max length
OptionText option = new OptionText("Option name", "Mod name", "Default Value", 12);
// Setting Content Type for numbers only
OptionText option = new OptionText("Option name", "Mod name", ContentType: InputField.ContentType.DecimalNumber);
```

## Slider
A slider option that has a float value
```csharp
OptionRange option = new OptionRange("Option name", "Mod name");
// Setting default value
OptionRange option = new OptionRange("Option name", "Mod name", 5f);
// Setting Min and Max values
OptionRange option = new OptionRange("Option name", "Mod name", MinValue: 5f, MaxValue: 10f);
// Rounding to multiples of 0.25
OptionRange option = new OptionRange("Option name", "Mod name", RoundTo: 0.25f);
```

## Dropdown
### List
A dropdown that has an int value
```csharp
OptionList<string> option = new OptionList<string>(
	"Option name",
	"Mod name",
	new List<string> { "1", "2", "3", "4", "5", "6", "7" }
);
//Selected value
String selected = option.Selected;
// Setting default value
OptionList<int> option = new OptionList<string>(
	"Option name",
	"Mod name",
	new List<int> { 1, 2, 3 },
	2
);
```
### Enum List
A dropdown that has an Enum value
```csharp
OptionListEnum<ChunkRarity> = new OptionListEnum<ChunkRarity>("Option name", "Mod name");
// Setting default value
OptionListEnum<ChunkRarity> = new OptionListEnum<ChunkRarity>("Option name", "Mod name", ChunkRarity.Common);
```

# Saved value
The options have an event that is fired when the options are saved.
You can use this event to get the SavedValue and save for the next session or to update another variable.
```csharp
bool saved;
OptionToggle option = new OptionToggle("Test save", "Test");
option.onValueSaved.AddListener(() =>
{
	saved = option.SavedValue;
});
```

You can also get or set the current value at any time using the Value property
```csharp
OptionToggle option = new OptionToggle("Test save", "Test");

bool current_value = option.Value;
option.Value = true;
```
