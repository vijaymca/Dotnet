#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: WellBookDetails.cs 
#endregion
using System;

namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// Gets/Sets the properties of Well Book.
    /// </summary>
    /// 
    [Serializable]
    public class WellBookDetails
    {
        #region DECLARATION

        string strTitle = string.Empty;
        string strTeam = string.Empty;
        string strTeamID = string.Empty;
        string strBookOwner = string.Empty;
        string strBookOwnerID = string.Empty;
        string strTerminated = string.Empty;
        string strSignOffStatus = string.Empty;
        int intRowId;
        int intNoOfActiveChapters;
        int intNoOfBookPages;
        #endregion

        #region PROPERTIES

        /// <summary>
        /// Gets/Sets Well Book ID.
        /// </summary>
        public int RowId
        {
            get { return intRowId; }
            set { intRowId = value; }
        }
        /// <summary>
        /// Gets/Sets Well Book Title.
        /// </summary>
        public string Title
        {
            get { return strTitle; }
            set { strTitle = value; }
        }

        /// <summary>
        /// Gets/Sets Well Book sign off status.
        /// </summary>
        public string SignOffStatus
        {
            get { return strSignOffStatus; }
            set { strSignOffStatus = value; }
        }
        /// <summary>
        /// Gets/Sets Well Book Owner.
        /// </summary>
        public string BookOwner
        {
            get { return strBookOwner; }
            set { strBookOwner = value; }
        }
        /// <summary>
        /// Gets/Sets Well Book Owner ID.
        /// </summary>
        public string BookOwnerID
        {
            get { return strBookOwnerID; }
            set { strBookOwnerID = value; }
        }

        /// <summary>
        /// Gets/Sets Well Book terminated or not.
        /// </summary>
        public string Terminated
        {
            get { return strTerminated; }
            set { strTerminated = value; }
        }

        /// <summary>
        /// Gets/Sets Well Book Team Name.
        /// </summary>
        public string Team
        {
            get { return strTeam; }
            set { strTeam = value; }
        }

        /// <summary>
        /// Gets/Sets Well Book Team ID.
        /// </summary>
        public string TeamID
        {
            get { return strTeamID; }
            set { strTeamID = value; }
        }

        /// <summary>
        /// Gets/Sets no of Active Chapters in a Book
        /// </summary>
        public int NoOfActiveChapters
        {
            get { return intNoOfActiveChapters; }
            set { intNoOfActiveChapters = value; }
        }

        /// <summary>
        /// Gets or sets the no of pages in a Book.
        /// </summary>
        /// <value>The no of book pages.</value>
        public int NoOfBookPages
        {
            get { return intNoOfBookPages; }
            set { intNoOfBookPages = value; }
        }
        #endregion

    }
}
