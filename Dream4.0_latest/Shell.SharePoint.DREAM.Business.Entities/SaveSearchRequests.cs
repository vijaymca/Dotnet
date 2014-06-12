#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: SaveSearchRequests.cs 
#endregion
/// <summary>
/// This class gets / sets the SaveSearchRequest object.
/// </summary>
using System.Collections;

namespace Shell.SharePoint.DREAM.Business.Entities
{
    /// <summary>
    /// The SaveSearchRequests class.
    /// </summary>
    public class SaveSearchRequests
    {
        #region DECLARATION
        private ArrayList arlSaveSearchRequest = null;
        #endregion
        #region PROPERTIES
        /// <summary>
        /// Gets or sets the SaveSearchRequest.
        /// </summary>
        /// <value>The SaveSearchRequest.</value>
        public ArrayList SaveSearchRequest
        {
            get
            {
                return arlSaveSearchRequest;
            }
            set
            {
                arlSaveSearchRequest = value;
            }
        }
        #endregion
    }
}
