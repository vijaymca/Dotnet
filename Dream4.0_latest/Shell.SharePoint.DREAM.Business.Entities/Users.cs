#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: Users.cs 
#endregion
/// <summary>
/// This class is used to create DataOwners object which contians collection of dataowner object
/// </summary>
using System.Collections;

namespace Shell.SharePoint.DREAM.Business.Entities
{
    /// <summary>
    /// The Users class.
    /// </summary>
    public class Users
    {
        #region DECLARATION
        private ArrayList arlDataOwner = null;
        #endregion


        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Users"/> class.
        /// </summary>
        public Users()
        {
            arlDataOwner = new ArrayList();

        }
        #endregion

        #region Indexer
        /// <summary>
        /// Gets or Sets specific User in the Users class
        /// </summary>
        /// <param name="intIndex">Index</param>
        /// <returns>User object</returns>
        public User this[int intIndex]
        {
            get
            {
                if ((intIndex >= 0) && (intIndex < arlDataOwner.Count))
                {
                    return (User)arlDataOwner[intIndex];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if ((intIndex >= 0) && (intIndex < arlDataOwner.Count))
                {
                    arlDataOwner[intIndex] = (User)value;
                }

            }
        }
        #endregion

        #region Public Method
        /// <summary>
        /// Adds a User
        /// </summary>
        /// <param name="user">User object to be added to Users collection</param>
        public void Add(User user)
        {
            arlDataOwner.Add(user);

        }

        /// <summary>
        /// Removes a User
        /// </summary>
        /// <param name="user">User object ot be removed from the Users collection</param>
        public void Remove(User user)
        {
            arlDataOwner.Remove(user);
        }

        #endregion

        #region PROPERTIES
        /// <summary>
        /// Gets  the count of users.
        /// </summary>
        /// <value>Count</value>
        public int Count
        {
            get
            {
                return arlDataOwner.Count;
            }
        }
        #endregion
    }
}
