// 15/02/2021   Muhammet Ali Hancer- IBM - WDG (IBM RPA Offering) Integration Library
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// This is an example of a .NET action for IBM Datacap using .NET 4.0.
// The compliled DLL needs to be placed into the RRS directory.
// The DLL does not need to be registered.  
// Datacap studio will find the RRX file that is embedded in the DLL, you do not need to place the RRX in the RRS directory.
// If you add references to other DLLs, such as 3rd party, you may need to place those DLLs in C:\RRS so they are found at runtime.
// If Datacap references are not found at compile time, add a reference path of C:\Datacap\DCShared\NET to the project to locate the DLLs while building.
// This template has been tested with IBM Datacap 9.0.  
// Note: This is custom library and can be used ONLY for testing purposes. This library is not part of the standard Datacap installation! 

using System;
using System.Net;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Text;

namespace WDGIntegration
{
    public class WDGScripts // This class must be a base class for .NET 4.0 Actions.
    {
        #region ExpectedByRRS
        /// <summary/>
        ~WDGScripts()
        {
            DatacapRRCleanupTime = true;
        }

        /// <summary>
        /// Cleanup: This property is set right before the object is released by RRS
        /// The Dispose method is not called by RRS.
        /// </summary>
        public bool DatacapRRCleanupTime
        {
            set
            {
                if (value)
                {
                    CleanUp();
                    CurrentDCO = null;
                    DCO = null;
                    RRLog = null;
                    RRState = null;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
        }

        protected PILOTCTRLLib.IBPilotCtrl BatchPilot = null;
        public PILOTCTRLLib.IBPilotCtrl DatacapRRBatchPilot { set { this.BatchPilot = value; GC.Collect(); GC.WaitForPendingFinalizers(); } get { return this.BatchPilot; } }

        protected TDCOLib.IDCO DCO = null;
        /// <summary/>
        public TDCOLib.IDCO DatacapRRDCO
        {
            get { return this.DCO; }
            set
            {
                DCO = value;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        protected dcrroLib.IRRState RRState = null;
        /// <summary/>
        public dcrroLib.IRRState DatacapRRState
        {
            get { return this.RRState; }
            set
            {
                RRState = value;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        public TDCOLib.IDCO CurrentDCO = null;
        /// <summary/>
        public TDCOLib.IDCO DatacapRRCurrentDCO
        {
            get { return this.CurrentDCO; }
            set
            {
                CurrentDCO = value;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        public dclogXLib.IDCLog RRLog = null;
        /// <summary/>
        public dclogXLib.IDCLog DatacapRRLog
        {
            get { return this.RRLog; }
            set
            {
                RRLog = value;
                LogAssemblyVersion();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        #endregion

        #region CommonActions

        void OutputToLog(int nLevel, string strMessage)
        {
            if (null == RRLog)
                return;
            RRLog.WriteEx(nLevel, strMessage);
        }

        public void WriteLog(string sMessage)
        {
            OutputToLog(5, sMessage);
        }

        private bool versionWasLogged = false;

        // Log the version of the library that was running to help with diagnosis.
        // Hooked this method to be called after the log object is assigned.  Also put in
        // a check that this action runs only once, just in case it gets called multiple times.
        protected bool LogAssemblyVersion()
        {
            try
            {
                if (versionWasLogged == false)
                {
                    FileVersionInfo fv = System.Diagnostics.FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
                    WriteLog(Assembly.GetExecutingAssembly().Location +
                             ". AssemblyVersion: " + Assembly.GetExecutingAssembly().GetName().Version.ToString() +
                             ". AssemblyFileVersion: " + fv.FileVersion.ToString() + ".");
                    versionWasLogged = true;
                }
            }
            catch (Exception ex)
            {
                WriteLog("Version logging exception: " + ex.Message);
            }

            // We can always return true.  If getting the version fails, we can try to continue anyway.
            return true;
        }

        #endregion


        // implementation of the Dispose method to release managed resources
        // There is no guarentee that dispose will be called.  Also note, class distructors are also not called.  CleanupTime is called by RRS.        
        public void Dispose()
        {
            CleanUp();
        }

        /// <summary>
        /// Everthing that should be cleaned up on exit
        /// It is recommended to avoid logging during cleanup.
        /// </summary>
        protected void CleanUp()
        {
            try
            {
                // Cleanup and release any allocated objects here. This will be called before the DLL is released.
            }
            catch { } // Ignore any errors.
        }

        struct Level
        {
            internal const int Batch = 0;
            internal const int Document = 1;
            internal const int Page = 2;
            internal const int Field = 3;
        }

        struct Status
        {
            internal const int Hidden = -1;
            internal const int OK = 0;
            internal const int Fail = 1;
            internal const int Over = 3;
            internal const int RescanPage = 70;
            internal const int VerificationFailed = 71;
            internal const int PageOnHold = 72;
            internal const int PageOverridden = 73;
            internal const int NoData = 74;
            internal const int DeletedPage = 75;
            internal const int ExportComplete = 76;
            internal const int DeleteApproved = 77;
            internal const int ReviewPage = 79;
            internal const int DeletedDoc = 128;
        }

        /// <summary/>
        /// This is an example custom .NET action that takes multiple parameters with multiple types.
        /// The parameter order and types must match the definition in the RRX file.

        public bool runWDGBot_GET(string hostURL, string relativePath, string scriptName, bool unLockMachine, string WDGQuerystring)
        {
            bool bRes = true;
            dcSmart.SmartNav localSmartObj = null;
            try
            {
                localSmartObj = new dcSmart.SmartNav(this);

                string smarthostURL = localSmartObj.MetaWord(hostURL);
                string smartrelativePath = localSmartObj.MetaWord(relativePath);
                string smartscriptName = localSmartObj.MetaWord(scriptName);
                string smartWDGQuerystring = localSmartObj.MetaWord(WDGQuerystring);
                WriteLog("Method is=GET");
                string WDGURL = smarthostURL + smartrelativePath + smartscriptName + "?unlockMachine="+unLockMachine.ToString().ToLower()+"&" + smartWDGQuerystring;
                WriteLog("RestURL with full path is: " + WDGURL);

                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                WebRequest request = HttpWebRequest.Create(WDGURL);
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string urlText = reader.ReadToEnd(); 
                WriteLog(urlText);
                
            }
            catch (Exception ex)
            {
                WriteLog("There was an exception: " + ex.Message);
            }
            localSmartObj = null;
            return bRes;
        }


        

        public bool runWDGBot_POST(string hostURL, string relativePath, string scriptName, bool unLockMachine, string WDGinputParameters)
        {
            bool bRes = true;
            dcSmart.SmartNav localSmartObj = null;
            try
            {
                localSmartObj = new dcSmart.SmartNav(this);

                string smarthostURL = localSmartObj.MetaWord(hostURL);
                string smartrelativePath = localSmartObj.MetaWord(relativePath);
                string smartscriptName = localSmartObj.MetaWord(scriptName);
                string smartWDGQuerystring = localSmartObj.MetaWord(WDGinputParameters);
                WriteLog("Method is=GET");
                string WDGURL = smarthostURL + smartrelativePath + smartscriptName + "?unlockMachine=" + unLockMachine.ToString().ToLower();
                WriteLog("RestURL with full path is: " + WDGURL);
                string JSONBodyString = "{ ";
                string[] listStrElements = smartWDGQuerystring.Split('|');
                foreach (string KVP in listStrElements)
                {
                    string[] KVParr = KVP.Split('=');
                    JSONBodyString = JSONBodyString + "\"" + KVParr[0] + "\"" + ":" + "\"" + KVParr[1] + "\"" + ",";
                }
                JSONBodyString = JSONBodyString + "}";
                WriteLog("JsonBodyString is:");
                WriteLog(JSONBodyString);

                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                WebRequest request = WebRequest.Create(WDGURL);
                request.Method = "POST";
                string postData = JSONBodyString;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                WriteLog("Response status is: "+((HttpWebResponse)response).StatusDescription);
                using (dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();
                    WriteLog(responseFromServer);
                }
                response.Close();                
            }
            catch (Exception ex)
            {
                WriteLog("There was an exception: " + ex.Message);
            }
            localSmartObj = null;
            return bRes;
        }        
    }
}
