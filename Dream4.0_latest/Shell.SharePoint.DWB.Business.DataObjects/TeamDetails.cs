#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: TeamDetails.cs 
#endregion
using System;

namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// Gets/Sets the properties of DWB Team.
    /// </summary>
    /// 
    [Serializable]
    public class TeamDetails
    {
        #region DECLARATION
        string strTeamName = string.Empty;
        string strAssetOwner = string.Empty;
        string strAssetOwnerID = string.Empty;
        string strTerminated = string.Empty;

        int intRowId;

        #endregion DECLARATION

        #region PROPERTIES

        /// <summary>
        /// Gets/Sets Team ID.
        /// </summary>
        public int RowId
        {
            get { return intRowId; }
            set { intRowId = value; }
        }
        /// <summary>
        /// Gets/Sets Team is terminated or not.
        /// </summary>
        public string Terminated
        {
            get { return strTerminated; }
            set { strTerminated = value; }
        }
        /// <summary>
        /// Gets/Sets Team Name.
        /// </summary>
        public string TeamName
        {
            get { return strTeamName; }
            set { strTeamName = value; }
        }
        /// <summary>
        /// Gets/Sets Team Asset Owner.
        /// </summary>
        public string AssetOwner
        {
            get { return strAssetOwner; }
            set { strAssetOwner = value; }
        }
        /// <summary>
        /// Gets/Sets Team Asset Owner ID.
        /// </summary>
        public string AssetOwnerID
        {
            get { return strAssetOwnerID; }
            set { strAssetOwnerID = value; }
        }

        #endregion PROPERTIES
    }
}
