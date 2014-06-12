#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename      : CopyrightToolpart.cs
#endregion
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;

namespace Shell.SharePoint.WebParts.DREAM
{
	/// <summary>
	/// Description of the toolpart. Override the GetToolParts method in your WebPart
	/// class to invoke this toolpart. To establish a reference to the Web Part 
	/// the user has selected, use the ParentToolPane.SelectedWebPart property.
	/// </summary>
	public class CopyrightToolpart: Microsoft.SharePoint.WebPartPages.ToolPart
	{
		/// <summary>
		/// Constructor for the class.
		/// </summary>
		public CopyrightToolpart()
		{
			   this.Title = "Copyright";
		}		

		/// <summary>
		/// Render this tool part to the output parameter specified.
		/// </summary>
		/// <param name="output">The HTML writer to write out to </param>
		protected override void RenderToolPart(HtmlTextWriter output)
		{
			output.Write("Created by DREAM Portal<br>");
			output.Write("© 2004 <a href='http://www.shell.com' target='_blank'>Shell IT</a>. All rights reserved.");
		}
	}											
}
								
