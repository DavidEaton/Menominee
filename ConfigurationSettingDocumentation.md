# New Feature: Application Settings Configuration
- [HOME](ReadMe.md)
## Description
The Application Settings Configuration is a new feature that leverages Entity Framework It allows users to customize their 
application experience by providing a flexible system for managing various application settings throughout the UI.
These settings are saved securely within the tenant's database, specifically within the new Settings table.
This feature introduces several key components to the code base, including:

1.) Domain: The new Setting Configuration Domain represents the data structure of the application settings. It defines the 
properties and attributes that can be customized by users.

2.) Enums: Two new enums have been added to enhance the settings configuration, SettingGroup and SettingName. These enums provide 
predefined options for the setting names and grouping options associated with the settings.

3.) Setting Repository: The Setting Repository is responsible for handling interactions with the database related to 
application settings. It provides the necessary (Create, Read, Update) operations for managing settings.

4.) Controller: The Controller acts as the interface between the user interface and the backend, handling user requests to view, 
modify, and save application settings.

5.) Settings Helper: The Settings Helper is a utility class that simplifies the process of accessing and applying application 
settings throughout the code base. It centralizes the logic for converting the new Entity into various DTOS ans well as a helper 
method that determines the type of the value when converting to an entity to save to the database.

By integrating this new feature, users can benefit from a more tailored and personalized experience within the application.

### Installation
To enable the Application Settings Configuration feature and update the local database, follow the steps below using 
Visual Studio and the Package Manager Console:

Database Migration:
First, ensure that you have the latest version of the code base from the repository. Open Visual Studio and open the solution 
for the project.

In Visual Studio, open the Package Manager Console by navigating to "Tools" > "NuGet Package Manager" > "Package Manager Console."

Run the following command in the Package Manager Console to apply it to the local database using the following command in the Package 
Manager Console:

Update-Database

This command will update the local database schema to include the necessary tables for storing application settings.

### Usage
The Application Settings Configuration feature will allow users to customize their application experience by adjusting various 
settings through the use of the Enums SettingName which will store the name for all of the settings options the application has.


#### Adding New SettingName Options
As a developer, if you want to add a new setting option to the list of settings provided they can be added by navigating to:
"menominee\Menominee.Domain\Entities\Settings\SettingName.cs"
There you will add a new setting to the end of list and also provide a Display Name. 

#### Adding New SettingGroup Options
As a developer, if you want to add a new setting group option to the list of setting groups provided they can be added by navigating to:
"menominee\Menominee.Domain\Entities\Settings\SettingGroup.cs"
There you will add a new setting group to the end of list and also provide the next increment of integer to be associated with that setting group enum. 

#### Adding New Setting Type Options
As a developer, if you want to add a new setting type to be used for the settings then you will have to navigate to:
menominee\Menominee.Shared\Models\Settings\SettingHelper.cs
navigate to the GetType method and from there you can add a a new if statement and attempt to TryPare the new type and set the typStr as equal to the value type outputted by the parse using GetType().Name.

#### Setting a New Value
To set a new value for a specific setting, the user provides the SettingName enum, SettingGroup enum, and the value of the setting 
as a string through the UI.

Once the WriteDto is sent to the SettingRepository it is converted into an SettingConfiguration entity and the SettingsHelper will 
determine the type of the value and save the type whether it be a boolean, integer, float or string. The type will be saved in the database 
as a string in the database. The reason the type it saved is so that it can be utilized on the UI when being read if the type is ever 
needed or needs to be converted to its intended type.

#### Bulk Adds and Updates of Settings
To optimize user experience, settings are usually saved or updated in bulk when users choose multiple settings simultaneously on the UI. 
The controller currently handles bulk updates and adds to the database using a list of WriteDtos.

#### Displaying Updated Settings
After the settings are retrieved, added or updated, the controller responds with an updated list of read-only settings retrieved 
from the database. This ensures that the user interface always reflects the latest settings on the UI.

#### How To: Adding A New Setting To The Database
To add a new setting to the database a POST call needs to be made to /api/settings.

The method takes in a list of SettingToWrite.

Each SettingToWrite dto will need a SettingName enum, SettingGroup enum as well as the value in the form of a string.

Example of SettingToWrite dto:

{
    "SettingName": "ShowAfterCustomerLookup",
    "SettingValue": "false",
    "SettingGroup": "0"
}

If a setting is provided in the the list that is already in the database with that SettingName, then the method will update that value with the setting name.

#### How To: Updating Settings In The Database
To update an existing setting in the database a PUT call will need to be made to api/settings.

The method takes in a list of SettingsToWrite dtos.

Each SettingToWrite dto will need a SettingName enum, SettingGroup enum as well as the value in the form of a string.

Example of SettingToWrite dto:

{
    "SettingName": "ShowAfterCustomerLookup",
    "SettingValue": "false",
    "SettingGroup": "0"
}

When a setting is updated. only the value is updated. The type and the setting name are not able to be updated with the current logic. 

#### How To: Get List Of All Settings From The Database
To get a list of all of the setting a GET call will need to be made to api/settings.

The call will return all of the setting in the table in a list of SettingToRead dtos.

A single SettingToRead dto will look like the following:
{
    "settingName": 0,
    "settingValue": "false",
    "settingValueType": "Boolean",
    "settingGroup": 0
}

#### How To: Get a Single Setting From The Database
To get a single setting from the database a GET call will new to be made to api/settings/{settingName} utilizing the numerical SettingName enum rather than its string value. 

#### How To: Get a List of Settings Based on Their Setting Group From The Database
To get a list of settings from the database a GET method will need to be made to api/settings/group/{settingGroup} utilizing the numerical SettingGroup enum associated with that group.

### Testing
The Application Settings Configuration feature introduces unit tests for the new SettingsConfiguration domain class. It tests Create, 
the various Set for the properties and the UpdateSettingsProperties.

### Considerations
We need to determine how the initial settings for the application are going to be added to the database. If during onboarding, are they seeded
upon adding a new tenant and given default values? From there, would the settings in the database only able to updated and no new ones added with 
that same settingName?

Currently when adding new settings to the database there are no checks in place that inhibit adding settings with the same setting name(SettingName enum)
the represents a single unique setting. This can could cause issues on the UI once settings are in place as there would be multiple settings having the same
setting name which would make it difficult to track the correct setting that the user intended to use as well as unnecessary storage of setting not being utilized.  

