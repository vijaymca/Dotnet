#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: ChartHelper.cs
#endregion

using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.Xml.Xsl;

using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;

namespace Shell.SharePoint.WebParts.DREAM.Charts
{
    /// <summary>
    /// Helper Class for Chart webparts.
    /// Contains common methods for Charting
    /// </summary>
    public class ChartHelper : WebPart
    {
        #region Declarations
        #region Constants
        protected const string TABULARRESPONSETYPE = "Tabular";
        protected const string EQUALSOPERATOR = "EQUALS";
        protected const string LIKEOPERATOR = "LIKE";
        protected const string INOPERATOR = "IN";
        protected const string ANDOPERATOR = "AND";
        protected const string STAROPERATOR = "*";
        protected const string AMPERSAND = "%";
        protected const string REPORTSERVICE = "ReportService";
        protected const string MOSSSERVICE = "MossService";
        protected const string ATTRIBUTETABLENAME = "attribute";
        protected const string DEPTHCOLUMN = "Depth";
        protected const string UNITCOLUMN = "Unit";
        protected const string UWBISEARCHCRITERIA = "UWBI";
        protected const string METERFULLSTRING = "metres";
        protected const string FEETFULLSTRING = "feet";
        protected const string NORECORDSFOUND = "No records were found that matched your search parameters. Please modify your paramaters and run the search again.";
        protected const string TECHNICALEXCEPTION = "Unable to plot the chart. Please contact the administrator.";
        #endregion

        #region Variables
        protected RequestInfo objRequestInfo;
        protected XmlDocument objRespXmlDocument ;
        protected AbstractController objMOSSController;
        protected ServiceProvider objFactory = new ServiceProvider();
        protected AbstractController objReportController ;
        protected CommonUtility objCommonUtility = new CommonUtility();
        protected UserPreferences objPreferences ;
        string strSearchType = string.Empty;
        string strRequestID = string.Empty;
        string strCriteriaName = string.Empty;
        string strResponseType = string.Empty;
        string strCriteriaValue = string.Empty;
        string strEntityName = string.Empty;
        protected string strCurrSiteUrl = string.Empty;
        #endregion

        #endregion

        #region Protected Properties
        protected string SearchType
        {
            get { return strSearchType; }
            set { strSearchType = value; }
        }

        protected string RequestID
        {
            get { return strRequestID; }
            set { strRequestID = value; }
        }

        /// <summary>
        /// Gets or sets the name of the entity.
        /// </summary>
        /// <value>The name of the entity.</value>
        protected string EntityName
        {
            get { return strEntityName; }
            set { strEntityName = value; }
        }

        /// <summary>
        /// Gets or sets the CRITERIA.
        /// </summary>
        /// <value>The CRITERIA.</value>
        string Criteria
        {
            get { return strCriteriaName; }
            set { strCriteriaName = value; }
        }
        /// <summary>
        /// Gets or sets the CRITERIAVALUE.
        /// </summary>
        /// <value>The CRITERIAVALUE.</value>
        string CriteriaValue
        {
            get { return strCriteriaValue; }
            set { strCriteriaValue = value; }
        }

