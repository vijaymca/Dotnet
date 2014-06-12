#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//
//Filename: ResponseXMLInterceptor.cs
#endregion

using System;
using System.Data;
using System.Text;
using System.Xml;

using Shell.SharePoint.DREAM.Utilities;

namespace Shell.SharePoint.DREAM.Controller
{
    /// <summary>
    /// Class which intetcepts the webservice response xml and insert/modify the necessary nodes
    /// </summary>
    public class ResponseXMLInterceptor : Constants
    {
        #region Declaration
        private AbstractController objMossController;
        private ServiceProvider objFactory = new ServiceProvider();
        private DataTable dtTectonicSetting;
        private string strCamlQuery = "<Where><Eq><FieldRef Name='Active' /><Value Type='Choice'>Yes</Value></Eq></Where>";
        CommonUtility objCommonUtility = new CommonUtility();

        #region Constants
        const string TECTONICTYPE = "TectonicType";
        const string TECTONICTYPEKLEMME = "Klemme";
        const string TECTONICTYPEBALLY = "Bally";
        #endregion
        #endregion

        /// <summary>
        /// Intercepts the Field Header response xml from webservice
        /// Modify the "tectonic setting" values .
        /// </summary>
        /// <param name="responseXmlDoc">The response XML doc.</param>
        /// <param name="viewMode">The view mode.</param>
        /// <param name="siteURL">The site URL.</param>
        /// <returns></returns>
        public XmlDocument InsertTectonicSettings(XmlDocument responseXmlDoc, string viewMode)
        {
            string strBallyValue = string.Empty;
            string strKlemmeValue = string.Empty;
            string strFilterExpression = "BasinName = '{0}'"; ;
            string strSortExpression = "TectonicType ASC";
            string strBasinXPath = string.Empty;
            XmlNodeList xmlNodeListBasinName;
            DataRow[] dtRows;
            XmlNodeList xmlNodeListGeology;
            XmlNode xmlBallyNode;
            XmlNode xmlKlemmeNode;

            try
            {
                if (responseXmlDoc != null && responseXmlDoc.HasChildNodes)
                {
                    /// If DataSheet view
                    /// Select the "Geology" group attributes using XPath Query and update the value
                    /// Query the SharePoint list - "Tectonic Setting" for the given "Basin Name"
                    /// Get the "BasinName"          
                    /// Else If Tabular view
                    /// For each record find "Tectonic Setting Bally" and "Tectonic Setting Klemme"
                    /// And replace with proper setting value
                    GetTectonicSettingList(objCommonUtility.GetSiteURL());
                    if (string.Equals(viewMode.ToLowerInvariant(), TABULARVIEW))
                    {
                        /// Get all "Basin Name" attribute
                        strBasinXPath = string.Format(ATTRIBUTEXPATHWITHNAMEVALUE, NAME, BASINNAMTATTRIBUTE);
                        xmlNodeListBasinName = responseXmlDoc.SelectNodes(strBasinXPath);
                        foreach (XmlNode xmlNode in xmlNodeListBasinName)
                        {
                            strBallyValue = string.Empty;
                            strKlemmeValue = string.Empty;
                            if (dtTectonicSetting != null && dtTectonicSetting.Rows.Count > 0)
                            {
                                dtRows = dtTectonicSetting.Select(string.Format(strFilterExpression, xmlNode.Attributes[VALUE].Value), strSortExpression);
                                foreach (DataRow dtRow in dtRows)
                                {
                                    if (dtRow[TECTONICTYPE].ToString().Equals(TECTONICTYPEBALLY))
                                    {
                                        strBallyValue = Convert.ToString(dtRow[TITLE]);
                                    }
                                    if (dtRow[TECTONICTYPE].ToString().Equals(TECTONICTYPEKLEMME))
                                    {
                                        strKlemmeValue = Convert.ToString(dtRow[TITLE]);
                                    }
                                }
                            }

                            responseXmlDoc.SelectSingleNode("response/report/record[@recordno='" + xmlNode.ParentNode.Attributes["recordno"].Value + "']/attribute[@name='" + BALLYATTRIBUTEXPATHTABULAR + "']").Attributes[VALUE].Value = strBallyValue;

                            responseXmlDoc.SelectSingleNode("response/report/record[@recordno='" + xmlNode.ParentNode.Attributes["recordno"].Value + "']/attribute[@name='" + KLEMMEATTRIBUTEXPATHTABULAR + "']").Attributes[VALUE].Value = strKlemmeValue;

                        }

                    }
                    else if (string.Equals(viewMode.ToLowerInvariant(), DATASHEETVIEW))
                    {
                        xmlNodeListGeology = responseXmlDoc.SelectNodes("response/report/record/blocks/block[@name='Field Information']/group[@name='Geology']");
                        foreach (XmlNode xmlNode in xmlNodeListGeology)
                        {
                            strBallyValue = string.Empty;
                            strKlemmeValue = string.Empty;
                            if (dtTectonicSetting != null && dtTectonicSetting.Rows.Count > 0)
                            {
                                dtRows = dtTectonicSetting.Select(string.Format(strFilterExpression, xmlNode.FirstChild.Attributes[VALUE].Value), strSortExpression);
                                foreach (DataRow dtRow in dtRows)
                                {
                                    if (dtRow[TECTONICTYPE].ToString().Equals(TECTONICTYPEBALLY))
                                    {
                                        strBallyValue = Convert.ToString(dtRow[TITLE]);
                                    }
                                    if (dtRow[TECTONICTYPE].ToString().Equals(TECTONICTYPEKLEMME))
                                    {
                                        strKlemmeValue = Convert.ToString(dtRow[TITLE]);
                                    }
                                }
                            }
                            xmlBallyNode = xmlNode.SelectSingleNode("attribute[@name='" + BALLYATTRIBUTEXPATHTABULAR + "']");

                            if (xmlBallyNode != null)
                            {

                                responseXmlDoc.SelectSingleNode("response/report/record[@recordno='" + xmlNode.ParentNode.ParentNode.ParentNode.Attributes["recordno"].Value + "']/blocks/block[@name='Field Information']/group[@name='Geology']/attribute[@name='" + BALLYATTRIBUTEXPATHTABULAR + "']").Attributes[VALUE].Value = strBallyValue;

                            }
                            xmlKlemmeNode = xmlNode.SelectSingleNode("attribute[@name='" + KLEMMEATTRIBUTEXPATHTABULAR + "']");

                            if (xmlKlemmeNode != null)
                            {

                                responseXmlDoc.SelectSingleNode("response/report/record[@recordno='" + xmlNode.ParentNode.ParentNode.ParentNode.Attributes["recordno"].Value + "']/blocks/block[@name='Field Information']/group[@name='Geology']/attribute[@name='" + KLEMMEATTRIBUTEXPATHTABULAR + "']").Attributes[VALUE].Value = strKlemmeValue;

                            }
                        }
                    }

                }
            }
            finally
            {
                if (dtTectonicSetting != null)
                {
                    dtTectonicSetting.Dispose();
                }
            }
            return responseXmlDoc;
        }

        /// <summary>
        /// Gets the tectonic setting list.
        /// </summary>
        /// <param name="siteURL">The site URL.</param>
        /// <returns></returns>
        private DataTable GetTectonicSettingList(string siteURL)
        {
            objMossController = objFactory.GetServiceManager(MOSSSERVICE);

            dtTectonicSetting = ((MOSSServiceManager)objMossController).ReadList(siteURL, TECTONICSETTINGLIST, strCamlQuery);

            return dtTectonicSetting;
        }       
    }
}
