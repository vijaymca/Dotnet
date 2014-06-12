<%@ Page Language="C#" MasterPageFile="DreamContent.master" EnableViewStateMac="false"
    Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage, Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Register TagPrefix="SharePointWebControls" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="PublishingWebControls" Namespace="Microsoft.SharePoint.Publishing.WebControls"
    Assembly="Microsoft.SharePoint.Publishing, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="PublishingNavigation" Namespace="Microsoft.SharePoint.Publishing.Navigation"
    Assembly="Microsoft.SharePoint.Publishing, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="LeftNavigation" Namespace="Shell.SharePoint.WebParts.DREAM.LeftNavigation"
    Assembly="Shell.SharePoint.WebParts.DREAM.LeftNavigation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
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
    <PublishingWebControls:EditModePanel runat="server" ID="editmodestyles">
			<!-- Styles for edit mode only-->
			<SharePointWebControls:CssRegistration ID="CssRegistration1" name="<% $SPUrl:~sitecollection/Style Library/~language/Core Styles/zz2_editMode.css %>" runat="server"/>
			<SharePointWebControls:CssRegistration ID="CssRegistration2" name="<% $SPUrl:~sitecollection/Style Library/~language/Core Styles/pageLayouts.css %>" runat="server"/>
    </PublishingWebControls:EditModePanel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    <SharePointWebControls:FieldValue ID="HomePageTitleInTitleArea" FieldName="Title"
        runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <table id="BodyTable" width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td class='ms-bodyareaframe123' valign="top" style="height: 100%; width: 100%" valign="top">
                <placeholder id="MSO_ContentDiv" runat="server">
            <table id="MSO_ContentTable" height="100%" width=100% border="0" cellspacing="0" cellpadding="0" class="ms-propertysheet">
                <tr>
	                <td valign="top">   
	                  <asp:UpdatePanel runat="server" ID="UpdatePanelContentPage" UpdateMode="Conditional" ChildrenAsTriggers="true" >        
            <ContentTemplate>   
                        <WebPartPages:WebPartZone runat="server" AllowPersonalization="false" ID="TopZone" FrameType="TitleBarOnly"
									                Title="Top Content Area" Orientation="Vertical"><ZoneTemplate></ZoneTemplate></WebPartPages:WebPartZone>	
									                    </ContentTemplate>
        </asp:UpdatePanel>
	                   </td>
                </tr>
            </table>
            </placeholder>
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">
    window.onload = function()     
        {     
           window.parent.OnContentWindowLoad();     
        } 
  RegisterPageRequestManagerEventsForSrchResults(); 
    </script>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PlaceHolderTitleBreadcrumb" runat="server" />
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderLeftNavBar" runat="server" />
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderPageImage" runat="server" />
<asp:Content ID="Content7" ContentPlaceHolderID="PlaceHolderNavSpacer" runat="server" />
