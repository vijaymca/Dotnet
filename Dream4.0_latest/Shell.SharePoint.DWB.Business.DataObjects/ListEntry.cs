#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: ListEntry.cs 
#endregion

using System.Collections;
using System.Collections.Generic;

namespace Shell.SharePoint.DWB.Business.DataObjects
{
    /// <summary>
    /// ListEntry class
    /// </summary>
    public class ListEntry
    {
        #region DECLARATION
        MasterPageDetails objMasterPage;
        TemplateDetails objTemplate;
        WellBookDetails objWellBookDetails;
        ChapterDetails objChapterDetails;
        ArrayList arlTemplateConfiguration;
        List<ChapterPagesMapping> objChapterPagesMapping;
        List<StoryBoard> objPageStoryBoard;
        UserDetails objUserDetails;
        TeamDetails objTeamDetails;
        StaffDetails objStaffDetails;
        PageCommentsDetails objPageCommentsDetails;
        ArrayList arlStaffs;
        ArrayList arlTeams;
        #endregion

        #region PROPERTIES

        /// <summary>
        /// Gets or sets the ChapterPagesMapping collection.
        /// </summary>
        /// <value>List of  chapter pages mapping.</value>
        public List<ChapterPagesMapping> ChapterPagesMapping 
        {
              get
            {
                return objChapterPagesMapping;
            }
            set
            {
                objChapterPagesMapping = value;
            }
        }

        /// <summary>
        /// Gets or sets the TemplateConfiguration collection.
        /// </summary>
        /// <value>The template configuration.</value>
        public ArrayList TemplateConfiguration
        {
            get
            {
                return arlTemplateConfiguration;
            }
            set
            {
                arlTemplateConfiguration = value;
            }
        }

        /// <summary>
        /// Gets or sets the MasterPageDetails object.
        /// </summary>
        /// <value>The master page.</value>
        public MasterPageDetails MasterPage
        {
            get
            {
                return objMasterPage;
            }
            set
            {
                objMasterPage = value;
            }
        }

        /// <summary>
        /// Gets or sets the page StoryBoard collection.
        /// </summary>
        /// <value>List of page story board.</value>
        public List<StoryBoard> PageStoryBoard
        {
            get
            {
                return objPageStoryBoard;
            }
            set
            {
                objPageStoryBoard = value;
            }
        }

        /// <summary>
        /// Gets or sets the TemplateDetails object.
        /// </summary>
        /// <value>The template details.</value>
        public TemplateDetails TemplateDetails
        {
            get
            {
                return objTemplate;
            }
            set
            {
                objTemplate = value;
            }
        }

        /// <summary>
        /// Gets or sets the WellBookDetails object.
        /// </summary>
        /// <value>The well book details.</value>
        public WellBookDetails WellBookDetails
        {
            get
            {
                return objWellBookDetails;
            }
            set
            {
                objWellBookDetails = value;
            }
        }

        /// <summary>
        /// Gets or sets the ChapterDetails object.
        /// </summary>
        /// <value>The chapter details.</value>
        public ChapterDetails ChapterDetails
        {
            get
            {
                return objChapterDetails;
            }
            set
            {
                objChapterDetails = value;
            }
        }

        /// <summary>
        /// Gets or sets the UserDetails object.
        /// </summary>
        /// <value>The user details.</value>
        public UserDetails UserDetails
        {
            get
            {
                return objUserDetails;
            }
            set
            {
                objUserDetails = value;
            }
        }

        /// <summary>
        /// Gets or sets the TeamDetails object.
        /// </summary>
        /// <value>The team details.</value>
        public TeamDetails TeamDetails
        {
            get
            {
                return objTeamDetails;
            }
            set
            {
                objTeamDetails = value;
            }
        }

        /// <summary>
        /// Gets or sets the StaffDetails object.
        /// </summary>
        /// <value>The staff details.</value>
        public StaffDetails StaffDetails
        {
            get
            {
                return objStaffDetails;
            }
            set
            {
                objStaffDetails = value;
            }
        }

        /// <summary>
        /// Gets or sets the Staff objects collection.
        /// </summary>
        /// <value>The staffs.</value>
        public ArrayList Staffs
        {
            get
            {
                return arlStaffs;
            }
            set
            {
                arlStaffs = value;
            }
        }

        /// <summary>
        /// Gets or sets the Team objects collection.
        /// </summary>
        /// <value>The teams.</value>
        public ArrayList Teams
        {
            get
            {
                return arlTeams;
            }
            set
            {
                arlTeams = value;
            }
        }

        /// <summary>
        /// Gets or sets the PageCommentsDetails object.
        /// </summary>
        /// <value>The page comments.</value>
        public PageCommentsDetails PageComments
        {
            get
            {
                return objPageCommentsDetails;
            }
            set
            {
                objPageCommentsDetails = value;
            }
        }
        #endregion
        
    }
}
