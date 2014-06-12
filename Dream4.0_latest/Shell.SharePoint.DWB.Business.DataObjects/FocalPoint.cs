#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: FocalPoint.cs 
#endregion

using System;

namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// Gets/Sets the Books the logged in user owns
    /// Teams the logged in user is part of
    /// Pages the logged in user owns
    /// </summary>
    [Serializable]
    public class FocalPoint
    {
        #region Declarations
        string strBookIds = string.Empty;
        string strPageIds = string.Empty;
        string strTeamIds = string.Empty;
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets/Sets the Books the logged in user owns
        /// IDs separated by "|"
        /// </summary>
        public string BookIDs
        {
            get { return strBookIds; }
            set { strBookIds = value; }
        }
        /// <summary>
        /// Gets/Sets Pages the logged in user owns
        /// IDs separated by "|"
        /// </summary>
        public string PageIDs
        {
            get { return strPageIds; }
            set { strPageIds = value; }
        }

        /// <summary>       
        /// Teams the logged in user is part of       
        /// IDs separated by "|"
        /// </summary>
        public string TeamIDs
        {
            get { return strTeamIds; }
            set { strTeamIds = value; }
        }
        #endregion
    }
}
