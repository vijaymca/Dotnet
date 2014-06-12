<!--
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//
//Filename: FeedbackUI.ascx
-->
<%@ Control Language="C#" AutoEventWireup="true" Codebehind="FeedbackUI.ascx.cs"
    Inherits="Shell.SharePoint.DREAM.Site.UI.UserControls.FeedbackUI" %>
<asp:Panel ID="pnlFeedback" runat="server" Width="100%" CssClass="panelPadding">
    <table border="0" cellpadding="4" cellspacing="0" class="tableAdvSrchBorder" width="100%">
        <tr>
            <td class="tdBasinAdvSrchHeader" valign="top" colspan="2">
                <strong>Feedback</strong></td>
        </tr>
        <tr>
            <td align="left" style="height: 14px" valign="top" colspan="2">
                <asp:Label ID="lblErrorMessage" runat="server" Text="" Visible="false" CssClass="ErrorMessagecss"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="tdAdvSrchItem" width="20%" colspan="2">
                <asp:RadioButtonList ID="rdoFeedback" onclick="javascript:showPageGeneralTable('TR1','TR2','TR3','TR4','TR5','TR6','TR7','TR8','TR9');"
                    CssClass="radiobuttonAdvSrch" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Selected="True">Page Level Feedback</asp:ListItem>
                    <asp:ListItem>General Feedback</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr id="TR1" style="display: none;">
            <td class="tdAdvSrchItem" width="20%" colspan="2">
                How satisfied are you with your experience with DREAM?
            </td>
        </tr>
        <tr id="TR2" style="display: none;">
            <td class="tdAdvSrchItem" valign="top">
                <span style="color: red">*</span></td>
            <td class="tdAdvSrchItem" width="99%">
                <asp:RadioButtonList ID="rdoRating" CssClass="radiobuttonAdvSrch" runat="server">
                    <asp:ListItem>Very satisfied</asp:ListItem>
                    <asp:ListItem>Satisfied</asp:ListItem>
                    <asp:ListItem>Neutral</asp:ListItem>
                    <asp:ListItem>Dissatisfied</asp:ListItem>
                    <asp:ListItem>Very dissatisfied</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr id="TR3" style="display: none;">
            <td class="tdAdvSrchItem" width="20%" colspan="2">
                Please explain the reason for given score.
            </td>
        </tr>
        <tr id="TR4" style="display: none;">
            <td class="tdAdvSrchItem" colspan="2">
                <asp:TextBox ID="txtReasonForRating" TextMode="MultiLine" runat="server" CssClass="textboxAdvSrch"
                    Width="536px" Height="50px"></asp:TextBox>
            </td>
        </tr>
        <tr id="TR5" style="display: none;">
            <td class="tdAdvSrchItem" width="20%" colspan="2">
                What additional information or features would you like to be included on the DREAM?
            </td>
        </tr>
        <tr id="TR6" style="display: none;">
            <td class="tdAdvSrchItem" colspan="2">
                <asp:TextBox ID="txtAdditionalInformation" TextMode="MultiLine" runat="server" CssClass="textboxAdvSrch"
                    Width="536px" Height="50px"></asp:TextBox>
            </td>
        </tr>
        <tr id="TR7">
            <td class="tdAdvSrchItem" width="20%" colspan="2" style="padding-left: 12px;">
                Page Name : &nbsp;&nbsp;
                <asp:DropDownList ID="ddlPageName" CssClass="dropdownAdvSrch" runat="server" Width="217px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr id="TR8">
            <td class="tdAdvSrchItem" colspan="2">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <div id="TR10" style="display: none">
                                <table style="font-size: 10px">
                                    <tr>
                                        <td id="tdUploadfiles" style="word-wrap: break-word; width: 200px">
                                            <a id="lnkFile" href="javascript:void(0)" onclick="javascript:OpenFileViewer();"></a>
                                        </td>
                                        <td id="td1">
                                            &nbsp;<a id="lnkRemove" href="javascript:void(0)" onserverclick="ClearAttachedFile"
                                                runat="server">Remove File</a>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <td>
                            <div>
                                <table style="font-size: 10px">
                                    <tr>
                                        <td style="padding-left: 7px;">
                                            <input type="button" id="btnUpload" class="buttonAdvSrch" onclick="javascript:AttachFileToUpload();"
                                                value="Attach Files" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="TR9" style="display: block">
            <td class="tdAdvSrchItem" colspan="2" style="padding-left: 12px;">
                Comments :<br />
                <asp:TextBox ID="txtPageLevelComment" TextMode="MultiLine" runat="server" CssClass="textboxAdvSrch"
                    Width="536px" Height="114px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="center" height="32px" colspan="2">
                <asp:Button ID="cmdSubmit" runat="server" CssClass="buttonAdvSrch" Text="Submit"
                    Width="77px" OnClick="BtnSubmit_Click" OnClientClick="return ValidateFeedback()" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="cmdReset" runat="server" CssClass="buttonAdvSrch" Text="Reset" Width="77px"
                    OnClick="CmdReset_Click" />
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="pnlConfirmFeedback" runat="server" Width="100%" Visible="false" CssClass="panelPadding">
    <table border="0" cellpadding="4" cellspacing="0" class="tableAdvSrchBorder" width="100%">
        <tr>
            <td class="tdBasinAdvSrchHeader" height="18px">
                Feedback</td>
        </tr>
        <tr>
            <td>
                <br />
                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                <br />
                <br />
                <br />
                <div align="center">
                    <asp:Button ID="cmdClose" runat="server" CssClass="buttonAdvSrch" Text="Close" OnClientClick="window.close();"
                        Width="60px" />
                </div>
                <br />
            </td>
        </tr>
    </table>
</asp:Panel>
<input type="hidden" id="hidFileName" runat="server" />
<input type="hidden" id="hidFilePath" runat="server" />
