<%@ Page language="C#"  MasterPageFile="ExternalSite.master" Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage, Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
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
                      <telerik:RadSplitter ID="splitterMainExternalSite" runat="server"  Width="100%" Height="1900" Orientation="Vertical" VisibleDuringInit="false">
             <telerik:RadPane ID="radPaneMain" runat="server"  Scrolling="none" ContentUrl="http://amsdc1-s-7606.europe.shell.com/SiestaBuild4/siesta_Main/Pressuresurvey.aspx" >
             
            </telerik:RadPane>
            </telerik:RadSplitter>
	                   </td>
                </tr>
            </table>
            </PlaceHolder>
         </td>
    </tr>
</table>
</asp:Content>
<asp:Content ContentPlaceholderID="PlaceHolderTitleBreadcrumb" runat="server"/>
<asp:Content ContentPlaceholderID="PlaceHolderLeftNavBar" runat="server"/>
<asp:Content ContentPlaceHolderId="PlaceHolderPageImage" runat="server" />
<asp:Content ContentPlaceholderID="PlaceHolderNavSpacer" runat="server" />
