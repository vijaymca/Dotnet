using System;
using System . Collections . Generic;
using System . Linq;
using System . Web;
using System . Web . UI;
using System . Web . UI . WebControls;
using System . Data;
using System . Data . SqlClient;
using System . IO;
using System . Text;

namespace EnzymeWeb
{
	public partial class PersonnelListingNew : System . Web . UI . Page
	{

		/// <summary>
		/// method:Page_Load
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load ( object sender , EventArgs e )
		{

			if ( !IsPostBack )
			{
				EnzymeBAL.PersonnelListingsBL objPersonnelListingsBL = new EnzymeBAL . PersonnelListingsBL ( );

				DataSet ds = new DataSet ( );
				ds = objPersonnelListingsBL . GetPersonnelListings ( "" );
				grdViewPersonnelListing . DataSource = ds;
				grdViewPersonnelListing . DataBind ( );

				if ( Request . QueryString != null )
				{
					if ( Request . QueryString . Count > 0 )
					{
						if ( Request . QueryString [ "ref" ] != null )
						{
							grdViewPersonnelListing . Columns [ 0 ] . Visible = true;
							grdViewPersonnelListing . Columns [ 1 ] . Visible = false;
							grdViewPersonnelListing . Columns [ 2 ] . Visible = false;
							grdViewPersonnelListing . Columns [ 3 ] . Visible = false;
							grdViewPersonnelListing . Columns [ 4 ] . Visible = false;
							grdViewPersonnelListing . Columns [ 5 ] . Visible = false;
							grdViewPersonnelListing . Columns [ 6 ] . Visible = false;
							grdViewPersonnelListing . Columns [ 7 ] . Visible = false;


							switch ( Request . QueryString [ "ref" ] . ToString ( ) )
							{
								case "enZymes":
									grdViewPersonnelListing . Columns [ 1 ] . Visible = true;
									grdViewPersonnelListing . Columns [ 1 ] . Width = Unit . Pixel ( 500 );
									grdViewPersonnelListing . Columns [ 2 ] . Visible = true;
									grdViewPersonnelListing . Columns [ 2 ] . Width = Unit . Pixel ( 500 );
									break;
								case "SitePhysician":
									grdViewPersonnelListing . Columns [ 2 ] . Visible = true;
									grdViewPersonnelListing . Columns [ 2 ] . Width = Unit . Pixel ( 500 );
									break;
								case "PrincipalAuditReviewer":
									grdViewPersonnelListing . Columns [ 2 ] . Width = Unit . Pixel ( 250 );
									grdViewPersonnelListing . Columns [ 3 ] . Width = Unit . Pixel ( 250 );
									grdViewPersonnelListing . Columns [ 2 ] . Visible = true;
									grdViewPersonnelListing . Columns [ 3 ] . Visible = true;
									break;
								case "HealthServicesPersonnel":
									grdViewPersonnelListing . Columns [ 2 ] . Visible = true;
									grdViewPersonnelListing . Columns [ 3 ] . Visible = true;
									grdViewPersonnelListing . Columns [ 4 ] . Visible = true;
									grdViewPersonnelListing . Columns [ 5 ] . Visible = true;
									grdViewPersonnelListing . Columns [ 6 ] . Visible = true;
									grdViewPersonnelListing . Columns [ 2 ] . Width = Unit . Pixel ( 100 );
									grdViewPersonnelListing . Columns [ 3 ] . Width = Unit . Pixel ( 100 );
									grdViewPersonnelListing . Columns [ 4 ] . Width = Unit . Pixel ( 100 );
									grdViewPersonnelListing . Columns [ 5 ] . Width = Unit . Pixel ( 100 );
									grdViewPersonnelListing . Columns [ 6 ] . Width = Unit . Pixel ( 100 );
									break;
								case "VendorDirectory":
									grdViewPersonnelListing . Columns [ 7 ] . Visible = true;
									//grdViewPersonnelListing . Columns [ 8 ] . Visible = true;
									grdViewPersonnelListing . Columns [ 7 ] . Width = Unit . Pixel ( 250 );
									//grdViewPersonnelListing . Columns [ 8 ] . Width = Unit . Pixel ( 250 );
									break;

							}
						}
					}

				}
			}


		}
		/// <summary>
		/// btnSubmit_Click method 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnSubmit_Click ( object sender , EventArgs e )
		{
			try
			{
				StringBuilder strBld = new StringBuilder ( );  ///taken two string builders for future specific needs
				StringBuilder strRetBld = new StringBuilder ( );
				List<object> fieldValues = grdViewPersonnelListing . GetSelectedFieldValues ( new string [ ] { "HSUPersons_ID" , "Person" } );// , "Person" , "Position" , "Employment_Status" , "FTE_Type" , "Worked_Hours" } );
				// foreach (string[] item in fieldValues)

				foreach ( object[] indField in fieldValues )
				{
					string strParam=string . Empty;
					foreach ( object locFied in indField )
					{
						Type type = locFied . GetType ( );

						if ( type . ToString ( ) == "System.String" )
						{
							strParam = ( string ) locFied;
						}
						else
						{
							int iValue = ( int ) locFied;
							strParam = iValue . ToString ( );
						}
						strBld . Append ( strParam + "~" );
					}
					strRetBld . Append ( strBld );
					strBld . Clear ( );
				}

				if ( fieldValues . Count > 0 )
				{
					Page . ClientScript . RegisterStartupScript ( this . GetType ( ) , "close" , "<script language='JavaScript'> if (window.opener) { window.opener.returnValue = '" + strRetBld . ToString ( ) . Trim ( ':' ) + "';  }window.returnValue = '" + strRetBld . ToString ( ) . Trim ( ':' ) + "';window.close();</script>" , false );

				}
				else
				{
					Page . ClientScript . RegisterStartupScript ( this . GetType ( ) , "Alert" , "alert('Please search and select a user');" , true );
				}
			}
			catch ( Exception ex )
			{
				string strEx = ex . ToString ( );
			}
		}
	}
}