        /// <summary>
        /// Gets or sets the type of the response.
        /// </summary>
        /// <value>The type of the response.</value>
        protected string ResponseType
        {
            get { return strResponseType; }
            set { strResponseType = value; }
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets the response XML.
        /// </summary>
        /// <param name="criteriaName">Name of the criteria.</param>
        /// <param name="selectedCriteriaValues">The selected criteria values.</param>
        /// <returns></returns>
        protected XmlDocument GetResponseXML(string criteriaName, string selectedCriteriaValues)
        {
            objRequestInfo = GetRequestInfo(criteriaName, selectedCriteriaValues);
            objFactory = new ServiceProvider();
            objReportController = objFactory.GetServiceManager(REPORTSERVICE);
            objRespXmlDocument = objReportController.GetSearchResults(objRequestInfo, -1, SearchType, null, 0);
            //objRespXmlDocument = new XmlDocument();
            //objRespXmlDocument.Load(@"C:\yasotha\RadChart\Shell.SharePoint.DREAM.WebParts.ChartPOC\dir survey detail resp-SIEP.xml");
            return objRespXmlDocument;

        }

        /// <summary>
        /// Gets the RAD data source.
        /// </summary>
        /// <param name="objXmlTextReader">The obj XML text reader.</param>
        /// <returns></returns>
        protected DataSet GetRadDataSource( XmlTextReader objXmlTextReader)
        {
            XmlDocument xmlDocForXSL = new XmlDocument();
            StringWriter objStringWriter = new StringWriter();
            XslCompiledTransform objCompiledTransform = new XslCompiledTransform();
            XsltArgumentList xsltArgsList = new XsltArgumentList();
            DataSet dsOutput = new DataSet();
            switch (EntityName.ToLowerInvariant())
            {
                case "directionalsurveydetail":
                    {
                        xsltArgsList.AddParam("PARAM1", string.Empty, DEPTHCOLUMN);
                        xsltArgsList.AddParam("PARAM2", string.Empty, "Inclination");
                        xsltArgsList.AddParam("PARAM3", string.Empty, "Azimuth");
                        xsltArgsList.AddParam("PARAM4", string.Empty, UNITCOLUMN);
                        xsltArgsList.AddParam("PARAM5", string.Empty, UWBISEARCHCRITERIA);
                        break;
                    }
                case "picksdetail":
                    {
                        xsltArgsList.AddParam("PARAM1", string.Empty, "Pick Name");
                        xsltArgsList.AddParam("PARAM2", string.Empty, "AHBDF AH");
                        xsltArgsList.AddParam("PARAM4", string.Empty, UNITCOLUMN);
                        xsltArgsList.AddParam("PARAM5", string.Empty, UWBISEARCHCRITERIA);

                        break;
                    }

            }
            xmlDocForXSL.Load(objXmlTextReader);
            objCompiledTransform.Load(xmlDocForXSL);

            objCompiledTransform.Transform(new XmlNodeReader(objRespXmlDocument), xsltArgsList, objStringWriter);
            objRespXmlDocument.LoadXml(objStringWriter.ToString());

            objStringWriter.Flush();
            objStringWriter.Close();
            objStringWriter.Dispose();

            dsOutput.ReadXml(new XmlNodeReader(objRespXmlDocument), XmlReadMode.InferTypedSchema);

            return dsOutput;
        }


        /// <summary>
        /// Converts the unit value.
        /// </summary>
        /// <param name="preferedUnit">The prefered unit.</param>
        /// <param name="dsChart">The ds chart.</param>
        /// <returns></returns>
        protected DataSet ConvertUnitValue(string preferedUnit, DataSet chartData, string depthColumnName)
        {
            double formulaValue = 3.28084;

            if (chartData != null && chartData.Tables[ATTRIBUTETABLENAME] != null && chartData.Tables[ATTRIBUTETABLENAME].Rows.Count > 0)
                {
                    int intRowIndex = 0;
                    double dblDepth = 0;
                    string strDepthUnit = string.Empty;
                    foreach (DataRow drChart in chartData.Tables[ATTRIBUTETABLENAME].Rows)
                    {
                        strDepthUnit = string.Empty;
                        /// Read the depth unit for the selected column
                        strDepthUnit = drChart[UNITCOLUMN].ToString().ToLowerInvariant();
                        /// Read the Depth value
                        if (drChart[depthColumnName] != null || drChart[depthColumnName] != DBNull.Value)
                        {
                            if (!string.IsNullOrEmpty(drChart[depthColumnName].ToString()))
                            {
                                dblDepth = Convert.ToDouble(drChart[depthColumnName].ToString());
                            }
                        }
                        /// If the column value is in meter and user preference is in feet, convert m to feet
                        /// Feet -> Meter = value in feet * 3.28084
                        if ((strDepthUnit.Equals("m") || strDepthUnit.Equals(METERFULLSTRING) || strDepthUnit.ToLowerInvariant().Equals("metres") || strDepthUnit.Equals("meters")) && preferedUnit.Equals(FEETFULLSTRING))
                        {
                            chartData.Tables[ATTRIBUTETABLENAME].Rows[intRowIndex][depthColumnName] = dblDepth * formulaValue;
                        } /// If the column value is in feet and user preference is in m, convert feet to m
                         /// Meter -> Feet = value in meter / 3.28084
                        else if ((strDepthUnit.Equals("ft") || strDepthUnit.ToLowerInvariant().Equals(FEETFULLSTRING)) && preferedUnit.ToLowerInvariant().Equals(METERFULLSTRING))
                        {
                            chartData.Tables[ATTRIBUTETABLENAME].Rows[intRowIndex][depthColumnName] = dblDepth / formulaValue;
                        }
                        intRowIndex++;
                    }
                }
            
            return chartData;

        }


        #endregion

        #region Private Methods


        #region DataObjectCreation
        /// <summary>
        /// Sets the basic data objects to create XML document
        /// </summary>
        /// <param name="criteriaName">Name of the criteria.</param>
        /// <param name="selectedCriteriaValues">The selected criteria values.</param>
        /// <returns></returns>
        protected RequestInfo GetRequestInfo(string criteriaName, string selectedCriteriaValues)
        {
            objRequestInfo = new RequestInfo();

            /// Set the dataobject values.              
            Criteria = criteriaName;
            if (!string.IsNullOrEmpty(selectedCriteriaValues))
            {
                CriteriaValue = selectedCriteriaValues;
            }
            objRequestInfo.Entity = SetEntity();

            return objRequestInfo;
        }
       
        /// <summary>
        /// This function will set the Entity object.
        /// </summary>
        /// <returns></returns>
        private Entity SetEntity()
        {

            Entity objEntity = new Entity();
            objEntity.Property = true;
            objEntity.ResponseType = ResponseType;
            objEntity.Name = EntityName;
            ArrayList arlAttribute = new ArrayList();
            ArrayList arlAttributeGroup = new ArrayList();
            /// The below condition set the attribute group objects based on search type.
            arlAttribute = SetAtribute();
            objEntity.Attribute = arlAttribute;

            return objEntity;

        }
       
        /// <summary>
        /// Sets the atribute.
        /// </summary>
        /// <returns></returns>
        private ArrayList SetAtribute()
        {
            ArrayList arlAttribute = new ArrayList();
            ArrayList arlValue = new ArrayList();
            Attributes objDetail = new Attributes();
            objDetail.Name = Criteria;

            if (!string.IsNullOrEmpty(CriteriaValue))
                arlValue.Add(SetValue(CriteriaValue.Trim()));
            objDetail.Value = arlValue;
            objDetail.Operator = GetOperator(objDetail.Value);
            arlAttribute.Add(objDetail);
            return arlAttribute;
        }

        /// <summary>
        /// Sets the string field as a Value object.
        /// </summary>
        /// <param name="strField">field.</param>
        /// <returns></returns>
        private Value SetValue(string field)
        {
            Value objValue = new Value();
            objValue.InnerText = field;
            /// returns the value object.
            return objValue;
        }
      
        /// <summary>
        /// Gets the query operator for request xml
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        protected string GetOperator(ArrayList value)
        {
            string strOperator = string.Empty;
            /// the below condition validates the value parameter.
            if (value.Count > 1)
            {
                strOperator = INOPERATOR;
            }
            else
            {
                /// Loop through the values in ArrayList.
                foreach (Value objValue in value)
                {
                    /// the below condition check for the innerText value.
                    if ((objValue.InnerText.Contains(STAROPERATOR)) || (objValue.InnerText.Contains(AMPERSAND)))
                        strOperator = LIKEOPERATOR;
                    else
                        strOperator = EQUALSOPERATOR;
                }
            }


            return strOperator;
        }
       
        /// <summary>
        /// Gets the operator.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        protected string GetOperator(string value)
        {
            string strOperator;
            /// The below condition check for the innerText value.
            if ((value.Contains(STAROPERATOR)) || (value.Contains(AMPERSAND)))
            {
                strOperator = LIKEOPERATOR;
            }
            else
            {
                strOperator = EQUALSOPERATOR;
            }
            return strOperator;
        }

        #endregion
        #endregion
    }
}
