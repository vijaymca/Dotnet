<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AHDTVDPopup.ascx.cs" Inherits="Shell.SharePoint.DREAM.Site.UI.UserControls.AHDTVDPopup" %>
<script src="/_Layouts/DREAM/Javascript/AHTVDCalculatorRel2_1.js" language="javascript" type="text/JavaScript"></script>
    <table id ="tblAHpopup" width ="100%">
        <tr>
            <td width="60%"><span id="spn1" class ="labelMessage3">AH Top Depth</span>
            </td>
            <td width="30%"><asp:TextBox ID ="txtAHTopDepth" CssClass ="AHTVDepthTextBox" runat ="server"></asp:TextBox>
            </td>
            <td width="10%"><span id ="spnAHTopdepth" class ="labelMessage2"></span>
            </td>
        </tr>
        <tr>
            <td width="60%"><span id="Span1" class ="labelMessage3">AH Bottom Depth</span>
            </td>
            <td width="30%"><asp:TextBox ID ="txtAHBottomDepth" CssClass ="AHTVDepthTextBox" runat ="server"></asp:TextBox>
            </td>
            <td width="10%"><span id ="spnAHBottomDepth" class ="labelMessage2"></span>
            
            </td>
        </tr>
        <tr>
            <td width="60%"><span id="Span2" class ="labelMessage3">AH Depth Interval</span>
            </td>
            <td width="30%"><asp:TextBox ID ="txtAHDepthInterval" CssClass ="AHTVDepthTextBox" runat ="server"></asp:TextBox>
            </td>          
             <td width="10%"><span id ="spnAHInterval" class ="labelMessage2"></span>
            </td>
        </tr>
        
        <tr>
        
            <td width="15%" colspan="3" align ="center" >
            <br />
            <asp:Button ID ="btnPopulateDepth" runat ="server" Text ="Populate Depths" Font-Bold ="true" CssClass="buttonAdvSrch" OnClientClick ="return CallAHParentmethod();" OnClick="BtnPopulateDepth_Click"  />
            </td>          
        </tr>   
    </table>

<script language ="javascript">
setWindowTitle('Populate Depths');
var depthReference = getQueryStringValue("rdoValue");
spnAHTopdepth.innerText = depthReference;
spnAHBottomDepth.innerText = depthReference;
spnAHInterval.innerText = depthReference;
</script>