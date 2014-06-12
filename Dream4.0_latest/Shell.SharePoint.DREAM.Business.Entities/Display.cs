#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: Display.cs 
#endregion
/// <summary> 
/// This is Display class. This class is used to create Display object for generating request info object
/// </summary>
using System.Collections;

namespace Shell.SharePoint.DREAM.Business.Entities
{
    /// <summary>
    /// Display class
    /// </summary>
    public class Display
    {
        private ArrayList arlValue = null;
        /// <summary>
        /// Gets or sets the column names.
        /// 
        /// </summary>
        /// <value>The column names.</value>
        public ArrayList Value
        {
            get
            {
                return arlValue;
            }
            set
            {
                arlValue = value;
            }
        }
    }
}
