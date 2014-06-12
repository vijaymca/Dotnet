<%@ Page language="C#"   Inherits="Microsoft.SharePoint.Publishing.PublishingLayoutPage,Microsoft.SharePoint.Publishing,Version=12.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" meta:progid="SharePoint.WebPartPage.Document" %>
<%@ Register Tagprefix="SharePointWebControls" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="PublishingWebControls" Namespace="Microsoft.SharePoint.Publishing.WebControls" Assembly="Microsoft.SharePoint.Publishing, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="PublishingNavigation" Namespace="Microsoft.SharePoint.Publishing.Navigation" Assembly="Microsoft.SharePoint.Publishing, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register tagprefix="LeftNavigation" namespace="Shell.SharePoint.WebParts.DREAM.LeftNavigation" assembly="Shell.SharePoint.WebParts.DREAM.LeftNavigation" %>
<%@ Register Assembly="Telerik.Web.UI, Version=2010.1.415.35, Culture=neutral,PublicKeyToken=121fae78165ba3d4" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ContentPlaceholderID="PlaceHolderAdditionalPageHead" runat="server">
	<style type="text/css">
		.ms-pagetitleareaframe table, .ms-titleareaframe
		{
			background: none;
			height: 10px;
			overflow:hidden;
		}
		.ms-pagetitle, .ms-titlearea
		{
			display:none;
		}
	</style>
	<PublishingWebControls:editmodepanel runat="server" id="editmodestyles">
			<!-- Styles for edit mode only-->
			<SharePointWebControls:CssRegistration name="<% $SPUrl:~sitecollection/Style Library/~language/Core Styles/zz2_editMode.css %>" runat="server"/>
			<SharePointWebControls:CssRegistration ID="CssRegistration1" name="<% $SPUrl:~sitecollection/Style Library/~language/Core Styles/pageLayouts.css %>" runat="server"/>
	</PublishingWebControls:editmodepanel>
</asp:Content>
<asp:Content ContentPlaceholderID="PlaceHolderPageTitle" runat="server">
	<SharePointWebControls:FieldValue id="HomePageTitleInTitleArea" FieldName="Title" runat="server"/>
</asp:Content>
<asp:Content ContentPlaceholderID="PlaceHolderMain" runat="server">
<WebPartPages:SPProxyWebPartManager runat="server" id="ProxyWebPartManager"></WebPartPages:SPProxyWebPartManager>
<table id="BodyTable" width="100%" cellpadding="0" cellspacing="0" border="0">
    <tr>
         <td class='ms-bodyareaframe123' valign="top" style="height:100%;">
            <PlaceHolder id="MSO_ContentDiv" runat="server">
            <table id="MSO_ContentTable" style="width:100%;height:100%;" border="0" cellspacing="0" cellpadding="0" class="123ms-propertysheet">
                <tr >
	                <td valign="top" style="width:100%;height:100%;">	              
                      <telerik:RadSplitter ID="splitterMain" runat="server"   Width="100%" Orientation="Vertical" OnClientLoad="OnRadSplitterLoaded" VisibleDuringInit="false">
            <telerik:RadPane ID="radPaneLeft" runat="server" Width="156px"  Scrolling="None" BackColor="#FCFEDF">
            <!--Left Navigation Starts here-->
				        <LeftNavigation:LeftNavigation runat="server" ID="LeftNavigation1" Title="LeftNavigation" __MarkupType="vsattributemarkup" __WebPartId="{26d75881-643c-4f8b-88f4-2b2b6a0fa18e}" WebPart="true" __designer:IsClosed="false"></LeftNavigation:LeftNavigation>
				<!--Left Navigation Ends here-->
            </telerik:RadPane>
            <telerik:RadSplitBar ID="radSplitBarMain" runat="server"
             CollapseMode="Forward" EnableResize="false"></telerik:RadSplitBar>
             <telerik:RadPane ID="radPaneRight" runat="server"  Scrolling="None">
              <WebPartPages:WebPartZone runat="server" AllowPersonalization="false" ID="TopZone" FrameType="TitleBarOnly"
									                Title="Top Content Area" Orientation="Vertical"><ZoneTemplate></ZoneTemplate></WebPartPages:WebPartZone>	
            </telerik:RadPane>
            </telerik:RadSplitter>
	                   </td>
                </tr>
            </table>
            </PlaceHolder>
         </td>
    </tr>
