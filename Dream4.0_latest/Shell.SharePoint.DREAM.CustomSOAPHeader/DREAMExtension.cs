#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
///Filename: DREAMExtension.cs
#endregion
// <summary>
/// This class will be used to insert the SOAPHeader(for security)during the 
/// Report Service Call.
/// </summary>
using System;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.IO;
using System.Xml;
using Shell.SharePoint.DREAM.Utilities;    

namespace Shell.SharePoint.DREAM.CustomSOAPHeader
{

    /// <summary>
    /// This library is used for sending the custom SOAP header to web services
    /// whenever there is a web service call made.
    /// </summary>
    public class DREAMExtension : SoapExtension
    {
        #region Declaration
        public bool blnOutgoing = true;
        public bool blnIncoming;
        private Stream objOutputStream;
        public Stream objOldStream;
        public Stream objNewStream;
        #endregion
        #region Overridden Methods
        /// <summary>
        /// When overridden in a derived class, allows a SOAP extension to initialize data specific to an XML Web service method using an attribute applied to the XML Web service method at a one time performance cost.
        /// </summary>
        /// <param name="methodInfo">A <see cref="T:System.Web.Services.Protocols.LogicalMethodInfo"></see> representing the specific function prototype for the XML Web service method to which the SOAP extension is applied.</param>
        /// <param name="attribute">The <see cref="T:System.Web.Services.Protocols.SoapExtensionAttribute"></see> applied to the XML Web service method.</param>
        /// <returns>
        /// The <see cref="T:System.Object"></see> that the SOAP extension initializes for caching.
        /// </returns>
        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            return 0;
        }

        // The SOAP extension was configured to run using a configuration file
        // instead of an attribute applied to a specific XML Web service
        // method.
        /// <summary>
        /// Gets the initializer.
        /// </summary>
        /// <param name="WebServiceType">Type of the web service.</param>
        /// <returns></returns>
        public override object GetInitializer(Type WebServiceType)
        {
            return 0;
        }
        /// <summary>
        /// When overridden in a derived class, allows a SOAP extension to initialize itself using the data cached in the <see cref="M:System.Web.Services.Protocols.SoapExtension.GetInitializer(System.Web.Services.Protocols.LogicalMethodInfo,System.Web.Services.Protocols.SoapExtensionAttribute)"></see> method.
        /// </summary>
        /// <param name="initializer">The <see cref="T:System.Object"></see> returned from <see cref="M:System.Web.Services.Protocols.SoapExtension.GetInitializer(System.Web.Services.Protocols.LogicalMethodInfo,System.Web.Services.Protocols.SoapExtensionAttribute)"></see> cached by ASP.NET.</param>
        public override void Initialize(object initializer)
        {
            return;
        }

