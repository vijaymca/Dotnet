#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: UserPreferences.cs 
#endregion

/// <summary> 
/// This class has get/set methods to handle user preferences value 
/// </summary>
using System;
using System.Collections;

namespace Shell.SharePoint.DREAM.Business.Entities
{
    /// <summary>
    /// The UserPreferences class
    /// </summary>
    [Serializable]
    public class UserPreferences
    {
        #region DECLARATION
        private string strDisplay = string.Empty;
        private string strDepthUnits = string.Empty;
        private string strCountry = string.Empty;
        private string strAsset = string.Empty;
        private string strRecordsPerPage = string.Empty;
        private string strBasin = string.Empty;
        private string strField = string.Empty;
        //Dream 3.0 code
        //Start
        private string strPressureUnits = string.Empty;
        private string strTemperatureUnits = string.Empty;
        //End
        private ArrayList arlURLobj = new ArrayList();
        #endregion
        #region PROPERTIES
        /// <summary>
        /// Property to Gets or sets the Display.
        /// </summary>
        /// <value>The display.</value>
        public string Display
        {
            get
            {
                return strDisplay;
            }
            set
            {
                strDisplay = value;
            }
        }

        /// <summary>
        /// Gets or sets the depth units.
        /// </summary>
        /// <value>The depth units.</value>
        public string DepthUnits
        {
            get
            {
                return strDepthUnits;
            }
            set
            {
                strDepthUnits = value;
            }
        }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>The country.</value>
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

        /// <summary>
        /// Property to Gets or sets the Asset.
        /// </summary>
        /// <value>The asset.</value>
        public string Asset
        {
            get
            {
                return strAsset;
            }
            set
            {
                strAsset = value;
            }
        }

        /// <summary>
        /// Property to Gets or sets the RecordsPerPage.
        /// </summary>
        /// <value>The records per page.</value>
        public string RecordsPerPage
        {
            get
            {
                return strRecordsPerPage;
            }
            set
            {
                strRecordsPerPage = value;
            }
        }

        /// <summary>
        /// Gets or sets the basin.
        /// </summary>
        /// <value>The basin.</value>
        public string Basin
        {
            get
            {
                return strBasin;
            }
            set
            {
                strBasin = value;
            }
        }

        /// <summary>
        /// Gets or sets the field.
        /// </summary>
        /// <value>The field.</value>
        public string Field
        {
            get
            {
                return strField;
            }
            set
            {
                strField = value;
            }
        }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public ArrayList URL
        {
            get
            {
                return arlURLobj;
            }
            set
            {
                arlURLobj = value;
            }
        }
        //Dream 3.0 Code
        //Start

        /// <summary>
        /// Gets or sets the pressure units.
        /// </summary>
        /// <value>The pressure units.</value>
        public string PressureUnits
        {
            get
            {
                return strPressureUnits;
            }
            set
            {
                strPressureUnits = value;
            }
        }
        /// <summary>
        /// Gets or sets the temperature units.
        /// </summary>
        /// <value>The temperature units.</value>
        public string TemperatureUnits
        {
            get
            {
                return strTemperatureUnits;
            }
            set
            {
                strTemperatureUnits = value;
            }
        }
        //End
#endregion
    }
}
