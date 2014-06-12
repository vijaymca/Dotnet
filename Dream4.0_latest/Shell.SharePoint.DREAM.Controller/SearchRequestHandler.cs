#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//
//Filename: SearchRequestHandler.cs
#endregion

/// <summary> 
/// This class is used to create request xml request for a search.
/// </summary> 
using System;
using System.Xml;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Utilities;

namespace Shell.SharePoint.DREAM.Controller
{
    /// <summary>
    /// RequestHandler class to generate the Request XML.
    /// </summary>
    public class SearchRequestHandler :Constants
    {
        #region DECLARATION
        XmlDocument objXmlDocument;
        CommonUtility objCommonUtility = new CommonUtility();
        #endregion
        #region XMLCREATION METHODS
        /// <summary>
        /// Creates the request XML string.
        /// </summary>
        /// <param name="objRequestInfo">The requestinfo object.</param>
        /// <returns>Xml String</returns>
        public XmlDocument CreateRequestXML(RequestInfo requestInfo)
        {
            try
            {
                objXmlDocument = new XmlDocument();
                //Calling the CreateRootElement method.
                objXmlDocument = CreateRootElement(requestInfo);
            }
            catch(Exception)
            {
                throw;
            }
            return objXmlDocument;
        }

        #region Request/Response XML Logger added in DREAM 4.0
        /// <summary>
        /// Requests the XML logger.
        /// </summary>
        /// <param name="requestXMl">The request X ml.</param>
        public void XmlLogger(XmlDocument requestXMl, string xmlType, string docLibName)
        {
            objCommonUtility.XmlLogger(requestXMl, xmlType, docLibName);
        }
        #endregion
        /// <summary>
        /// Creates the root element.
        /// </summary>
        /// <param name="objRequestInfo">The request info object.</param>
        /// <returns></returns>
        private XmlDocument CreateRootElement(RequestInfo requestInformation)
        {
            try
            {
                //Creating the root Xml Element RequestInfo.
                XmlElement RequestInfoElement = objXmlDocument.CreateElement(REQUESTINFO);
                objXmlDocument.AppendChild(RequestInfoElement);

                //Creating the RequestInfo node with Name attribute.
                XmlNode RequestInfoNode = objXmlDocument.DocumentElement;
                //Method call to create Entity Node.
                CreateEntity(requestInformation, RequestInfoNode);
            }
            catch(Exception)
            {
                throw;
            }
            //returns the final SearchRequest Xml.
            return objXmlDocument;
        }