        /// <summary>
        /// When overridden in a derived class, allows a SOAP extension access to the memory buffer containing the SOAP request or response.
        /// </summary>
        /// <param name="stream">A memory buffer containing the SOAP request or response.</param>
        /// <returns>
        /// A <see cref="T:System.IO.Stream"></see> representing a new memory buffer that this SOAP extension can modify.
        /// </returns>
        public override Stream ChainStream(Stream stream)
        {
            // save a copy of the stream, create a new one for manipulating.
            this.objOutputStream = stream;
            objOldStream = stream;
            objNewStream = new MemoryStream();
            return objNewStream;
        }
        /// <summary>
        /// When overridden in a derived class, allows a SOAP extension to receive a <see cref="T:System.Web.Services.Protocols.SoapMessage"></see> to process at each <see cref="T:System.Web.Services.Protocols.SoapMessageStage"></see>.
        /// </summary>
        /// <param name="message">The <see cref="T:System.Web.Services.Protocols.SoapMessage"></see> to process.</param>
        public override void ProcessMessage(SoapMessage message)
        {
            StreamReader objReadStr;
            StreamWriter objWriteStr;
            string strSoapMsg1;
            string strUserName = PortalConfiguration.GetInstance().GetKey("ReportServiceUserName");
            XmlDocument xmlDoc = new XmlDocument();
            // a SOAP message has 4 stages. We're interested in .AfterSerialize
            switch (message.Stage)
            {
                case SoapMessageStage.BeforeSerialize:
                    break;
                case SoapMessageStage.AfterSerialize:
                    {
                        // Get the SOAP body as a string, so we can manipulate...
                        String strSoapBodyString = getXMLFromCache();
                        
                        int intPos1 = strSoapBodyString.IndexOf("Body") - 6;
                        int intPos2 = strSoapBodyString.Length - intPos1;
                        strSoapBodyString = strSoapBodyString.Substring(intPos1, intPos2);

                        // Create the SOAP Message 
                        // It's comprised of a <soap:Element> that's enclosed in <soap:Body>. 
                        // Pack the XML document inside the <soap:Body> element 
                        String strXmlVersionString = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                        String strSoapEnvelopeBeginString =
                          "<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:soapenc=\"http://schemas.xmlsoap.org/soap/encoding/\"  xmlns:tns=\"http://tempuri.org/\"    xmlns:types=\"http://tempuri.org/encodedTypes\"    xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"  xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">";
                        String strSoapEnvHeaderString1 =
                            "<soap:Header><Username>";
                        String strSoapEnvHeaderString2 = "</Username>";
                        String strSoapEnvHeaderString3 =
                           "</soap:Header>";
                        Stream objAppOutputStream = new MemoryStream();
                        StreamWriter objSoapMessageWriter = new StreamWriter(objAppOutputStream);

                        //Writer XML Version
                        objSoapMessageWriter.Write(strXmlVersionString);

                        //Writer Envolpe
                        objSoapMessageWriter.Write(strSoapEnvelopeBeginString);

                        // The heavy-handed part - forcing the right headers AND the uname/pw :)
                        objSoapMessageWriter.Write(strSoapEnvHeaderString1);
                        objSoapMessageWriter.Write(strUserName);
                        objSoapMessageWriter.Write(strSoapEnvHeaderString2);                        
                        objSoapMessageWriter.Write(strSoapEnvHeaderString3);
                        // End clubbing of baby seals
                        // Add the strSoapBodyString back in - it's got all the closing 
                        // XML we need.
                        objSoapMessageWriter.Write(strSoapBodyString);
                        // write it all out.

                        objSoapMessageWriter.Flush();
                        objAppOutputStream.Flush();
                        objAppOutputStream.Position = 0;
                        StreamReader objReader = new StreamReader(objAppOutputStream);
                        StreamWriter objWriter = new StreamWriter(this.objOutputStream);
                        objWriter.Write(objReader.ReadToEnd());
                        objWriter.Flush();
                        objAppOutputStream.Close();
                        this.blnOutgoing = false;
                        this.blnIncoming = true;
                        break;
                    }
                case SoapMessageStage.BeforeDeserialize:
                    {
                        // Make the output available for the client to parse...
                        objReadStr = new StreamReader(objOldStream);
                        objWriteStr = new StreamWriter(objNewStream);
                        strSoapMsg1 = objReadStr.ReadToEnd();
                        xmlDoc.LoadXml(strSoapMsg1);
                        strSoapMsg1 = xmlDoc.InnerXml;
                        objWriteStr.Write(strSoapMsg1);
                        objWriteStr.Flush();
                        objNewStream.Position = 0;
                        break;
                    }
                case SoapMessageStage.AfterDeserialize:
                    break;
                default:
                    throw new Exception("invalid stage!");
            }
        }
        #endregion
        #region Private Methods
        /// <summary>
        /// Gets the XML from cache.
        /// </summary>
        /// <returns></returns>
        public string getXMLFromCache()
        {
            objNewStream.Position = 0; // start at the beginning!
            string strSOAPresponse = ExtractFromStream(objNewStream);
            return strSOAPresponse;
        }

        /// <summary>
        /// Extracts from stream.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        private String ExtractFromStream(Stream target)
        {
            if (target != null)
                return (new StreamReader(target)).ReadToEnd();
            return string.Empty;
        }
        #endregion
    }
    /// <summary>
    /// DreamExtensionAttribute class
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class DreamExtensionAttribute : SoapExtensionAttribute
    {
        private string strName;

        // You can use this property to enable the setting of the priority of the attribute
        // If needed, create a private variable to hold the value, and assign appropiately
        /// <summary>
        /// When overridden in a derived class, gets or set the priority of the SOAP extension.
        /// </summary>
        /// <value></value>
        /// <returns>The priority of the SOAP extension.</returns>
        public override int Priority
        {
            get { return 1; }
            set { }
        }

        // This is the most important property. It tells the runtime which class to 
        // instantiate and execute.
        /// <summary>
        /// When overridden in a derived class, gets the <see cref="T:System.Type"></see> of the SOAP extension.
        /// </summary>
        /// <value></value>
        /// <returns>The <see cref="T:System.Type"></see> of the SOAP extension.</returns>
        public override Type ExtensionType
        {
            get { return typeof(DREAMExtension); }
        }

        // this can hold the name of the extension, not needed.
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return strName; }
            set { strName = value; }
        }
    }
}
