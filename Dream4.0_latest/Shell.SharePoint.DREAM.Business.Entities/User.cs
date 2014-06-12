#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: User.cs 
#endregion
/// <summary> 
/// This class is used to create a User object
/// </summary> 

namespace Shell.SharePoint.DREAM.Business.Entities
{
    /// <summary>
    /// User Class.
    /// </summary>
    public class User
    {
        #region DECLARATION
        string strUserID = string.Empty;
        string strName = string.Empty;        
        string strDepartment = string.Empty;
        string strDepartmentCode = string.Empty;
        string strCompany = string.Empty;
        string strLocation = string.Empty;
        string strTelephoneNumber = string.Empty;
        string strMobileNumber = string.Empty;
        string strOtherTelephoneNumber = string.Empty;
        string strExtnNumber = string.Empty;
        string strEmail = string.Empty;
        string strRole = string.Empty;
        string strPostalCode = string.Empty;
        string strCountry = string.Empty;

        #endregion

        #region PROPERTIES        
        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        /// <value>The user ID.</value>
        public string UserID
        {
            get
            {
                return strUserID;
            }
            set
            {
                strUserID = value;
            }
        }
        /// <summary>
        /// Gets or sets the DataOwner Name.
        /// </summary>
        /// <value>The name</value>
        public string Name
        {
            get
            {
                return strName;
            }
            set
            {
                strName = value;
            }
        }

        /// <summary>
        /// Gets or sets the Department Name.
        /// </summary>
        /// <value>Department</value>
        public string Department
        {
            get
            {
                return strDepartment;
            }
            set
            {
                strDepartment = value;
            }
        }
        /// <summary>
        /// Gets or sets the DepartmentCode of DataOwner.
        /// </summary>
        /// <value>Department Code</value>
        public string DepartmentCode
        {
            get
            {
                return strDepartmentCode;
            }
            set
            {
                strDepartmentCode = value;
            }
        }

        /// <summary>
        /// Gets or sets the Company Name.
        /// </summary>
        /// <value>Company Name</value>
        public string Company
        {
            get
            {
                return strCompany;
            }
            set
            {
                strCompany = value;
            }
        }
        /// <summary>
        /// Gets or sets the Location Name.
        /// </summary>
        /// <value>Location</value>
        public string Location
        {
            get
            {
                return strLocation;
            }
            set
            {
                strLocation = value;
            }
        }
        /// <summary>
        /// Gets or sets the Telephone Number of dataowner.
        /// </summary>
        /// <value>Telephone Number</value>
        public string TelephoneNumber
        {
            get
            {
                return strTelephoneNumber;
            }
            set
            {
                strTelephoneNumber = value;
            }
        }
        /// <summary>
        /// Gets or sets the Extn Number of dataowner.
        /// </summary>
        /// <value>Extn Number</value>
        public string ExtnNumber
        {
            get
            {
                return strExtnNumber;
            }
            set
            {
                strExtnNumber = value;
            }
        }
        /// <summary>
        /// Gets or sets the OtherTelephoneNumber of dataowner.
        /// </summary>
        /// <value>Other Telephone Number</value>
        public string OtherTelephoneNumber
        {
            get
            {
                return strOtherTelephoneNumber;
            }
            set
            {
                strOtherTelephoneNumber = value;
            }
        }
        /// <summary>
        /// Gets or sets the Mobile Number of dataowner.
        /// </summary>
        /// <value>Mobile Number</value>
        public string MobileNumber
        {
            get
            {
                return strMobileNumber;
            }
            set
            {
                strMobileNumber = value;
            }
        }        
        /// <summary>
        /// Gets or sets the Email address of dataowner.
        /// </summary>
        /// <value>Email address</value>
        public string Email
        {
            get
            {
                return strEmail;
            }
            set
            {
                strEmail = value;
            }
        }
        /// <summary>
        /// Gets or sets the Role Name.
        /// </summary>
        /// <value>Role</value>
        public string Role
        {
            get
            {
                return strRole;
            }
            set
            {
                strRole = value;
            }
        }
        /// <summary>
        /// Gets or sets the PostalCode Name.
        /// </summary>
        /// <value>Postal Code</value>
        public string PostalCode
        {
            get
            {
                return strPostalCode;
            }
            set
            {
                strPostalCode = value;
            }
        }
        /// <summary>
        /// Gets or sets the Country name
        /// </summary>
        /// <value>Country</value>
        public string Country
        {
            get
            {
                return strCountry;
            }
            set
            {
                strCountry = value;
            }
        }
        #endregion

        #region Public Method
        /// <summary>
        /// This method assigns value to property of user object
        /// </summary>
        /// <param name="key">Key mapped with property,it is AD key</param>
        /// <param name="value">Value of property to be assigned</param>
        public void AddUserProperty(string key, string value)
        {
            //keys are mapped according to AD keys which is define in ADPropertyKey Enum class
            switch (key)
            {
                case "samaccountname": //userid
                    this.UserID = value;
                    break;
                case "displayname":
                    this.Name = value;
                    break;
                case "department":
                    this.Department = value;
                    break;
                case "shellggddepartmentnumber"://departmentcode
                    this.DepartmentCode = value;
                    break;
                case "company":
                    this.Company = value;
                    break;
                case "location":
                    this.Location = value;
                    break;
                case "telephonenumber":
                    this.TelephoneNumber = value;
                    break;
                case "mobile":
                    this.MobileNumber = value;
                    break;
                case "othertelephone":
                    this.OtherTelephoneNumber = value;
                    break;
                case "shellggdtelephoneextension"://extension number
                    this.ExtnNumber = value;
                    break;
                case "mail":
                    this.Email = value;
                    break;
                case "title":
                    this.Role = value;
                    break;
                case "postalcode":
                    this.PostalCode = value;
                    break;
                case "co":
                    this.Country = value;
                    break;
            }

        } 
        #endregion
    }
}
