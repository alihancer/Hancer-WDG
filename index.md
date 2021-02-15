<h1>WDG Action Library for Datacap Applications</h1>

<h2>Introduction Welcome to Datacap WDG integration on GitHub Pages</h2>
With this integration, my purpose was to demonstrate how Datacap can call WDG REST API in a datacap application. 

### UseCase Description
Lets assume Datacap classified and extracted all the required data from the pages processed, and in the export step, the export csv file will be produced and an RPA Bot will be triggered by Datacap with an input of the exported txt file.

###Deployment of Action Library to Datacap
Copy the dll file provided under the deployment folder to RRS folder under the Datacap installation which is generally C:/Datacap

###Configure/use of action
1. Open your Datacap application with Datacap Studio
2. Create a ruleset/Rule/Function in your ruleset list
3. Assign your rule to the related DCO object which in our sample, it can be batch level.
3. Add the provided action to your function with the related RPA parameters.

I provided a sample screenshot how the parameters should look like but again this varies based on your input variable name in the WDG bot and also your bot name and server settings. 

PS:Dont forget to put '/' at the end of the server name and the scripts text as shown in screenshot. 
<br/>
![Sample Screenshot](https://github.com/alihancer/Hancer-WDG/blob/main/WDGIntegration%20Action%20Library/Deployment/DStudio%20ActionScreenhotCapture.PNG)

Enjoy the integration and feel free to reach out to me for any suggestions and comments.

Muhammet Ali Hancer
