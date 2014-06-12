#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename      : UIUtilities.cs
#endregion

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Xml.Xsl;
using Shell.SharePoint.DREAM.Utilities;

namespace Shell.SharePoint.DREAM.Site.UI
{
    /// <summary>
    /// This class is used for commonly used functionalities in User Interface(ASPX apages).
    /// </summary>
    public class UIUtilities : UIHelper
    {
        #region Declaration
        const string REPORTXPATH = "response/report";
        const string RECORDXPATH = "/response/report/record";
        #endregion
        #region UI Utility Methods
        /// <summary>
        /// Assigns the values to drop down.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="document">The XML document.</param>
        /// <param name="listControl">The list control for which data needs to be populated.</param>
        public void AssignValuesToDropDown(XmlDocument document, ListBox listControl)
        {
            XmlNodeList xmlMainNodeList = null;
            try
            {
                //Check all the required objects are NULL
                if (listControl != null && document != null)
                {
                    xmlMainNodeList = document.SelectNodes(REPORTXPATH);
                    if (xmlMainNodeList != null)
                    {
                        foreach (XmlNode xmlMainNode in xmlMainNodeList)
                        {
                            //Check Current Field Exists
                            XmlNodeList xmlSubNodeList = document.SelectNodes(RECORDXPATH);

                            if (xmlSubNodeList != null)
                            {
                                //If sub node has value then added it to DropDown Box
                                foreach (XmlNode xmlSubNode in xmlSubNodeList)
                                {
                                    if (!string.IsNullOrEmpty(xmlSubNode.FirstChild.Attributes.GetNamedItem(VALUE).InnerText))
                                    {
                                        listControl.Items.Add(xmlSubNode.FirstChild.Attributes.GetNamedItem(VALUE).InnerText);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("XML Nodes not Found");
                    }
                }
                else
                {
                    throw new Exception("Invalid Arguments");
                }
            }
            catch 
            {
                throw;
            }
        }

        /// <summary>
        /// Assigns the values to drop down.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="document">The XML document.</param>
        /// <param name="listControl">The list control for which data needs to be populated.</param>
        public void AssignFieldValuesToDropDown(XmlDocument document, ListBox listControl)
        {
            XmlNodeList xmlMainNodeList = null;
            try
            {
                //Check all the required objects are NULL
                if (listControl != null && document != null)
                {
                    xmlMainNodeList = document.SelectNodes(REPORTXPATH);
                    if (xmlMainNodeList != null)
                    {
                        foreach (XmlNode xmlMainNode in xmlMainNodeList)
                        {
                            //Check Current Field Exists
                            XmlNodeList xmlSubNodeList = document.SelectNodes(RECORDXPATH);

                            if (xmlSubNodeList != null)
                            {
                                ListItem lstItem = null;
                                //If sub node has value then added it to DropDown Box
                                foreach (XmlNode xmlSubNode in xmlSubNodeList)
                                {
                                    if (!string.IsNullOrEmpty(xmlSubNode.FirstChild.Attributes.GetNamedItem(VALUE).InnerText))
                                    {
                                        lstItem = new ListItem();
                                        lstItem.Value = xmlSubNode.ChildNodes[0].Attributes.GetNamedItem(VALUE).InnerText;
                                        lstItem.Text = xmlSubNode.ChildNodes[1].Attributes.GetNamedItem(VALUE).InnerText;
                                        listControl.Items.Add(lstItem);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("XML Nodes not Found");
                    }
                }
                else
                {
                    throw new Exception("Invalid Arguments");
                }
            }
            catch
            {
                throw;
            }
        }        
#endregion
    }
}


