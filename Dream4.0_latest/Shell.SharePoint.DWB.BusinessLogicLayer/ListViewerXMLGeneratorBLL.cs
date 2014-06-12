#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: ListViewerXMLGeneratorBLL.cs
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Shell.SharePoint.DWB.Business.DataObjects;

namespace Shell.SharePoint.DWB.BusinessLogicLayer
{
    /// <summary>
    /// ListViewerXMLGeneratorBLL class.
    /// </summary>
       
    public class ListViewerXMLGeneratorBLL
    {
        #region DECLARATION
        XmlDocument objXmlDocument;
        #endregion
        #region XMLCREATION METHODS
        /// <summary>
        /// Creates the list viewer XML.
        /// </summary>
        /// <param name="records">The records.</param>
        /// <returns></returns>
        public XmlDocument CreateListViewerXML(Records records)
        {

            objXmlDocument = new XmlDocument();
            objXmlDocument = CreateRootElement(records);
            return objXmlDocument;
        }     

        /// <summary>
        /// Creates the root element.
        /// </summary>
        /// <param name="objRequestInfo">The request info object.</param>
        /// <returns></returns>
        private XmlDocument CreateRootElement(Records records)
        {

            //Creating the root Xml Element RequestInfo.
            XmlElement RecordsElement = objXmlDocument.CreateElement("records");
            objXmlDocument.AppendChild(RecordsElement);

            //Creating the RequestInfo node with Name attribute.
            XmlNode RecordsNode = objXmlDocument.DocumentElement;
            if (records.Record != null)
            {
                //Method call to create Entity Node.
                CreateRecord(records, RecordsNode);
            }
            else
                objXmlDocument = null;

            //returns the final SearchRequest Xml.
            return objXmlDocument;
        }

        /// <summary>
        /// Creates the entity.
        /// </summary>
        /// <param name="objRequestInfo">The requestinfo object.</param>
        /// <param name="EntitiesElement">The entities element.</param>
        private void CreateRecord(Records records, XmlNode recordsNode)
        {
            XmlElement RecordElement;

            foreach (Record objRecord in records.Record)
            {
                RecordElement = objXmlDocument.CreateElement("record");
                recordsNode.AppendChild(RecordElement);
                if (objRecord.Order.Length > 0)
                {
                    XmlAttribute OrderAttribute = objXmlDocument.CreateAttribute("order");
                    RecordElement.Attributes.Append(OrderAttribute);
                    OrderAttribute.Value = objRecord.Order;
                }
                if (objRecord.RecordNumber.Length > 0)
                {
                    XmlAttribute RecordNumberAttribute = objXmlDocument.CreateAttribute("recordNumber");
                    RecordElement.Attributes.Append(RecordNumberAttribute);
                    RecordNumberAttribute.Value = objRecord.RecordNumber;
                }
                if (objRecord.RecordInfo != null)
                {
                    CreateRecordInfo(objRecord.RecordInfo, RecordElement);
                }
                //if (objRecord.RecordHistories != null)
                //{
                //    CreateRecordHistories(objRecord.RecordHistories, RecordElement);
                //}
            }

        }

        /// <summary>
        /// Sets the Record Histor details.
        /// </summary>
        /// <param name="recordHistories">RecordHistories.</param>
        /// <param name="recordElement">XmlElement.</param>
        private void CreateRecordHistories(RecordHistories recordHistories, XmlElement recordElement)
        {
            XmlElement RecordHistoriesElement = objXmlDocument.CreateElement("recordHistories");
            recordElement.AppendChild(RecordHistoriesElement);
            foreach (RecordHistory objRecordHistory in recordHistories.History)
            {
                XmlElement HistoryElement = objXmlDocument.CreateElement("history");
                RecordHistoriesElement.AppendChild(HistoryElement);

                XmlAttribute HistoryOrder = objXmlDocument.CreateAttribute("order");
                HistoryElement.Attributes.Append(HistoryOrder);
                HistoryOrder.Value = objRecordHistory.Order;

                if (objRecordHistory.Attributes != null)
                {
                    foreach (Attributes objAttribute in objRecordHistory.Attributes)
                    {
                        XmlElement AttributeElement = objXmlDocument.CreateElement("attribute");
                        HistoryElement.AppendChild(AttributeElement);

                        //Creating attributes for Attribute Element.
                        if (objAttribute.Name.ToString().Length > 0)
                        {
                            XmlAttribute AttributeName = objXmlDocument.CreateAttribute("name");
                            AttributeElement.Attributes.Append(AttributeName);
                            AttributeName.Value = objAttribute.Name;
                        }

                        if (objAttribute.Value.ToString().Length > 0)
                        {
                            XmlAttribute AttributeValue = objXmlDocument.CreateAttribute("value");
                            AttributeElement.Attributes.Append(AttributeValue);
                            AttributeValue.Value = objAttribute.Value;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Creates the record info.
        /// </summary>
        /// <param name="recordInfo">The record info.</param>
        /// <param name="recordElement">The record element.</param>
        private void CreateRecordInfo(RecordInfo recordInfo, XmlElement recordElement)
        {
            XmlElement RecordInfoElement = objXmlDocument.CreateElement("recordInfo");
            recordElement.AppendChild(RecordInfoElement);
            foreach (Attributes objAttribute in recordInfo.Attributes)
            {
                XmlElement AttributeElement = objXmlDocument.CreateElement("attribute");
                RecordInfoElement.AppendChild(AttributeElement);

                //Creating attributes for Attribute Element.
                if (objAttribute.Name.ToString().Length > 0)
                {
                    XmlAttribute AttributeName = objXmlDocument.CreateAttribute("name");
                    AttributeElement.Attributes.Append(AttributeName);
                    AttributeName.Value = objAttribute.Name;
                }

                if (objAttribute.Value.ToString().Length > 0)
                {
                    XmlAttribute AttributeValue = objXmlDocument.CreateAttribute("value");
                    AttributeElement.Attributes.Append(AttributeValue);
                    AttributeValue.Value = objAttribute.Value;
                }

                if (objAttribute.Display.ToString().Length > 0)
                {
                    XmlAttribute AttributeValue = objXmlDocument.CreateAttribute("display");
                    AttributeElement.Attributes.Append(AttributeValue);
                    AttributeValue.Value = objAttribute.Display;
                }
                if (objAttribute.DataType.Length > 0)
                {
                    XmlAttribute AttributeValue = objXmlDocument.CreateAttribute("datatype");
                    AttributeElement.Attributes.Append(AttributeValue);
                    AttributeValue.Value = objAttribute.DataType;
                }
            }
        }
        #endregion        
    }
}
