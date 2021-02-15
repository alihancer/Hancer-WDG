The dll file in this folder should be copied under the RRS folder of Datacap Installation Folder which generally C:/Datacap. 

Here is an action example.The below example calls the bot named "HancerRPA" and sending the export.txt filename which is produced dynamically under export folder of the Datacap app.

runWDGBot_POST("@STRING(https://ibmbaw:8099/)","@STRING(scripts/)","HancerRPA","False","InputText=+@STRING(C:\\Datacap\\PolicyApp\\Export\\)+@B.ID+@STRING(.txt)")
In Datacap Studio: the belows are examples of how each parameter can be set to create the above action<br/>
string hostURL=@STRING(https://ibmbaw:8099/)<br/>
string relativePath= @STRING(scripts/)<br/>
string wdgBotName=HancerRPA<br/>
boolean unLockMachine=False<br/>
string WDGinputParameters=InputText=+@STRING(C:\\Datacap\\PolicyAppDev\\Export\\)+@B.ID+@STRING(.txt)<br/>

Screenshot:
![alt text](./DStudioActionScreenhotCapture.PNG?raw=true)