</table>
	<!-- <PublishingWebControls:editmodepanel runat="server" id="editmodepanel1" __designer:Preview="&lt;Regions&gt;&lt;Region Name=&quot;0&quot; Editable=&quot;True&quot; Content=&quot;&amp;#xD;&amp;#xA;		 Add field controls here to bind custom metadata viewable and editable in edit mode only.&amp;#xD;&amp;#xA;		&amp;lt;table cellpadding=&amp;quot;10&amp;quot; cellspacing=&amp;quot;0&amp;quot; align=&amp;quot;center&amp;quot; class=&amp;quot;editModePanel&amp;quot;&amp;gt;&amp;#xD;&amp;#xA;			&amp;lt;tr&amp;gt;&amp;#xD;&amp;#xA;				&amp;lt;td&amp;gt;&amp;#xD;&amp;#xA;					&amp;lt;SharePointWebControls:TextField runat=&amp;quot;server&amp;quot; id=&amp;quot;TitleField&amp;quot; FieldName=&amp;quot;Title&amp;quot; __designer:Preview=&amp;quot;&amp;amp;lt;div align=&amp;amp;quot;left&amp;amp;quot; class=&amp;amp;quot;ms-formfieldcontainer&amp;amp;quot;&amp;amp;gt;&amp;amp;lt;div class=&amp;amp;quot;ms-formfieldlabelcontainer&amp;amp;quot; nowrap=&amp;amp;quot;nowrap&amp;amp;quot;&amp;amp;gt;&amp;amp;lt;span class=&amp;amp;quot;ms-formfieldlabel&amp;amp;quot; nowrap=&amp;amp;quot;nowrap&amp;amp;quot;&amp;amp;gt;Title&amp;amp;lt;/span&amp;amp;gt;&amp;amp;lt;/div&amp;amp;gt;&amp;amp;lt;div class=&amp;amp;quot;ms-formfieldvaluecontainer&amp;amp;quot;&amp;amp;gt;Dream Home&amp;amp;lt;/div&amp;amp;gt;&amp;amp;lt;/div&amp;amp;gt;&amp;quot; __designer:Values=&amp;quot;&amp;amp;lt;P N='ID' T='TitleField' /&amp;amp;gt;&amp;amp;lt;P N='ItemFieldValue' ID='1' Serial='AAEAAAD/////AQAAAAAAAAAGAQAAAApEcmVhbSBIb21lCw' /&amp;amp;gt;&amp;amp;lt;P N='ListItemFieldValue' R='1' /&amp;amp;gt;&amp;amp;lt;P N='Visible' T='True' /&amp;amp;gt;&amp;amp;lt;P N='FieldName' T='Title' /&amp;amp;gt;&amp;amp;lt;P N='ControlMode' E='1' /&amp;amp;gt;&amp;amp;lt;P N='InDesign' T='False' /&amp;amp;gt;&amp;amp;lt;P N='Page' ID='2' /&amp;amp;gt;&amp;amp;lt;P N='TemplateControl' R='2' /&amp;amp;gt;&amp;amp;lt;P N='AppRelativeTemplateSourceDirectory' R='-1' /&amp;amp;gt;&amp;quot;/&amp;gt;&amp;#xD;&amp;#xA;				&amp;lt;/td&amp;gt;&amp;#xD;&amp;#xA;			&amp;lt;/tr&amp;gt;&amp;#xD;&amp;#xA;		&amp;lt;/table&amp;gt;&amp;#xD;&amp;#xA;	&quot; /&gt;&lt;/Regions&gt;&lt;table height=&quot;&quot; width=&quot;&quot; style=&quot;color:Black; ;&quot; cellpadding=1 cellspacing=0&gt;
                &lt;tr&gt;
                    &lt;td nowrap align=center valign=middle style=&quot;color:Black; background-color:LightBlue; font-family:Tahoma; font-size:X-Small; &quot;&gt;Edit Mode Panel&lt;/td&gt;
                &lt;/tr&gt;
                &lt;tr&gt;
                    &lt;td nowrap style=&quot;vertical-align:top;;&quot; _designerRegion=0&gt;&lt;/td&gt;
                &lt;/tr&gt;
            &lt;/table&gt;" __designer:Values="&lt;P N='ID' ID='1' T='editmodepanel1' /&gt;&lt;P N='Font' ID='2' /&gt;&lt;P N='Page' ID='3' /&gt;&lt;P N='TemplateControl' R='3' /&gt;&lt;P N='AppRelativeTemplateSourceDirectory' R='-1' /&gt;&lt;P N='Visible' T='False' /&gt;">
		 Add field controls here to bind custom metadata viewable and editable in edit mode only.
		<table cellpadding="10" cellspacing="0" align="center" class="editModePanel">
			<tr>
				<td>
					<SharePointWebControls:TextField runat="server" id="TitleField" FieldName="Title"/>
				</td>
			</tr>
		</table>
	</PublishingWebControls:editmodepanel>-->
</asp:Content>
<asp:Content ContentPlaceholderID="PlaceHolderTitleBreadcrumb" runat="server"/>
<asp:Content ContentPlaceholderID="PlaceHolderLeftNavBar" runat="server"/>
<asp:Content ContentPlaceHolderId="PlaceHolderPageImage" runat="server" />
<asp:Content ContentPlaceholderID="PlaceHolderNavSpacer" runat="server" />
