[![Build status](https://ci.appveyor.com/api/projects/status/k3hsg8anc4vrsup5?svg=true)](https://ci.appveyor.com/project/TheMaximum/gbxmapparser-net) [![NuGet version](https://img.shields.io/nuget/v/GBXMapParser.NET.svg)](https://www.nuget.org/packages/GBXMapParser.NET)

# GBXMapParser.NET
ManiaPlanet GBX map parser written in C# for .NET Core. It provides a static class allowing for parsing of a file via it's file location, a byte array or a stream.

## Credits
Most of this parser is based on the GBX map parser included in [PyPlanet](https://pypla.net), created by Toffe.

## Usage
The solution on the GitHub website contains one example, a console application that reads the map information from the included map file.

### Ways to parse map information
All three public methods (``ReadFile``, ``ReadBytes``, ``ReadStream``) in the end use the ``ReadStream`` method, and the data will be put into a stream if not being called directly with a stream. This is required to read the required data properly.

#### Read from file
Reading the map information directly from the file can be done by calling the ``ReadFile`` method of the ``MapParser`` class:
```csharp
MapInformation map = MapParser.ReadFile(fileName);
```

#### Read from byte array
Reading the map information from a byte array is also possible by calling the ``ReadBytes`` method of the ``MapParser`` class:
```csharp
MapInformation map = MapParser.ReadBytes(bytes);
```

#### Read from stream
Reading the map information from a stream (FileStream, MemoryStream, etc.) is also possible by calling the ``ReadStream`` method of the ``MapParser`` class:
```csharp
MapInformation map = MapParser.ReadStream(stream);
```

### Available information
The following information is being parsed (check out the example for a clearer picture):
* ``UId`` (``string``): map unique identifier;
* ``Name`` (``string``): name of the map (with styling);
* ``AuthorLogin`` (``string``): login of the author;
* ``TitleId`` (``string``): title in which the map is created (e.g. ``TMCanyon`` or ``SMStorm``);
* ``Environment`` (``string``): environment of the map (e.g. ``Canyon``);
* ``Mood`` (``string``): mood of the map (e.g. ``Day`` or ``Night``);
* ``DecorationEnvironmentId`` (``string``);
* ``DecorationEnvironmentAuthor`` (``string``);
* ``MapType`` (``string``): type of the map (e.g. ``TrackMania\Race``);
* ``MapStyle`` (``string``): style of the map (e.g. ``Full Speed``);
* ``MapTypeId`` (``uint``): type identifier of the map (corresponds with ``MapType``);
* ``IsMultilap`` (``bool``): whether the map is a multilap map;
* ``Laps`` (``uint``): amount of maps to be driven to set a time;
* ``Checkpoints`` (``uint``): amount of checkpoints in one lap of the map;
* ``Price`` (``uint``): indicates how heavy the map is for the computer (formerly "copper price");
* ``Editor`` (``string``): indicates which editor was used to create the map (either ``Simple`` or ``Advanced``);
* ``AuthorTime`` (``uint``): validation time in milliseconds;
* ``AuthorScore`` (``uint``): validation score;
* ``GoldTime`` (``uint``): gold time in milliseconds;
* ``SilverTime`` (``uint``): silver time in milliseconds;
* ``BronzeTime`` (``uint``): bronze time in milliseconds;
* ``AuthorVersion`` (``uint``): author version;
* ``AuthorNickName`` (``string``): nickname of the author at time of validation;
* ``AuthorZone`` (``string``): zone of the author at time of validation;
* ``AuthorExtra`` (``string``);
* ``HeaderXml`` (``string``): header information as XML string (**does not contain all data!**);
* ``HasThumbnail`` (``bool``): whether the map contains a thumbnail;
* ``Thumbnail`` (``byte[]``): thumbnail (JPEG, 512x512) as byte array;
* ``Comments`` (``string``): map comments by author.
