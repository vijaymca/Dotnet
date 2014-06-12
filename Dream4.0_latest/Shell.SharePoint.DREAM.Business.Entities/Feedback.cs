#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: Feedback.cs 
#endregion
/// <summary> 
/// This class has get/set methods to handle Feedback 
/// </summary>
using System;

namespace Shell.SharePoint.DREAM.Business.Entities
{
    /// <summary>
    /// The Feedback class
    /// </summary>
    [Serializable]
    public class Feedback
    {
        #region DECLARATION
        private string strPageName = string.Empty;
        private string strComments = string.Empty;
        private string strTypeofFeedback = string.Empty;
        private byte[] attachedFile = null;
        private string strFileName = string.Empty;
        private string strRating = string.Empty;
        private string strReason = string.Empty;
        private string strInformation = string.Empty;

        #endregion
        #region PROPERTIES
        /// <summary>
        /// Property to Get or sets the Page Name.
        /// </summary>
        /// <value>The display.</value>
        public string PageName
        {
            get
            {
                return strPageName;
            }
            set
            {
                strPageName = value;
            }
        }
        /// <summary>
        /// Property to Get or sets the Comments
        /// </summary>
        /// <value>The display.</value>
        public string Comment
        {
            get
            {
                return strComments;
            }
            set
            {
                strComments = value;
            }
        }
               /// <summary>
        /// Property to Gets or sets the Type of Feedback
        /// </summary>
        /// <value>The display.</value>
        public string TypeofFeedback
        {
            get
            {
                return strTypeofFeedback;
            }
            set
            {
                strTypeofFeedback = value;
            }
        }

        /// <summary>
        /// Gets or Sets the attached file
        /// </summary>
        public byte[] FileAttached
        {
            get
            {
                return attachedFile;
            }
            set
            {
                attachedFile = value; 
            }
        }

        /// <summary>
        /// Gets or Sets the attached file name
        /// </summary>
        public string FileName
        {
            get
            {
                return strFileName;
            }
            set
            {
                strFileName = value;
            }
        }

        /// <summary>
        /// Property to Gets or sets the Rating.
        /// </summary>
        /// <value>The display.</value>
        public string Rating
        {
            get
            {
                return strRating;
            }
            set
            {
                strRating = value;
            }
        }
        /// <summary>
        /// Property to Gets or sets the Reason for Rating.
        /// </summary>
        /// <value>The display.</value>
        public string Reason
        {
            get
            {
                return strReason;
            }
            set
            {
                strReason = value;
            }
        }
        /// <summary>
        /// Property to Gets or sets the Additional Information.
        /// </summary>
        /// <value>The display.</value>
        public string AdditionalInformation
        {
            get
            {
                return strInformation;
            }
            set
            {
                strInformation = value;
            }
        }

        #endregion
    }
}
