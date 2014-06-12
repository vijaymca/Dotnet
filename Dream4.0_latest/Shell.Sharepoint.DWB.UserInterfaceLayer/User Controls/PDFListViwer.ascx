<%@ Control Language="C#" AutoEventWireup="true" Codebehind="PDFListViwer.ascx.cs"
  Inherits="Shell.SharePoint.DWB.UserInterfaceLayer.PDFListViwer" %>
<link href="/_Layouts/DREAM/Styles/DWBStyleSheetRel2_0.css" rel="stylesheet" type="text/css" />
<script language="javascript" type="text/javascript" src="/_layouts/DREAM/Javascript/DWBJavascriptFunctionRel2_0.js"></script>
<asp:Panel ID="ExceptionBlock" Visible="false" runat="server">
  <br />
  <asp:Label ID="lblException" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label>
  <br />
</asp:Panel>
<div>
  <table width="700px" class="tableAdvSrchBorder" border="0" cellSpacing="0" cellPadding="4">
    <tr width="700px">
      <td class="DWBtdAdvSrchHeader" valign="top">
        <b><asp:Label ID="lblHeader" runat="server"></asp:Label></b>
      </td>
    </tr>
    <tr>
      <td>
        <table width="100%" class="scrollContent">
          <tr class="DWBtdAdvSrchSubHeader" style="height: 20px;" >
            <td>
             <asp:Label ID="lblHeaderPublishedDate" runat="server" Text="Published Date"></asp:Label>
            </td>
            <td>             
                <asp:Label ID="lblHeaderBookName" runat="server" Text="Published Book Name"></asp:Label>
            </td>
          </tr>
          <asp:Repeater ID="rptrFileslist" runat="server">
            <ItemTemplate>
              <tr class="oddRowStyle">
               <td>
                  <%# Eval("PublishedDate")%>
                </td>
                <td>
                  <A onClick="Javascript:window.open('<%# Eval("FileURL")%>');" 
                  onMouseOver="this.style.cursor='hand'" > <%# Eval("FileName")%> </A>
                </td>
               
              </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
              <tr class="evenRowStyle">
               <td>
                  <%# Eval("PublishedDate")%>
                </td>
                <td>
                  <A onClick="Javascript:window.open('<%# Eval("FileURL")%>');"
                  onMouseOver="this.style.cursor='hand'" > <%# Eval("FileName")%> </A>
                  <%--<asp:HyperLink NavigateUrl='<%#"~/"+ Eval("FileURL")%>' Text='<%# Eval("FileName")%>'
                    runat="server" ID="Hyperlink" />--%>
                </td>               
              </tr>
            </AlternatingItemTemplate>
          </asp:Repeater>
        </table>
      </td>
    </tr>
  </table>
</div>
