<%@ Control Language="C#" AutoEventWireup="true" Codebehind="ReservoirChronostratPopup.ascx.cs"
    Inherits="Shell.SharePoint.DREAM.Site.UI.UserControls.ReservoirChronostratPopup" %>

<script type="text/javascript" language="javascript" src="/_Layouts/DREAM/Javascript/SRPJavaScriptFunctionsRel3_0.js"></script>

<br />
<table>
    <tr>
        <td align="left">
            <span class="shell-SRPBoldtext"><b>&nbsp;Chronostratigraphic Name</b></span>
            <br />
            <br />
            <span>&nbsp;Select Chronostratigraphic Name from the chart below and click on Confirm.</span>
            <br />
            <br />
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top" class="shell-FormItemText" style="width: 201px">
                        <span>
                            <asp:Label ID="lblChrono" runat="server" Font-Bold="true"></asp:Label></span>
                    </td>
                    <td valign="top">
                        &nbsp;&nbsp;<asp:Button CssClass="shell-buttonAlternate" ID="btnConfirm" runat="server"
                            Font-Bold="true" Text=" Confirm " />
                        &nbsp;&nbsp;&nbsp;
                        <%--   <input type="button" class="shell-buttonAlternate" name="btClose" value="  Close  "
                            style="font-weight: bold;" onclick="Javascript:window.close();" />--%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <input type="hidden" id="hidFldChronostrat" runat="server" /></td>
                </tr>
            </table>
            <asp:ImageMap ID="imgMapChronostrat" runat="server" ImageUrl="/_layouts/DREAM/images/ChronostratDiagram_01.gif"
                HotSpotMode="Navigate">
            </asp:ImageMap>
            <br />
        </td>
    </tr>
</table>

<script type="text/javascript">
  
setWindowTitle('Reservoir Chronostratigraphic Search');
</script>