        /// <summary>
        /// Creates the entity.
        /// </summary>
        /// <param name="objRequestInfo">The requestinfo object.</param>
        /// <param name="EntitiesElement">The entities element.</param>
        private void CreateEntity(RequestInfo requestInformation, XmlNode requestInfoNode)
        {
            try
            {

                //Looping through the Entity ArrayList.
                Entity objEntity = requestInformation.Entity;
                XmlElement EntityElement = objXmlDocument.CreateElement(ENTITY);
                //Creating the Entity Element.
                requestInfoNode.AppendChild(EntityElement);

                //Added by Dev
                // Start
                //Adding username attribute to enity element
                XmlAttribute EntityUserName = objXmlDocument.CreateAttribute(USERNAME);
                EntityUserName.Value = objCommonUtility.GetUserName();
                EntityElement.Attributes.Append(EntityUserName);
                if(!string.IsNullOrEmpty(objEntity.SessionID))
                {
                    XmlAttribute SessionID = objXmlDocument.CreateAttribute(SESSIONID);
                    SessionID.Value = requestInformation.Entity.SessionID;
                    EntityElement.Attributes.Append(SessionID);
                }
                //Adding includeMetadata attribute for tool tip
                try
                {
                    if(!string.IsNullOrEmpty(PortalConfiguration.GetInstance().GetKey(INCLUDEMETADATA)))
                    {
                        XmlAttribute EntityIncludeMetadata = objXmlDocument.CreateAttribute(INCLUDEMETADATA.ToLowerInvariant());
                        EntityIncludeMetadata.Value = PortalConfiguration.GetInstance().GetKey(INCLUDEMETADATA);
                        EntityElement.Attributes.Append(EntityIncludeMetadata);
                    }
                }
                catch
                {
                    XmlAttribute EntityIncludeMetadata = objXmlDocument.CreateAttribute(INCLUDEMETADATA.ToLowerInvariant());
                    EntityIncludeMetadata.Value = "true";
                    EntityElement.Attributes.Append(EntityIncludeMetadata);
                }
                //Adding tvdss attribute for AH/TV depth calculation
                if(objEntity.TVDSS)
                {
                    XmlAttribute EntityTVDSS = objXmlDocument.CreateAttribute(TVDSS);
                    EntityTVDSS.Value = objEntity.TVDSS.ToString().ToLowerInvariant();
                    EntityElement.Attributes.Append(EntityTVDSS);
                }
                if(!string.IsNullOrEmpty(objEntity.ProjectName))
                {
                    XmlAttribute ProjectName = objXmlDocument.CreateAttribute(PROJECTNAME);
                    ProjectName.Value = objEntity.ProjectName;
                    EntityElement.Attributes.Append(ProjectName);
                }
                //end

                //checking the null check of request id
                if(requestInformation.Entity.SkipInfo != null)
                {
                    //Creating id attribute
                    if(!string.IsNullOrEmpty(requestInformation.Entity.RequestID))
                    {
                        XmlAttribute RequestID = objXmlDocument.CreateAttribute(ID);
                        EntityElement.Attributes.Append(RequestID);
                        RequestID.Value = requestInformation.Entity.RequestID;
                    }

                    //Creating Skip recourd count
                    XmlElement SkipRecords = objXmlDocument.CreateElement(SKIPRECORDS);
                    EntityElement.AppendChild(SkipRecords);
                    SkipRecords.InnerText = requestInformation.Entity.SkipInfo.SkipRecord;

                    //Creating Max record fetch
                    XmlElement MaxPageRecords = objXmlDocument.CreateElement(MAXPAGERECORDS);
                    EntityElement.AppendChild(MaxPageRecords);
                    MaxPageRecords.InnerText = requestInformation.Entity.SkipInfo.MaxFetch;
                }


                //Creating attributes for Entity Element.
                if(objEntity.Type.Length > 0)
                {
                    XmlAttribute EntityType = objXmlDocument.CreateAttribute(TYPE);
                    EntityElement.Attributes.Append(EntityType);
                    EntityType.Value = objEntity.Type;
                }


                //Creating attributes for Entity Element.
                if(objEntity.ResponseType.Length > 0)
                {
                    XmlAttribute EntityResponseType = objXmlDocument.CreateAttribute(RESPONSETYPE);
                    EntityElement.Attributes.Append(EntityResponseType);
                    EntityResponseType.Value = objEntity.ResponseType;
                }

                /// Creating attributes for Entity Element.
                if(objEntity.Name.Length > 0)
                {
                    XmlAttribute Entityname = objXmlDocument.CreateAttribute(NAME);
                    EntityElement.Attributes.Append(Entityname);
                    Entityname.Value = objEntity.Name;
                }


                //Creating attributes for Entity Element.
                if(objEntity.DataSource.Length > 0)
                {
                    XmlAttribute Entitydatasource = objXmlDocument.CreateAttribute(DATASOURCE.ToLowerInvariant());
                    EntityElement.Attributes.Append(Entitydatasource);
                    Entitydatasource.Value = objEntity.DataSource;
                }
                //Creating attributes for Entity Element.
                if(objEntity.DataProvider.Length > 0)
                {
                    XmlAttribute EntityDataprovider = objXmlDocument.CreateAttribute(DATAPROVIDERNAME.ToLowerInvariant());
                    EntityElement.Attributes.Append(EntityDataprovider);
                    EntityDataprovider.Value = objEntity.DataProvider;
                }

                XmlAttribute EntityProperty = objXmlDocument.CreateAttribute(PROPERTY);
                EntityElement.Attributes.Append(EntityProperty);

                EntityProperty.Value = Convert.ToString(objEntity.Property);


                if(objEntity.Display != null)          //Checks if Entity object contains AttributeGroup. 
                {
                    //Method call to create Display Node.
                    CreateDisplay(objEntity, EntityElement);
                }

                //Added to check the existance of Query node in the case of Save Search Functionality
                if(objEntity.Query != null)
                {
                    //Method call to create Query Node.
                    CreateQuery(objEntity, EntityElement);
                }


                if(objEntity.AttributeGroups != null)          //Checks if Entity object contains AttributeGroup. 
                {
                    //Method call to create AttributeGroup Node.
                    CreateAttributeGroup(objEntity, EntityElement);
                }
                if(objEntity.Attribute != null)               //Else the entity object should contain Attribute.
                {
                    //Method call to create Attribute Node.
                    CreateEntityAttribute(objEntity, EntityElement);
                }
                if(objEntity.Criteria != null)            //Checks if Entity object contains Criteria.
                {
                    //Method call to create Criteria Node.
                    CreateCriteria(objEntity, EntityElement);
                }
            }
            catch(Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="objEntity"></param>
        /// <param name="EntityElement"></param>
        private void CreateDisplay(Entity objEntity, XmlElement EntityElement)
        {
            Display objDisplay = objEntity.Display;
            XmlElement DisplayElement = objXmlDocument.CreateElement(DISPLAY);
            EntityElement.AppendChild(DisplayElement);

            if(objDisplay.Value != null)
            {
                CreateValue(objDisplay, DisplayElement);
            }
        }


        /// <summary>
        /// Creates the attributegroup Node.
        /// </summary>
        /// <param name="objEntity">The entity object.</param>
        /// <param name="EntityElement">The entity element.</param>
        private void CreateAttributeGroup(Entity entityInformation, XmlElement entityElement)
        {
            try
            {
                //Looping through all the AttributeGroup object.
                foreach(AttributeGroup objAttributeGroup in entityInformation.AttributeGroups)
                {
                    if(objAttributeGroup.AttributeGroups != null)
                    {
                        XmlElement AttributeGroupElement = objXmlDocument.CreateElement(ATTRIBUTEGROUP);
                        entityElement.AppendChild(AttributeGroupElement);

                        //Creating attribute for AttributeGroup Element.
                        XmlAttribute AttributeGroupOperator = objXmlDocument.CreateAttribute(OPERATOR);
                        AttributeGroupElement.Attributes.Append(AttributeGroupOperator);
                        AttributeGroupOperator.Value = objAttributeGroup.Operator;

                        if(objAttributeGroup.Name.ToString().Length > 0)
                        {
                            XmlAttribute AttributeGroupName = objXmlDocument.CreateAttribute(NAME);
                            AttributeGroupElement.Attributes.Append(AttributeGroupName);
                            AttributeGroupName.Value = objAttributeGroup.Name;
                        }

                        if(objAttributeGroup.Label.ToString().Length > 0)
                        {
                            XmlAttribute AttributeGroupLabel = objXmlDocument.CreateAttribute(LABEL);
                            AttributeGroupElement.Attributes.Append(AttributeGroupLabel);
                            AttributeGroupLabel.Value = objAttributeGroup.Label;
                        }

                        if(objAttributeGroup.Checked.ToString().Length > 0)
                        {
                            XmlAttribute AttributeGroupChecked = objXmlDocument.CreateAttribute(CHECKED);
                            AttributeGroupElement.Attributes.Append(AttributeGroupChecked);
                            AttributeGroupChecked.Value = objAttributeGroup.Checked;
                        }

                        CreateChildAttributeGroup(objAttributeGroup, AttributeGroupElement);
                    }
                    if(objAttributeGroup.Attribute != null)
                    {
                        //creating the AttributeGroup Element.
                        XmlElement AttributeGroupElement = objXmlDocument.CreateElement(ATTRIBUTEGROUP);
                        entityElement.AppendChild(AttributeGroupElement);

                        //Creating attribute for AttributeGroup Element.
                        XmlAttribute AttributeGroupOperator = objXmlDocument.CreateAttribute(OPERATOR);
                        AttributeGroupElement.Attributes.Append(AttributeGroupOperator);
                        AttributeGroupOperator.Value = objAttributeGroup.Operator;

                        if(objAttributeGroup.Name.ToString().Length > 0)
                        {
                            XmlAttribute AttributeGroupName = objXmlDocument.CreateAttribute(NAME);
                            AttributeGroupElement.Attributes.Append(AttributeGroupName);
                            AttributeGroupName.Value = objAttributeGroup.Name;
                        }

                        if(objAttributeGroup.Label.ToString().Length > 0)
                        {
                            XmlAttribute AttributeGroupLabel = objXmlDocument.CreateAttribute(LABEL);
                            AttributeGroupElement.Attributes.Append(AttributeGroupLabel);
                            AttributeGroupLabel.Value = objAttributeGroup.Label;
                        }

                        if(objAttributeGroup.Checked.ToString().Length > 0)
                        {
                            XmlAttribute AttributeGroupChecked = objXmlDocument.CreateAttribute(CHECKED);
                            AttributeGroupElement.Attributes.Append(AttributeGroupChecked);
                            AttributeGroupChecked.Value = objAttributeGroup.Checked;
                        }

                        //Method Call to create Attribute Node.
                        CreateAttributes(objAttributeGroup, AttributeGroupElement);
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Creates the child attribute group.
        /// </summary>
        /// <param name="objAttributeGroup">The obj attribute group.</param>
        /// <param name="xmlelmentAttributeGroup">The xmlelment attribute group.</param>
        private XmlElement CreateChildAttributeGroup(AttributeGroup childAttributeGroup, XmlElement parentAttributeGroup)
        {
            try
            {
                foreach(AttributeGroup objChildAttributeGroup in childAttributeGroup.AttributeGroups)
                {
                    if(objChildAttributeGroup.AttributeGroups != null)
                    {
                        XmlElement AttributeGroupElement = objXmlDocument.CreateElement(ATTRIBUTEGROUP);
                        parentAttributeGroup.AppendChild(AttributeGroupElement);

                        //Creating attribute for AttributeGroup Element.
                        XmlAttribute AttributeGroupOperator = objXmlDocument.CreateAttribute(OPERATOR);
                        AttributeGroupElement.Attributes.Append(AttributeGroupOperator);
                        AttributeGroupOperator.Value = objChildAttributeGroup.Operator;

                        if(objChildAttributeGroup.Name.ToString().Length > 0)
                        {
                            XmlAttribute AttributeGroupName = objXmlDocument.CreateAttribute(NAME);
                            AttributeGroupElement.Attributes.Append(AttributeGroupName);
                            AttributeGroupName.Value = objChildAttributeGroup.Name;
                        }

                        if(objChildAttributeGroup.Label.ToString().Length > 0)
                        {
                            XmlAttribute AttributeGroupLabel = objXmlDocument.CreateAttribute(LABEL);
                            AttributeGroupElement.Attributes.Append(AttributeGroupLabel);
                            AttributeGroupLabel.Value = objChildAttributeGroup.Label;
                        }

                        if(objChildAttributeGroup.Checked.ToString().Length > 0)
                        {
                            XmlAttribute AttributeGroupChecked = objXmlDocument.CreateAttribute(CHECKED);
                            AttributeGroupElement.Attributes.Append(AttributeGroupChecked);
                            AttributeGroupChecked.Value = objChildAttributeGroup.Checked;
                        }

                        CreateChildAttributeGroup(objChildAttributeGroup, AttributeGroupElement);
                    }
                    if(objChildAttributeGroup.Attribute != null)
                    {
                        //creating the AttributeGroup Element.
                        XmlElement AttributeGroupElement = objXmlDocument.CreateElement(ATTRIBUTEGROUP);
                        parentAttributeGroup.AppendChild(AttributeGroupElement);

                        //Creating attribute for AttributeGroup Element.
                        XmlAttribute AttributeGroupOperator = objXmlDocument.CreateAttribute(OPERATOR);
                        AttributeGroupElement.Attributes.Append(AttributeGroupOperator);
                        AttributeGroupOperator.Value = objChildAttributeGroup.Operator;

                        if(objChildAttributeGroup.Name.ToString().Length > 0)
                        {
                            XmlAttribute AttributeGroupName = objXmlDocument.CreateAttribute(NAME);
                            AttributeGroupElement.Attributes.Append(AttributeGroupName);
                            AttributeGroupName.Value = objChildAttributeGroup.Name;
                        }

                        if(objChildAttributeGroup.Label.ToString().Length > 0)
                        {
                            XmlAttribute AttributeGroupLabel = objXmlDocument.CreateAttribute(LABEL);
                            AttributeGroupElement.Attributes.Append(AttributeGroupLabel);
                            AttributeGroupLabel.Value = objChildAttributeGroup.Label;
                        }

                        if(objChildAttributeGroup.Checked.ToString().Length > 0)
                        {
                            XmlAttribute AttributeGroupChecked = objXmlDocument.CreateAttribute(CHECKED);
                            AttributeGroupElement.Attributes.Append(AttributeGroupChecked);
                            AttributeGroupChecked.Value = objChildAttributeGroup.Checked;
                        }

                        //Method Call to create Attribute Node.
                        CreateAttributes(objChildAttributeGroup, AttributeGroupElement);
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
            return parentAttributeGroup;
        }

        /// <summary>
        /// Creates the attributes Node.
        /// </summary>
        /// <param name="objAttrbuteGroup">The attributegroup object.</param>
        /// <param name="AttributeGroupElement">The attributegroup element.</param>
        private void CreateAttributes(AttributeGroup attributeGroup, XmlElement attributeGroupElement)
        {
            try
            {
                if(attributeGroup.AttributeGroups != null)
                {
                    foreach(AttributeGroup objAttributeGroupChild in attributeGroup.AttributeGroups)
                    {
                        //Loop through all the Attributes object in AttributeGroup object.
                        foreach(Attributes objAttribute in objAttributeGroupChild.Attribute)
                        {
                            //creating the Attribute Element.
                            XmlElement AttributeElement = objXmlDocument.CreateElement(ATTRIBUTE);
                            attributeGroupElement.AppendChild(AttributeElement);

                            //Creating attributes for Attribute Element.
                            if(objAttribute.Name.ToString().Length > 0)
                            {
                                XmlAttribute AttributeName = objXmlDocument.CreateAttribute(NAME);
                                AttributeElement.Attributes.Append(AttributeName);
                                AttributeName.Value = objAttribute.Name;
                            }
                            //Added For DWB Chapter.
                            if(objAttribute.DisplayField.ToString().Length > 0)
                            {
                                XmlAttribute AttributeDisplayField = objXmlDocument.CreateAttribute("displayonlyselectedcolumn");
                                AttributeElement.Attributes.Append(AttributeDisplayField);
                                AttributeDisplayField.Value = objAttribute.DisplayField;
                            }

                            if(objAttribute.Operator.ToString().Length > 0)
                            {
                                XmlAttribute AttributeOperator = objXmlDocument.CreateAttribute(OPERATOR);
                                AttributeElement.Attributes.Append(AttributeOperator);
                                AttributeOperator.Value = objAttribute.Operator;
                            }

                            if(objAttribute.Label.ToString().Length > 0)
                            {
                                XmlAttribute AttributeLabel = objXmlDocument.CreateAttribute(LABEL);
                                AttributeElement.Attributes.Append(AttributeLabel);
                                AttributeLabel.Value = objAttribute.Label;
                            }

                            if(objAttribute.Checked.ToString().Length > 0)
                            {
                                XmlAttribute AttributeCheck = objXmlDocument.CreateAttribute(CHECKED);
                                AttributeElement.Attributes.Append(AttributeCheck);
                                AttributeCheck.Value = objAttribute.Checked;
                            }

                            if(objAttribute.Value != null)         //Checks if Attribute contains Value object.
                            {
                                //Method Call to create Value Node.
                                CreateValue(objAttribute, AttributeElement);
                            }
                            if(objAttribute.Parameter != null)    //Checks if Attribute contains Parameter object.
                            {
                                //Method Call to create Parameter Node.
                                CreateParameter(objAttribute, AttributeElement);
                            }
                            if(objAttribute.IsRangeApplicable.ToString().Length > 0)
                            {

                                XmlAttribute AttributeIsRange = objXmlDocument.CreateAttribute(ISRANGEAPPLICABLE);
                                AttributeElement.Attributes.Append(AttributeIsRange);
                                AttributeIsRange.Value = objAttribute.IsRangeApplicable.ToString();
                            }
                        }
                    }
                }
                if(attributeGroup.Attribute != null)
                {
                    //Loop through all the Attributes object in AttributeGroup object.
                    foreach(Attributes objAttribute in attributeGroup.Attribute)
                    {
                        //creating the Attribute Element.
                        XmlElement AttributeElement = objXmlDocument.CreateElement(ATTRIBUTE);
                        attributeGroupElement.AppendChild(AttributeElement);

                        //Creating attributes for Attribute Element.
                        if(objAttribute.Name.Length > 0)
                        {
                            XmlAttribute AttributeName = objXmlDocument.CreateAttribute(NAME);
                            AttributeElement.Attributes.Append(AttributeName);
                            AttributeName.Value = objAttribute.Name;
                        }

                        //Added For DWB Chapter.
                        if(objAttribute.DisplayField.ToString().Length > 0)
                        {
                            XmlAttribute AttributeDisplayField = objXmlDocument.CreateAttribute("displayonlyselectedcolumn");
                            AttributeElement.Attributes.Append(AttributeDisplayField);
                            AttributeDisplayField.Value = objAttribute.DisplayField;
                        }

                        if(objAttribute.Operator.Length > 0)
                        {
                            XmlAttribute AttributeOperator = objXmlDocument.CreateAttribute(OPERATOR);
                            AttributeElement.Attributes.Append(AttributeOperator);
                            AttributeOperator.Value = objAttribute.Operator;
                        }

                        if(objAttribute.Label.Length > 0)
                        {
                            XmlAttribute AttributeLabel = objXmlDocument.CreateAttribute(LABEL);
                            AttributeElement.Attributes.Append(AttributeLabel);
                            AttributeLabel.Value = objAttribute.Label;
                        }

                        if(objAttribute.Checked.Length > 0)
                        {
                            XmlAttribute AttributeCheck = objXmlDocument.CreateAttribute(CHECKED);
                            AttributeElement.Attributes.Append(AttributeCheck);
                            AttributeCheck.Value = objAttribute.Checked;
                        }

                        if(objAttribute.Value != null)
                        {
                            //Method Call to create Value Node.
                            CreateValue(objAttribute, AttributeElement);
                        }
                        else if(objAttribute.Parameter != null)
                        {
                            //Method Call to create Parameter Node.
                            CreateParameter(objAttribute, AttributeElement);
                        }

                        if(objAttribute.IsRangeApplicable.ToString().Length > 0)
                        {

                            XmlAttribute AttributeIsRange = objXmlDocument.CreateAttribute(ISRANGEAPPLICABLE);
                            AttributeElement.Attributes.Append(AttributeIsRange);
                            AttributeIsRange.Value = objAttribute.IsRangeApplicable.ToString();
                        }
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Creates the criteria Node.
        /// </summary>
        /// <param name="objEntity">The entity object.</param>
        /// <param name="EntityElement">The entity element.</param>
        private void CreateCriteria(Entity entityInformation, XmlElement entityElement)
        {
            try
            {
                //creating the Criteria Element.
                Criteria objCriteria = entityInformation.Criteria;
                XmlElement CriteriaElement = objXmlDocument.CreateElement(CRITERIA);
                entityElement.AppendChild(CriteriaElement);

                if(entityInformation.Criteria.Name.ToString().Length > 0)
                {
                    //Creating attributes for Criteria Element.
                    XmlAttribute CriteriaName = objXmlDocument.CreateAttribute(NAME);
                    CriteriaElement.Attributes.Append(CriteriaName);
                    CriteriaName.Value = entityInformation.Criteria.Name;
                }
                //Added for DWB Chapter.
                if(entityInformation.Criteria.DisplayField.ToString().Length > 0)
                {
                    //Creating attributes for Criteria Element.
                    XmlAttribute CriteriaDisplayField = objXmlDocument.CreateAttribute("displayonlyselectedcolumn");
                    CriteriaElement.Attributes.Append(CriteriaDisplayField);
                    CriteriaDisplayField.Value = entityInformation.Criteria.DisplayField;
                }

                //Creating attributes for Criteria Element.
                if(entityInformation.Criteria.Value.ToString().Length > 0)
                {
                    XmlAttribute CriteriaValue = objXmlDocument.CreateAttribute(VALUE);
                    CriteriaElement.Attributes.Append(CriteriaValue);
                    CriteriaValue.Value = entityInformation.Criteria.Value;
                }

                if(entityInformation.Criteria.Operator.ToString().Length > 0)
                {
                    XmlAttribute CriteriaOperator = objXmlDocument.CreateAttribute(OPERATOR);
                    CriteriaElement.Attributes.Append(CriteriaOperator);
                    CriteriaOperator.Value = entityInformation.Criteria.Operator;
                }
            }
            catch(Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="objCriteria"></param>
        /// <param name="CriteriaElement"></param>
        private void CreateQuery(Entity objEntity, XmlElement EntityElement)
        {
            try
            {
                Query objQuery = objEntity.Query;
                XmlElement QueryElement = objXmlDocument.CreateElement(QUERY);
                EntityElement.AppendChild(QueryElement);
                //Setting the Innertext value for each value.
                QueryElement.InnerText = objQuery.InnerText;
                //Creating attributes for Query Element.
                if(objQuery.Type.ToString().Length > 0)
                {
                    XmlAttribute QueryType = objXmlDocument.CreateAttribute("Type");
                    QueryElement.Attributes.Append(QueryType);
                    QueryType.Value = objQuery.Type;
                }
            }
            catch(Exception)
            {
                throw;
            }
        }



        /// <summary>
        /// Creates the parameter Node.
        /// </summary>
        /// <param name="objAttribute">The attribute object.</param>
        /// <param name="AttributeElement">The attribute element.</param>
        private void CreateParameter(Attributes attribute, XmlElement attributeElement)
        {
            try
            {
                //Looping through all the Parameter object in Attributes object.
                foreach(Parameters objParameter in attribute.Parameter)
                {
                    //creating the Parameter Element.
                    XmlElement ParameterElement = objXmlDocument.CreateElement(PARAMETER);
                    attributeElement.AppendChild(ParameterElement);

                    //Creating attribute for Parameter Element.
                    XmlAttribute ParameterName = objXmlDocument.CreateAttribute(NAME);
                    ParameterElement.Attributes.Append(ParameterName);
                    ParameterName.Value = objParameter.Name;

                    XmlAttribute ParameterValue = objXmlDocument.CreateAttribute(VALUE);
                    ParameterElement.Attributes.Append(ParameterValue);
                    ParameterValue.Value = objParameter.Value;

                    XmlAttribute ParameterLabel = objXmlDocument.CreateAttribute(LABEL);
                    ParameterElement.Attributes.Append(ParameterLabel);
                    ParameterLabel.Value = objParameter.Label;
                }
            }
            catch(Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Creates the value.
        /// </summary>
        /// <param name="objAttribute">The attribute Object.</param>
        /// <param name="AttributeElement">The attribute element.</param>
        private void CreateValue(Attributes attribute, XmlElement attributeElement)
        {
            try
            {
                //Loop through all the value object in Attribute object.
                foreach(Value objValue in attribute.Value)
                {
                    //creating the Value Element.
                    XmlElement ValueElement = objXmlDocument.CreateElement(VALUE);
                    attributeElement.AppendChild(ValueElement);
                    //Setting the Innertext value for each value.
                    ValueElement.InnerText = objValue.InnerText;
                }
            }
            catch(Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Creates the value.
        /// </summary>
        /// <param name="objAttribute">The attribute Object.</param>
        /// <param name="AttributeElement">The attribute element.</param>
        private void CreateValue(Display display, XmlElement displayElement)
        {
            try
            {
                //Loop through all the value object in Attribute object.
                foreach(Value objValue in display.Value)
                {
                    //creating the Value Element.
                    XmlElement ValueElement = objXmlDocument.CreateElement(VALUE);
                    displayElement.AppendChild(ValueElement);
                    //Setting the Innertext value for each value.
                    ValueElement.InnerText = objValue.InnerText;
                }
            }
            catch(Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Creates the entity attribute.
        /// </summary>
        /// <param name="objEntity">The entity object.</param>
        /// <param name="EntityElement">The entity element.</param>
        private void CreateEntityAttribute(Entity entity, XmlElement entityElement)
        {
            try
            {
                //Loop through all the Attributes object in Entity object.
                foreach(Attributes objAttribute in entity.Attribute)
                {
                    if(objAttribute.Value != null)
                    {
                        foreach(Value objValue in objAttribute.Value)
                        {
                            //creating the Attribute Element.
                            XmlElement AttributeElement = objXmlDocument.CreateElement(ATTRIBUTE);
                            entityElement.AppendChild(AttributeElement);

                            //Creating the attribute for each attribute Element.
                            XmlAttribute AttributeName = objXmlDocument.CreateAttribute(NAME);
                            AttributeElement.Attributes.Append(AttributeName);
                            AttributeName.Value = objAttribute.Name;

                            if(objAttribute.Type.Length > 0)
                            {
                                XmlAttribute AttributeType = objXmlDocument.CreateAttribute("type");
                                AttributeElement.Attributes.Append(AttributeType);
                                AttributeType.Value = objAttribute.Type;
                            }

                            if(objAttribute.Operator.ToString().Length > 0)
                            {
                                XmlAttribute AttributeOperator = objXmlDocument.CreateAttribute(OPERATOR);
                                AttributeElement.Attributes.Append(AttributeOperator);
                                AttributeOperator.Value = objAttribute.Operator;
                            }

                            if(objAttribute.Label.ToString().Length > 0)
                            {
                                XmlAttribute AttributeLabel = objXmlDocument.CreateAttribute(LABEL);
                                AttributeElement.Attributes.Append(AttributeLabel);
                                AttributeLabel.Value = objAttribute.Label;
                            }

                            if(objAttribute.Checked.ToString().Length > 0)
                            {

                                XmlAttribute AttributeCheck = objXmlDocument.CreateAttribute(CHECKED);
                                AttributeElement.Attributes.Append(AttributeCheck);
                                AttributeCheck.Value = objAttribute.Checked;
                            }
                            if(objAttribute.IsRangeApplicable.ToString().Length > 0)
                            {

                                XmlAttribute AttributeIsRange = objXmlDocument.CreateAttribute(ISRANGEAPPLICABLE);
                                AttributeElement.Attributes.Append(AttributeIsRange);
                                AttributeIsRange.Value = objAttribute.IsRangeApplicable.ToString();
                            }

                            if(objAttribute.Value != null)     //Checks if Attribute object contains Value object.
                            {
                                CreateValue(objAttribute, AttributeElement);
                            }
                            if(objAttribute.Parameter != null)    //Checks if Attribute object contains Parameter object.
                            {
                                CreateParameter(objAttribute, AttributeElement);
                            }
                            break;
                        }
                    }
                    else if(objAttribute.Parameter != null)
                    {
                        //creating the Attribute Element.
                        XmlElement AttributeElement = objXmlDocument.CreateElement(ATTRIBUTE);
                        entityElement.AppendChild(AttributeElement);

                        //Creating the attribute for each attribute Element.
                        XmlAttribute AttributeName = objXmlDocument.CreateAttribute(NAME);
                        AttributeElement.Attributes.Append(AttributeName);
                        AttributeName.Value = objAttribute.Name;

                        if(objAttribute.Type.Length > 0)
                        {
                            XmlAttribute AttributeType = objXmlDocument.CreateAttribute("type");
                            AttributeElement.Attributes.Append(AttributeType);
                            AttributeType.Value = objAttribute.Type;
                        }

                        XmlAttribute AttributeOperator = objXmlDocument.CreateAttribute(OPERATOR);
                        AttributeElement.Attributes.Append(AttributeOperator);
                        AttributeOperator.Value = objAttribute.Operator;

                        XmlAttribute AttributeLabel = objXmlDocument.CreateAttribute(LABEL);
                        AttributeElement.Attributes.Append(AttributeLabel);
                        AttributeLabel.Value = objAttribute.Label;

                        XmlAttribute AttributeCheck = objXmlDocument.CreateAttribute(CHECKED);
                        AttributeElement.Attributes.Append(AttributeCheck);
                        AttributeCheck.Value = objAttribute.Checked;

                        XmlAttribute AttributeIsRange = objXmlDocument.CreateAttribute(ISRANGEAPPLICABLE);
                        AttributeElement.Attributes.Append(AttributeIsRange);
                        AttributeIsRange.Value = objAttribute.IsRangeApplicable.ToString();

                        if(objAttribute.Value != null)     //Checks if Attribute object contains Value object.
                        {
                            CreateValue(objAttribute, AttributeElement);
                        }
                        if(objAttribute.Parameter != null)    //Checks if Attribute object contains Parameter object.
                        {
                            CreateParameter(objAttribute, AttributeElement);
                        }
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
