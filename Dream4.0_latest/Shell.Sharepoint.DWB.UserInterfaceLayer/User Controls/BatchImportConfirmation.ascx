<%@ Control Language="C#" AutoEventWireup="true" Codebehind="BatchImportConfirmation.ascx.cs"
    Inherits="Shell.SharePoint.DWB.UserInterfaceLayer.BatchImportConfirmation" %>
<link href="/_LAYOUTS/DREAM/styles/DREAMStyleSheetRel2_1.css" rel="stylesheet" type="text/css" />
<link href="/_LAYOUTS/DREAM/styles/DWBStyleSheetRel2_0.css" rel="stylesheet" type="text/css" />

<script language="javascript" src="/_Layouts/DREAM/Javascript/DWBJavascriptFunctionRel2_0.js"></script>

<table width="100%">
    <tr>
        <td>
            <div style="border: Solid 1px #dedede;">
                <table width="100%">
                    <tr>
                        <td style="text-align: left; width: 100%; padding: 2px 2px 2px 2px; vertical-align: top;"
                            class="DWBtdAdvSrchSubHeader">
                            <b>
                                <asp:Label ID="lblBookName" runat="server" Text="Batch Import Confirmation : " /></b></td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 100%;">
                            <asp:Panel ID="ExceptionBlock" Visible="false" runat="server">
                                <asp:Label ID="lblException" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label>
                                <br />
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 100%;">
                            <asp:Panel ID="pnlGrid" runat="server" Width="100%" ScrollBars="Auto" CssClass="grdPanelStyle">
                                <asp:GridView ID="gvBatchImportConfirmation" AutoGenerateColumns="False" runat="server"
                                    GridLines="Both" EmptyDataText="No Data to Load"  ShowFooter="False" Width="98%"
                                    CssClass="DWBWellBookSummaryGridViewCSS" OnRowDataBound="gvBatchImportConfirmation_RowDataBound" EnableViewState="true" HeaderStyle-Height="20px"
                                    RowStyle-CssClass="evenRowStyle" AlternatingRowStyle-CssClass="oddRowStyle" HeaderStyle-CssClass="ResultFixedHeader">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="3%" HeaderStyle-Width="3%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chbSelectID" runat="server"  />
                                            </ItemTemplate>
                                            <HeaderTemplate>
                                                <input id="chbHeader" onclick="javascript:return gvHeaderSelectAll(this);" type="checkbox" />
                                            </HeaderTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PageName" HeaderText="Page Name" ItemStyle-Width="35%"
                                            HeaderStyle-Width="35%" HeaderStyle-HorizontalAlign="Center"/>
                                        <asp:BoundField DataField="CompleteSharedPath" HeaderText="Shared Area Path" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="NoOfPages" HeaderText="No. of Pages" ItemStyle-Width="10%"
                                            HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" />
                                         <asp:BoundField DataField="FileFormat" HeaderStyle-CssClass="DWBDlstLeftListHide" ItemStyle-CssClass="DWBDlstLeftListHide" />
                                         <asp:BoundField DataField="ActualFileFormat" HeaderStyle-CssClass="DWBDlstLeftListHide" ItemStyle-CssClass="DWBDlstLeftListHide" />                                         
                                         <asp:BoundField DataField="FileType" HeaderStyle-CssClass="DWBDlstLeftListHide" ItemStyle-CssClass="DWBDlstLeftListHide" />
                                         <asp:BoundField DataField="SharedPath" HeaderStyle-CssClass="DWBDlstLeftListHide" ItemStyle-CssClass="DWBDlstLeftListHide" />
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 100%;">
                            <asp:Button ID="btnContinue" runat="server" Text="Continue" OnClick="btnContinue_Click" CssClass="DWBbuttonAdvSrch" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="DWBbuttonAdvSrch"
                                OnClientClick="window.close();return false;" />
                            <br />
                        </td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
</table>
<script language="javascript" type="text/javascript">
var countCheckboxChecked = 0;
var countTotalCheckbox = 0;

for (i=0; i < document.forms[0].elements.length; i++) 
        {                
            if (document.forms[0].elements[i].type == 'checkbox')
            {
                countTotalCheckbox++;
                if (document.forms[0].elements[i].id != 'chbHeader')
                {
                    if(document.forms[0].elements[i].checked == true)
                    {
                        countCheckboxChecked++;
                    }     
                }       
            }
        }
        
        if((countTotalCheckbox-1) == countCheckboxChecked)
        {
            document.getElementById('chbHeader').checked = true;
        }
</script>