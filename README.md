# KsWare.Screen

This library provides information for displays (screens, monitors). 
The name is derived von [System.Windows.Forms.Screen](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.screen?view=windowsdesktop-6.0). This is my implementation targeting WPF.

This is a really early stage of development. But is it usable.

## API
- [DisplayInfo-class](#DisplayInfo-class)
- [WindowExtensions-class](#WindowExtensions-class)

### DisplayInfo-class

Contains static methods to get instances of DisplayInfo.

The DisplayInfo instance contains information about a specific display.

### WindowExtensions-class

contains "display" specifig extention methods for the [Window-class](https://docs.microsoft.com/en-us/dotnet/api/system.windows.window?view=windowsdesktop-6.0)

## Naming mismatch

**KsWare.Screen** assembly and **DisplayInfo** class  
The name "KsWare.Screen" is a working title and subject to change. Microsoft API uses "display". So I named all classes also Display.

**KsWare.Windows.Screen** assembly and **Screen** class  
One could assume that it is a 1:1 implementation of Winfowms [System.Windows.Forms.Screen](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.screen?view=windowsdesktop-6.0) is, but this is not intended.

**KsWare.Windows.Display** assembly and **DisplayInfo** class  
This seems to be a good name.

**Screen**: a flat surface in a cinema, on a television, or as part of a computer, on which pictures or words are shown

**Screen** In a computer display , the screen is the physical surface on which visual information is presented. This surface is usually made of glass. The screen size is measured from one corner to the opposite corner diagonally.

**Display** A display is a computer output surface and projecting mechanism that shows text and often graphic images to the computer user, using a cathode ray ...