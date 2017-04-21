Description:
-------------------------------------------------
This application takes a point feature class / shapefile as an input
then creates a copy of the points in memory.

Those points are then used to create an xy events layer that
has the points offset at the user specified distance 
in a north easterly direction.

The idea of the application was to create a tool that would
allow stacked points to be symbolized and labeled independently.

Kirk Kuykendall's code from this post 
http://forums.esri.com/Thread.asp?c=93&f=993&t=210767&mc=12#msgid652601
is what is used for the creating in memory feature class.

The application has been tested with point data from shapefiles
and geodatabase feature classes.  

The tool is a command on a toolbar.

Please let me know of any problems with the application, if you are able.

Thanks! 


Requirements:
-------------------------------------------------
1). ArcMap 9.2:
	though it might be able to be used with earlier versions, 
	it just has not been tested using previous versions.

2). .NET Framework 2.0

3). The user will need to have write permissions to the directory
	that they want to use as an output dir.




Build Info:
-------------------------------------------------
Language: 		C# 
.NET Framework: 	2.0
ArcMap Version:		9.2
Service Pack:		sp3

Installer:		Visual Studio Setup Project
Uninstall included:	yes

	* note: if you recompile the the solution, be sure to 
	increment the installer "Version" property by 1 (ie: 1.0.1 --> 1.0.2)
	so that the "removePreviousVersions" property will work.




Contact Info:
-------------------------------------------------
Organization:   City Of Medford GIS
Location:       Medford, Or, United States
Programmer:     David Renz
Email:          djrenz@cityofmedford.org



Setup:
-------------------------------------------------
1. Extract the files to a directory.
2. That directory will contain a folder named "Move Points"
3. Within that folder are 2 folders
	a. Unstack Points Setup
		i. this is the setup project
	b. Unstack Points
		i. this is the application
4. Open the "Unstack Points Setup" directory
5. Open the "Release" directory
6. Click on "setup.exe"
7. Follow the instructions




Load the toolbar:
---------------------------------------------------
1. Go to View --> toolbars
2. Check "City of Medford: Unstack Points"





Notes:
-------------------------------------------------

1. Installer Launch Condition:
	The installer has a launch condition that looks for ArcMap.exe , 
	version 9.2 or higher. If this is a problem for you the launch 
	condition can be changed or even removed from the Visual Studio 
	setup project.
	
2. Source Control
	This solution was under source control. I tried to remove all the 
	bindings, but some seem to have slipped through. You should be able to 
	open the solution if you just click through the warnings.
	
3. On large datasets it can take some time to load the points into memory.	
	On my machine, 85,000 records took approximately 3-5 minutes to load.
	Please be patient, or use a clipped dataset if you are not getting the 
	performance you want.
	
	
Bug fixes: 
-------------------------------------------------
11_05_2007: Fixed problem with button state / form visibility after click. 
11_05_2007: Fixed installer issue. 
11_05_2007: Fixed error with null IGxObject when GxDialog is fired then canceled. 
11_05_2007: Add test to ensure output location exists. 
11_07_2007: Fix null point error from geocode export (handles points that are null) 
11_07_2007: Changed installer 
03_31_2008: Seperated offset in to XOffset and YOffset, so that they can be independently managed (thanks to Trevor in Tamarac, Florida for that idea)




