# FortuneCookie.PersonalizationEngine

The Fortune Cookie Personalization Engine for EPiServer extends the Visitor Groups functionality in EPiServer CMS 6 R2

## Requirements
The Personalization runs on EPiServer CMS 6 R2 (which can be downloaded from EPiServer World). The NuGet package (see below) is available compiled against ASP.NET 3.5 or ASP.NET 4.0

## Installation
The easiest way to install the PersonalizationEngine is to install the Nuget package directly from the EPiServer Nuget Feed http://nuget.episerver.com

Alternatively follow these steps,
- Add a reference to FortuneCookie.PersonalizationEngine.dll
- Copy the Views folder (and its content) into your web project into the following location ~/modules/PersonalizationEngine
- Add the following key to the EPiServer.Shell configuration section in your web.config file

<episerver.shell>
    <publicModules rootPath="~/modules/" autoDiscovery="Minimal">
        <add name="PersonalizationEngine">
	    <assemblies>
	        <add assembly="FortuneCookie.PersonalizationEngine" />
            </assemblies>
	</add>
    </publicModules>
</episerver.shell>

Did I mention that Nuget will do all of this for you at the click of a button?

## Future Enhancements
- We've got a lot of ideas about future enhancements to the Personalization Engine. Some ideas that may either be rolled into this project or a separate related project
- More ContentProviders
- Provider specific to the EPiServer Commerce API
- Recently viewed pages ContentProvider - to show you pages you’ve recently visited
- GeoTaggedContentProvider - some kind of tag matching based around personalization that looks for content that is tagged as close to you
- More UI examples - such as Custom Properties (@croweman is already working on this), a dynamic content plugin, possible some form of Composer module
- Would love to hear your ideas............

## About the project
This open source project was conceived and developed by Mark Everard. All feedback is most welcome and greatly appreciated. Please submit feature requests or bugs using the Issue Tracker