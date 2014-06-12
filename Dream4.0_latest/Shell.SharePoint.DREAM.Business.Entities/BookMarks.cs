#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: BookMarks.cs 
#endregion

/// <summary> 
/// This is BookMarks class. This class is used to create BookMarks  object for generating request info object
/// </summary>
using System.Collections;

namespace Shell.SharePoint.DREAM.Business.Entities
{
    /// <summary>
    /// BookMarks class
    /// </summary>
    public class BookMarks
    {
        #region DECLARATION
        private ArrayList arlBookMark = null;
        #endregion

        #region PROPERTIES
        /// <summary>
        /// Gets or sets the BookMark object value.
        /// </summary>
        /// <value>The report.</value>
        public ArrayList BookMark
        {
            get
            {
                return arlBookMark;
            }
            set
            {
                arlBookMark = value;
            }
        }
        #endregion
    }

}
